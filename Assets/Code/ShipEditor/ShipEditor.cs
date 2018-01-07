using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QventSystem;

public class ShipEditor : MonoBehaviour {

	public enum ItemFilter
	{
		MODULE,
		SHIP
	}

	public GameObject ModuleCapsule;
	public GameObject ShipCapsule;

    public static ShipEditor instance;
    public List<GameObject> ActiveItems;
	public List<GameObject> DanglingItems;
    public PartPicker PartPicker;
	public GameObject BuildPaneCenter;

	public List<GameObject> KnownShips;

	public ItemFilter FilterState = ItemFilter.MODULE;
    private bool isEnabled = false;


	// Camera space.
	private Vector2 offset;
    public void Start()
    {
        ActiveItems = new List<GameObject>();
		DanglingItems = new List<GameObject> ();
        if (instance) {
            Debug.LogWarning("More than one ShipEditor Instance");
            return;
        }
        instance = this;
    }

	public void Enable() {
        if (isEnabled) {
            return;
        }

		gameObject.SetActive (true);
		offset = PlayerData.instance.PlayerShip.transform.position - BuildPaneCenter.transform.position;
		Debug.DrawLine (PlayerData.instance.PlayerShip.transform.position, BuildPaneCenter.transform.position);
		CameraManager.instance.AddOffset (offset);
		switch (FilterState) {
		case ItemFilter.MODULE:
			LoadModules ();
			break;
		case ItemFilter.SHIP:
			LoadShips ();
			break;
		}

		PartPicker.Start ();
        Time.timeScale = 0;
        isEnabled = true;
    }

	public void Disable() {
        if (!isEnabled)
        {
            return;
        }

        gameObject.SetActive (false);
		CameraManager.instance.AddOffset (-offset);
		ClearDangling ();
        Time.timeScale = 1;
        isEnabled = false;
    }

	public void LoadShips() {
		PartPicker.Source = KnownShips;
		PartPicker.ElementsPerRow = 2;
		PartPicker.ElementPrefab = ShipCapsule;
		FilterState = ItemFilter.SHIP;
		PartPicker.Start ();
	}

	public void LoadModules() {
		PartPicker.Source = new List<GameObject> (PlayerData.instance.KnownModules.Values);
		PartPicker.ElementsPerRow = 3;
		PartPicker.ElementPrefab = ModuleCapsule;
		FilterState = ItemFilter.MODULE;
		PartPicker.Start ();
	}

    public void ModuleSelected(GameObject capsuleItem) {
        if (ActiveItems.Count >= 1)
        {
            // already has
        }
        else {
			Module m = capsuleItem.GetComponent<Module> ();
			if (PlayerData.instance.RemoveScrap (m.ScrapCost)) {
				ActiveItems.Add (GameObject.Instantiate (capsuleItem));
			}
        }
    }

	public void ShipSelected(GameObject capsuleItem){
		Ship s = capsuleItem.GetComponent<Ship> ();
		if (PlayerData.instance.RemoveScrap (s.ScrapCost)) {
			ClearAll ();
			Qvent q = new Qvent (QventType.DESTROYED, typeof(Ship), PlayerData.instance.PlayerShip);
			foreach (QventHandler handler in PlayerData.instance.PlayerShip.Listeners) {
				handler.HandleQvent (q);
			}
			Vector3 previousPlayerPos = PlayerData.instance.PlayerShip.gameObject.transform.position;
			PlayerData.instance.AddScrap (s.ScrapCost);
			GameObject.Destroy (PlayerData.instance.PlayerShip.gameObject);
			PlayerData.instance.PlayerShip = null;
			GameObject newShip = GalaxyManager.SpawnWorldObject (s.gameObject, previousPlayerPos);
			Ship ship =	newShip.GetComponent<Ship> ();
			ship.IsPlayer = true;
			ship.curhp = ship.maxhp / 5;
			PlayerData.instance.PlayerShip = ship;
			newShip.AddComponent<PlayerController> ();
			CameraManager.instance.SetToFollow (newShip);
			GalaxyManager.instance.AddObserver (newShip);
		}
	}

