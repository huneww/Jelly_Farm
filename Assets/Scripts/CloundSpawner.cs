using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CloundType
{
    CloudA, CloudB
}

public class CloundSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("구름 프리펩")] GameObject[] cloundPrefab;
    [SerializeField, Tooltip("구름 프리펩 풀 사이즈")] int poolSize = 5;
    [SerializeField, Tooltip("구름A 프리펩 풀")] Queue<GameObject> cloudAQueue;
    [SerializeField, Tooltip("구름B 프리펩 풀")] Queue<GameObject> cloudBQueue;
    [Tooltip("구름 제거 타임")] public float returnTime = 10f;

    // 구름 이동 영역의 각 꼭지점 위치
    private Vector2 leftTop;
    private Vector2 rightTop;
    private Vector2 leftBottom;
    private Vector2 rightBottom;

    private static CloundSpawner instance;

    public static CloundSpawner Instance
    {
        get
        {
            GameObject obj = GameObject.Find("CloundSpawner");
            if (obj == null)
            {
                obj = new GameObject("CloundSpawner");
                instance = obj.AddComponent<CloundSpawner>();
            }
            else
            {
                instance = obj.GetComponent<CloundSpawner>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        // 오브젝트 풀 생성
        cloudAQueue = new Queue<GameObject>();
        cloudBQueue = new Queue<GameObject>();

        // poolSize만큼 오브젝트 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cloneA = Instantiate(cloundPrefab[0]);
            cloneA.transform.parent = this.transform;
            cloudAQueue.Enqueue(cloneA);
            cloneA.SetActive(false);
            GameObject cloneB = Instantiate(cloundPrefab[1]);
            cloneB.transform.parent = this.transform;
            cloudBQueue.Enqueue(cloneB);
            cloneB.SetActive(false);
        }

        // 구름 이동영역 콜라이더 획득
        BoxCollider2D bound = GetComponent<BoxCollider2D>();

        // 이동 영역 콜라이더 꼭지점 저장
        leftTop = new Vector2(bound.bounds.min.x, bound.bounds.max.y);
        leftBottom = bound.bounds.min;
        rightTop = bound.bounds.max;
        rightBottom = new Vector2(bound.bounds.max.x, bound.bounds.min.y);

        // 구름 소환 코루틴 실행
        StartCoroutine(SpawnClound());
    }

    private IEnumerator SpawnClound()
    {
        // 일정 시간후 실행
        yield return new WaitForSeconds(Random.Range(0f, 0.25f));

        while (true)
        {
            // CloundType에 따라 오브젝트 생성 및 풀에서 제거
            GetQueue((CloundType)Random.Range(0, 2));
            // 일정 시간 정지후 다시 실행
            yield return new WaitForSeconds(Random.Range(0f, returnTime));
        }
    }

    public void InsertQueue(GameObject obj, CloundType type)
    {
        switch (type)
        {
            // type이 cloudA라면
            case CloundType.CloudA:
                // 오브젝트풀에 오브젝트 반환
                cloudAQueue.Enqueue(obj);
                break;
                // type이 cloudB라면
            case CloundType.CloudB:
                // 오브젝트풀에 오브젝트 반환
                cloudBQueue.Enqueue(obj);
                break;  
        }

        // 오브젝트 비활성화
        obj.SetActive(false);
        // 오브젝트 속도 0으로 초기화
        obj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void GetQueue(CloundType type)
    {
        // 생성 오브젝트 임시 저장 변수
        GameObject obj = null;

        // 매변수 type에 따라 구름 생성
        switch(type)
        {
            // type이 CloudA라면
            case CloundType.CloudA:
                // cloudAQueue에서 오브젝트 생성
                obj = cloudAQueue.Dequeue();
                break;
            // tpye이 CloudB라면
            case CloundType.CloudB:
                // cloudBQueue에서 오브젝트 생성
                obj = cloudBQueue.Dequeue();
                break;
            // 그 외의 type이 넘어오면
            default:
                // 오류 메시지 출력
                throw new System.Exception("매개변수 값 이상");
        }

        // 생성한 오브젝트 활성화
        obj.SetActive(true);

        // 어디에 소환할지 정할 랜덤 벨류
        int value = Random.Range(0, 2);

        if (value == 0)
        {
            // 오브젝트의 위치를 영역의 맨 좌측으로 이동
            obj.transform.position = new Vector2(Random.Range(leftTop.x, leftBottom.x), Random.Range(leftTop.y, leftBottom.y));
            // 오브젝트의 Clound 스크립트의 SetUp메서드 실행
            obj.GetComponent<Clound>().SetUp(true);
        }
        else
        {
            // 오브젝트의 위치를 영역의 맨 우측으로 이동
            obj.transform.position = new Vector2(Random.Range(rightTop.x, rightBottom.x), Random.Range(rightTop.y, rightBottom.y));
            // 오브젝트의 Clound 스크립트의 SetUp메서드 실행
            obj.GetComponent<Clound>().SetUp(false);
        }
    }

}
