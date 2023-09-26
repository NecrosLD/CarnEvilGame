using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string ItemName;

    public void OnMouseDown()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;

        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        transform.localPosition = new Vector2(0, 0);

        //throw new System.NotImplementedException();
    }
}
