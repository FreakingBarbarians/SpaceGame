using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capsule : MonoBehaviour {

    public GameObject Item = null;
    public Image ItemImage;

	public virtual void SetItem(GameObject item) {
        if (!item) {
            Debug.LogWarning("Set Item: Null Item in container");
        }

        SpriteRenderer rendy;
        if ((rendy = item.GetComponent<SpriteRenderer>())) {
            if (rendy.sprite) {
                ItemImage.sprite = rendy.sprite;
            }
        }

        Item = item;

    }

    public GameObject GetItem() {
        return Item;
    }

    public virtual void OnClick() {

    }

    public virtual void OnCapsuleMouseOver() {
        Debug.Log("ON");
        PartInfoController.instance.Set(Item);
    }

    public virtual void OnCapsuleMouseOff() {
        Debug.Log("OFF");
        PartInfoController.instance.Set(null);
    }
}
