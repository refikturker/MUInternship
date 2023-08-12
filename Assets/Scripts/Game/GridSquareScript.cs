using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSquareScript : MonoBehaviour, IDropHandler
{
    public Image normalImage;
    public List<Sprite> normalImages;


    public void SetImage()
    {
        normalImage.sprite = normalImages[0];
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
           eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        int index = transform.GetSiblingIndex();
        GridScript gridScript = transform.parent.GetComponent<GridScript>();
        gridScript.pieces[index] = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int index = transform.GetSiblingIndex();
        GridScript gridScript = transform.parent.GetComponent<GridScript>();
        gridScript.pieces[index] = false;
    }
}