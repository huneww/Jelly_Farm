using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 마우스가 판매 UI 위에 온다면
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 게임 매니저의 선택한 젤리가 있다면
        if (GameManager.Instance.SelectJelly != null)
            // IsSell 값 true로 변경
            GameManager.Instance.IsSell = true;
    }

    // 마우스가 판매 UI 위에서 나가면
    public void OnPointerExit(PointerEventData eventData)
    {
        // IsSell 값 false로 변경
        GameManager.Instance.IsSell = false;
    }
}
