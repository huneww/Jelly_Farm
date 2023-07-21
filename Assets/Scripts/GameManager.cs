using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            var obj = GameObject.Find("GameManager");

            if (obj == null)
            {
                obj = new GameObject("GameManager");
                instance = obj.AddComponent<GameManager>();
            }
            else
            {
                instance = obj.GetComponent<GameManager>();
            }

            return instance;
        }
    }

    [Tooltip("���� �̵� ����")]
    public BoxCollider2D jellyBoundBox;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // ���콺 ��Ŭ����
        if (Input.GetMouseButtonDown(0))
        {
            // ����ĳ��Ʈ ����
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            // hit�� ���� ������Ʈ�� �±װ� Jelly���
            if (hit.transform.tag == "Jelly")
            {
                // Jelly ��ũ��Ʈ�� �ִ� Touch�޼��� ȣ��
                hit.transform.GetComponent<Jelly>().Touch();
            }
        }
    }
}
