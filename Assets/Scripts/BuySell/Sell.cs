using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ���콺�� �Ǹ� UI ���� �´ٸ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���� �Ŵ����� ������ ������ �ִٸ�
        if (GameManager.Instance.SelectJelly != null)
            // IsSell �� true�� ����
            GameManager.Instance.IsSell = true;
    }

    // ���콺�� �Ǹ� UI ������ ������
    public void OnPointerExit(PointerEventData eventData)
    {
        // IsSell �� false�� ����
        GameManager.Instance.IsSell = false;
    }
}
