using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEditor : MonoBehaviour {

    public static ShipEditor instance;
    public List<GameObject> ActiveItems;
	public List<GameObject> DanglingItems;
    public PartPicker PartPicker;

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

    public void OnCapsuleClicked(GameObject capsuleItem) {
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

    public void Update()
    {
        foreach (GameObject item in ActiveItems) {
            item.transform.position = new Vector3(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 
                item.transform.position.z);

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
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, ~((1 << 12) | ( 1 << 8)));

        if (hits.Length >= 1) {
            RaycastHit2D hit = hits[0];

            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.gameObject.CompareTag("Port")) {
                return;
            }

			Module m = hit.collider.GetComponent<Module> ();
			Port p = m.root;

			if (p && p.root.gameObject == ShipPicker.instance.GetCurrentShip()) {
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
		Ship ship = ShipPicker.instance.GetCurrentShip ().GetComponent<Ship> ();
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