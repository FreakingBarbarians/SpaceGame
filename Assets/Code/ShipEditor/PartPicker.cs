using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartPicker : MonoBehaviour {

	public List<GameObject> Source;

    public GameObject ElementPrefab;

    public int xPadding;
	public int yPadding;

	public int xEndPaddding;
	public int yEndPadding;

    public int ElementsPerRow;

    public int ElementsPerPage;

    private int ElementWidth;
    private int RowCount;
    public GameObject ItemArea;

    public void Start()
    {
        RectTransform rectTran = (RectTransform)ItemArea.transform;
        int width = (int)rectTran.rect.width;
        int height = (int)rectTran.rect.height;
        ElementWidth = (width - (ElementsPerRow - 1) * xPadding - xEndPaddding * 2) / ElementsPerRow;
        RowCount = height - yEndPadding;
        RowCount = RowCount / (ElementWidth + yPadding);
        ElementsPerPage = RowCount * ElementsPerRow;
        Populate(Source);
    }

    private void Populate(List<GameObject> items, int page = 0) {
        int xcount = 0;
        int ycount = 0;
        foreach (GameObject item in items) {
            Capsule capsule = Instantiate(ElementPrefab).GetComponent<Capsule>();
            capsule.transform.SetParent(ItemArea.transform);
            capsule.SetItem(item);
            capsule.transform.localScale = new Vector3(((float)ElementWidth /100f), (float)((float)ElementWidth / 100f), 1f);
            capsule.transform.localPosition = new Vector3(0, 0, 0);
            ((RectTransform)capsule.transform).anchoredPosition = new Vector3(
                xEndPaddding + xcount * (xPadding + ElementWidth), 
                -(yEndPadding + ycount * (yPadding + ElementWidth)));
            xcount++;
            if (xcount == ElementWidth) {
                xcount = 0;
                ycount++;
            }
        }
    }
}
