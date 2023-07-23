using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.SelectJelly != null)
            GameManager.Instance.IsSell = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.IsSell = false;
    }

    private void OnMouseEnter()
    {
        if (GameManager.Instance.SelectJelly != null)
            GameManager.Instance.IsSell = true;
    }

    private void OnMouseExit()
    {
        GameManager.Instance.IsSell = false;
    }
}
