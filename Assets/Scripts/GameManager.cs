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

    [Tooltip("젤리 이동 영역")]
    public BoxCollider2D jellyBoundBox;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // 마우스 좌클릭시
        if (Input.GetMouseButtonDown(0))
        {
            // 레이캐스트 생성
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            // hit에 맞은 오브젝트의 태그가 Jelly라면
            if (hit.transform.tag == "Jelly")
            {
                // Jelly 스크립트에 있는 Touch메서드 호출
                hit.transform.GetComponent<Jelly>().Touch();
            }
        }
    }
}
