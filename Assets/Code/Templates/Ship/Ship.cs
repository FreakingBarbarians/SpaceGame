using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Set this script to execute before port
// I'm probably abusing partial here and not using S.R.P properly :(
// oh well.

[Serializable]
public partial class Ship : Damageable {

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

    private Animator annie;

    public List<Port> ports = new List<Port>();
    public List<Port> mainPorts = new List<Port>();
	public List<Weapon> weapons = new List<Weapon>();

    public void Start()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Ship");
		annie = GetComponent<Animator> ();
        // register all of our ports
        foreach (Port p in ports) {
            p.Register(this);
        }

        foreach (Port p in mainPorts) {
            p.Register(this);
        }
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

	private void tick(){
		EnergyCur = Mathf.Min (EnergyMax, EnergyCur + EnergyRegen);
	}

    public void Thrust(Vector2 thrustDir)
    {
        DeltaPosition += thrustDir.normalized * Time.deltaTime * DeltaPositionFactor;
        if (DeltaPosition.magnitude > DeltaPositionMax)
        {
            DeltaPosition = DeltaPosition.normalized * DeltaPositionMax;
        }
    }

    public void Rotate(float dir)
    {
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
        DeltaRotation = 0;
        transform.up = Vector3.RotateTowards(transform.up, target, DeltaRotationMax * 3.141f / 180f * Time.deltaTime, 0.0f);
    }

    public void Brake() {
        if (DeltaPosition.magnitude <= 0.2) {
            DeltaPosition *= 0;
        }
        if (DeltaRotation <= 2) {
            DeltaRotation = 0;
        }
        DeltaPosition *= (1 - Time.deltaTime);
        DeltaRotation *= (1 - Time.deltaTime);
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

		// some advanced code
		/*
		 * Goal: Each module will be set "drifting" and dissappear when leaving the camera bounds
		 * Modules can "Survive" the explosion where they can be picked up by the ship via contact.
		 * And used in the ship later on
		 */

	}

    public new static GameObject ReadXml(XmlReader reader, Component workingCO) {
        Ship ship = (Ship)workingCO;

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
                writer.WriteElementString("MODULE", "EMPTY");
            }
            writer.WriteEndElement();
        }
        foreach (Port port in ports) {
            writer.WriteStartElement("PORT");
            if (port.IsConnected()) {
                SpaceSerializerDeserializer.MyMonoSerializeToStream(writer, port.GetModule());
            } else {
                // insert blank module for good iteration!
                writer.WriteElementString("MODULE", "EMPTY");
            }
            writer.WriteEndElement();
        }
    }
}
