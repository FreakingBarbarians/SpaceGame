using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using QventSystem;

// Set this script to execute before port
// I'm probably abusing partial here and not using S.R.P properly :(
// oh well.

[Serializable]
public partial class Ship : Damageable, QventHandler {
	public enum ShipState
	{
		NORMAL,
		COMBAT,
		REPAIR
	};

	private HealthBar hpb;

	public ShipState State = ShipState.NORMAL;

	public bool IsPlayer;

	public int ScrapCost;

	public int EnergyMax;
	public int EnergyCur;
    
    // change in rotation
    public float DeltaRotation;
    // maximum traversal factor in degrees
    public float DeltaRotationMax;
    // change in traversal factor
    public float DeltaRotationAcceleration;

    // change in direction
    public Vector2 DeltaPosition;
    // maximum change in direction
    public float DeltaPositionMax;
    // maximum change of change in direction
    public float DeltaPositionFactor;

    public int EnergyRegen;

    private float timer = SpaceGameGlobal.TICK_RATE;
	private float CombatCooldownTimer = 0;

	[SerializeField]
    protected Animator annie;

    public List<Port> ports = new List<Port>();
    public List<Port> mainPorts = new List<Port>();
	public List<Weapon> weapons = new List<Weapon>();

    public void Start()
    {
		
		if (IsPlayer) {
			transform.gameObject.layer = LayerMask.NameToLayer ("Player");

			if (PlayerData.instance.PlayerShip != this) {
				Debug.LogWarning ("More than one PlayerShip");
				return;
			} else {
				PlayerData.instance.PlayerShip = this;
			}
			// add player controller?
		} else {
			transform.gameObject.layer = LayerMask.NameToLayer ("Ship");
		}

		annie = GetComponent<Animator> ();
        // register all of our ports
        foreach (Port p in ports) {
            p.Register(this);
			Module m;
			if((m = p.GetModule())){
				m.RegisterListener(this);
			}
        }

        foreach (Port p in mainPorts) {
			p.Register(this);
			Module m;
			if((m = p.GetModule())){
				m.RegisterListener(this);
			}
		}
		AddHealthBar ();
    }

	public void FixedUpdate(){
		timer -= Time.deltaTime;
		if (timer <= 0) {
			timer += SpaceGameGlobal.TICK_RATE;
			tick ();
		}
        transform.position += (Vector3) DeltaPosition * Time.deltaTime;
        transform.Rotate(0, 0, DeltaRotation * Time.deltaTime);
    }

	public void Fire(int mask) {

		if (State == ShipState.REPAIR) {
			return;
		}

		foreach (Weapon w in weapons) {
			w.UpdateWeaponState (mask);
		}
	}

	private void tick(){
		EnergyCur = Mathf.Min (EnergyMax, EnergyCur + EnergyRegen);

		switch (State) {
		case ShipState.REPAIR:
			Repair (SpaceGameGlobal.TICK_RATE);
			break;
		case ShipState.COMBAT:
			if (CombatCooldownTimer <= 0) {
				SetState (ShipState.NORMAL);
			} else {
				CombatCooldownTimer -= SpaceGameGlobal.TICK_RATE;
			}
			break;
		}
	}

	public override void Repair (float time)
	{
		base.Repair (time);
		foreach (Port p in ports) {
			Module m;
			if ((m = p.GetModule ())) {
				m.Repair (time);			
			}
		}

		foreach (Port p in mainPorts) {
			Module m;
			if ((m = p.GetModule ())) {
				m.Repair (time);			
			}
		}
	}

	public void BeginRepair() {
		if (State == ShipState.COMBAT) {
			return;
		}

		if (DeltaRotation == 0 && DeltaPosition.magnitude == 0) {
			SetState (ShipState.REPAIR);
		}
	}

	public void StopRepair() {
		SetState (ShipState.COMBAT);
	}

	public void SetState(ShipState newState){
		switch(newState){
		case ShipState.REPAIR:
			if (State == ShipState.COMBAT) {
				return;
			}
			State = newState;
			break;
		case ShipState.COMBAT:
			StopRepair ();
			newState = ShipState.COMBAT;
			break;
		default:
			State = newState;
			break;
		}
	}

	protected override void AddHealthBar ()
	{
		GameObject hpBar = WidgetManager.instance.CreateHealthBar ();
		hpBar.transform.position = transform.position;
		hpBar.transform.SetParent (transform);
		HealthBar bar = hpBar.GetComponent<HealthBar> ();
		bar.source = this;
		RegisterListener (bar);

		bar.Refresh ();

		Vector2 offset;

		Sprite s = GetComponent<SpriteRenderer> ().sprite;
		float yoffset =  s.textureRect.size.y / s.pixelsPerUnit;

		hpBar.transform.position += new Vector3 (0, -yoffset/2, 0);
		hpBar.transform.localScale = new Vector3 (0.09f, 0.09f, 1);
		hpb = bar;
	}

    public void Thrust(Vector2 thrustDir)
    {
		
		if (State == ShipState.REPAIR) {
			return;
		}

        DeltaPosition += thrustDir.normalized * Time.deltaTime * DeltaPositionFactor;
        if (DeltaPosition.magnitude > DeltaPositionMax)
        {
            DeltaPosition = DeltaPosition.normalized * DeltaPositionMax;
        }
    }

