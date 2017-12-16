using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEditor : MonoBehaviour {

    public static ShipEditor instance;
    public List<GameObject> ActiveItems;
    public PartPicker PartPicker;

    public void Start()
    {
        ActiveItems = new List<GameObject>();
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
            ActiveItems.Add(GameObject.Instantiate(capsuleItem));
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

    } 

    public virtual void OnRelease()
    {
        
        if (IsInPartPicker())
        {
            foreach (GameObject item in ActiveItems)
            {
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
                    port.Connect(ActiveItems[0].GetComponent<Module>());
                }
            }
            ActiveItems.Clear();
        }
    }

    public virtual void OnPress() {
        if (IsInPartPicker()) {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, ~(1 << 12));

        if (hits.Length >= 1) {
            RaycastHit2D hit = hits[0];

            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Port")) {
                return;
            }
            if (hit.collider.gameObject.GetComponent<Module>().root) {
                hit.collider.gameObject.GetComponent<Module>().root.Disconnect();
            }
            ActiveItems.Add(hit.collider.gameObject);
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
}