using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
        {
            Events.Swipe_Call(eventData.delta.x);
        }
    }
}