    public void Rotate(float dir)
    {

		if (State == ShipState.REPAIR) {
			return;
		}

        DeltaRotation += dir * DeltaRotationAcceleration * Time.deltaTime;
        if (Mathf.Abs(DeltaRotation) > DeltaRotationMax)
        {
            if (DeltaRotation < 0)
            {
                DeltaRotation = -DeltaRotationMax;
            }
            else
            {
                DeltaRotation = DeltaRotationMax;
            }
        }
    }

    public void RotateTowards(Vector2 target)
    {

		if (State == ShipState.REPAIR) {
			return;
		}

        DeltaRotation = 0;
        transform.up = Vector3.RotateTowards(transform.up, target, DeltaRotationMax * 3.141f / 180f * Time.deltaTime, 0.0f);
    }

	public void PointWeaponsTowards(Vector2 target) {
		foreach (Weapon w in weapons) {
			w.PointTowards (target);
		}
	}

    public void Brake() {


		if (State == ShipState.REPAIR) {
			return;
		}

        if (DeltaPosition.magnitude <= 0.2) {
            DeltaPosition *= 0;
        }
        if (DeltaRotation <= 2) {
            DeltaRotation = 0;
        }
        DeltaPosition *= (1 - Time.deltaTime);
        DeltaRotation *= (1 - Time.deltaTime);
    }

	public void SetFaction(FACTION faction){
		foreach (Port p in ports) {
			if (p.IsConnected ()) {
				p.GetModule ().faction = faction;
			}
		}

		foreach (Port p in mainPorts) {
			if(p.IsConnected()){
				p.GetModule ().faction = faction;
			}
		}

		this.faction = faction;
	}

	public override void Die ()
	{
		this.enabled = false;
		foreach (Port p in ports) {
			p.Eject ();
		}
		foreach (Port p in mainPorts) {
			p.Eject ();
		}

		int num = UnityEngine.Random.Range (1, 11);

		for (int i = 0; i < num; i++) {
			GameObject debris = WidgetManager.instance.CreateFloatingScrap (ScrapCost/100, transform.position);
			Rigidbody2D rb = debris.GetComponent<Rigidbody2D> ();
			Vector2 away = UnityEngine.Random.insideUnitCircle;
			rb.velocity = away.normalized * UnityEngine.Random.value;
			gameObject.layer = LayerMask.NameToLayer ("PlayerOnly");
			rb.gravityScale = 0;
		}

		Qvent qvent = new Qvent (QventType.DESTROYED, typeof(Ship), this);

		foreach (QventHandler listeners in Listeners) {
			listeners.HandleQvent (qvent);
		}

		GameObject.Destroy (this.gameObject);
	}


	public void HandleQvent(Qvent qvent){
		switch (qvent.QventType) {
		case QventType.DAMAGED:
			Debug.Log ("DAMAGED");
			SetState (ShipState.COMBAT);
			CombatCooldownTimer = SpaceGameGlobal.COMBAT_COOLDOWN;
			break;
		}
	}


    public new static GameObject ReadXml(XmlReader reader, Component workingCO) {
        Ship ship = (Ship)workingCO;
		reader.Read ();
		ship.IsPlayer = XmlUtils.DeserializeBool (reader);

        reader.Read();
        ship.DeltaRotationMax = float.Parse(reader.ReadString());

        reader.Read();
        ship.DeltaRotationAcceleration = float.Parse(reader.ReadString());

        reader.Read();
        ship.DeltaPositionMax = float.Parse(reader.ReadString());

        reader.Read();
        ship.DeltaPositionFactor = float.Parse(reader.ReadString());
        
        reader.Read();
        ship.EnergyMax = int.Parse(reader.ReadString());

        reader.Read();
        ship.EnergyCur = int.Parse(reader.ReadString());

        reader.Read();
        ship.EnergyRegen = int.Parse(reader.ReadString());
        
        return workingCO.gameObject;
    }

    public override void WriteXml(XmlWriter writer) {
        base.WriteXml(writer);

        writer.WriteStartElement("SHIP_DATA");
		XmlUtils.SerializeBool (writer, IsPlayer, "IS_PLAYER");
        writer.WriteElementString("DELTA_ROTATION_MAX", DeltaRotationMax.ToString());
        writer.WriteElementString("DELTA_ROTATION_ACCELERATION", DeltaRotationAcceleration.ToString());
        writer.WriteElementString("DELTA_POSITION_MAX", DeltaPositionMax.ToString());
        writer.WriteElementString("DELTA_POSITION_FACTOR", DeltaPositionFactor.ToString());
        writer.WriteElementString("ENERGY_MAX", EnergyMax.ToString());
        writer.WriteElementString("ENERGY_CUR", EnergyMax.ToString());
        writer.WriteElementString("ENERGY_REGEN", EnergyRegen.ToString());

        writer.WriteEndElement();

        foreach (Port mainPort in mainPorts)
        {
            writer.WriteStartElement("MAIN_PORT");
            if (mainPort.IsConnected())
            {
                SpaceSerializerDeserializer.MyMonoSerializeToStream(writer, mainPort.GetModule());
            }
            else {
				writer.WriteElementString(typeof(Module).Name, "EMPTY");
            }
            writer.WriteEndElement();
        }
        foreach (Port port in ports) {
            writer.WriteStartElement("PORT");
            if (port.IsConnected()) {
                SpaceSerializerDeserializer.MyMonoSerializeToStream(writer, port.GetModule());
            } else {
                // insert blank module for good iteration!
				writer.WriteElementString(typeof(Module).Name, "EMPTY");
            }
            writer.WriteEndElement();
        }
    }
}