    public void Update()
    {
        foreach (GameObject item in ActiveItems) {
            item.transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
				transform.position.z);

        }
        if (Input.GetMouseButtonUp(0)) {
            OnRelease();
        }

        if (Input.GetMouseButtonDown(0)) {
            OnPress();
        }

		if (Input.GetKeyDown (KeyCode.C)) {
			ClearDangling ();
		}

		if(Input.GetKeyDown(KeyCode.X)){
			ClearShip ();
		}
    } 

    public virtual void OnRelease()
    {
        
        if (IsInPartPicker())
        {
            foreach (GameObject item in ActiveItems)
            {
				DanglingItems.Remove (item);
				Module m = item.GetComponent<Module> ();
				PlayerData.instance.AddScrap (m.ScrapCost);
                Destroy(item);
            }
            ActiveItems.Clear();
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            foreach (RaycastHit2D hit in hits) {
                if (ActiveItems.Count >= 1 && hit.collider.gameObject.CompareTag("Port")) {
                    Port port = hit.collider.gameObject.GetComponent<Port>();
					if (!port.Connect (ActiveItems [0].GetComponent<Module> ())) {
						DanglingItems.Add (ActiveItems [0]);
					} else {
						DanglingItems.Remove (ActiveItems [0]);
						ActiveItems.RemoveAt(0);
					}
                }
            }

			DanglingItems.AddRange(ActiveItems);
            ActiveItems.Clear();
        }
    }

    public virtual void OnPress() {
        if (IsInPartPicker()) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, ~((1 << 12) | ( 1 << 8) | (1 << 14)));

        if (hits.Length >= 1) {
            RaycastHit2D hit = hits[0];
            if (hit.collider.gameObject.CompareTag("Port")) {
                return;
            }

			Module m = hit.collider.GetComponent<Module> ();
			Port p = m.root;

			if (p && p.root.gameObject == PlayerData.instance.PlayerShip.gameObject) {
                hit.collider.gameObject.GetComponent<Module>().root.Disconnect();
				ActiveItems.Add(m.gameObject);
			} else if (DanglingItems.Contains(m.gameObject)) {
				ActiveItems.Add(m.gameObject);
			}
        }
    }

    private bool Contains(Vector3[] corners, Vector3 WorldMousePos) {
        if (WorldMousePos.x >= corners[0].x && WorldMousePos.y >= corners[0].y 
            && WorldMousePos.x <= corners[2].x && WorldMousePos.y <= corners[2].y) {
            return true;
        }
        return false;
    }

    private bool IsInPartPicker() {
        Vector3[] array = new Vector3[4];
        ((RectTransform)PartPicker.transform).GetWorldCorners(array);
        if (Contains(array, Camera.main.ScreenToWorldPoint(Input.mousePosition))) {
            return true;
        }
        return false;
    }

	public void ClearDangling() {
		foreach (GameObject obj in DanglingItems) {
			Module m = obj.GetComponent<Module> ();
			PlayerData.instance.AddScrap (m.ScrapCost);
			Destroy (obj);
		}
		DanglingItems.Clear ();
	}

	public void ClearAll() {
		ClearDangling ();
		ClearShip ();
	}

	public void ClearShip() {
		Ship ship = PlayerData.instance.PlayerShip;
		foreach (Port p in ship.ports) {
			if (p.IsConnected ()) {
				Module m = p.GetModule ();
				p.Disconnect ();
				PlayerData.instance.AddScrap (m.ScrapCost);
				Destroy (m.gameObject);
			}
		}
		foreach (Port p in ship.mainPorts) {
			if (p.IsConnected ()) {
				Module m = p.GetModule ();
				p.Disconnect ();
				PlayerData.instance.AddScrap (m.ScrapCost);
				Destroy (m.gameObject);
			}
		}
	}



}