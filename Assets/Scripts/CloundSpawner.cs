using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CloundType
{
    CloudA, CloudB
}

public class CloundSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("���� ������")] GameObject[] cloundPrefab;
    [SerializeField, Tooltip("���� ������ Ǯ ������")] int poolSize = 5;
    [SerializeField, Tooltip("����A ������ Ǯ")] Queue<GameObject> cloudAQueue;
    [SerializeField, Tooltip("����B ������ Ǯ")] Queue<GameObject> cloudBQueue;
    [Tooltip("���� ���� Ÿ��")] public float returnTime = 10f;

    // ���� �̵� ������ �� ������ ��ġ
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
        // ������Ʈ Ǯ ����
        cloudAQueue = new Queue<GameObject>();
        cloudBQueue = new Queue<GameObject>();

        // poolSize��ŭ ������Ʈ ����
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

        // ���� �̵����� �ݶ��̴� ȹ��
        BoxCollider2D bound = GetComponent<BoxCollider2D>();

        // �̵� ���� �ݶ��̴� ������ ����
        leftTop = new Vector2(bound.bounds.min.x, bound.bounds.max.y);
        leftBottom = bound.bounds.min;
        rightTop = bound.bounds.max;
        rightBottom = new Vector2(bound.bounds.max.x, bound.bounds.min.y);

        // ���� ��ȯ �ڷ�ƾ ����
        StartCoroutine(SpawnClound());
    }

    private IEnumerator SpawnClound()
    {
        // ���� �ð��� ����
        yield return new WaitForSeconds(Random.Range(0f, 0.25f));

        while (true)
        {
            // CloundType�� ���� ������Ʈ ���� �� Ǯ���� ����
            GetQueue((CloundType)Random.Range(0, 2));
            // ���� �ð� ������ �ٽ� ����
            yield return new WaitForSeconds(Random.Range(0f, returnTime));
        }
    }

    public void InsertQueue(GameObject obj, CloundType type)
    {
        switch (type)
        {
            // type�� cloudA���
            case CloundType.CloudA:
                // ������ƮǮ�� ������Ʈ ��ȯ
                cloudAQueue.Enqueue(obj);
                break;
                // type�� cloudB���
            case CloundType.CloudB:
                // ������ƮǮ�� ������Ʈ ��ȯ
                cloudBQueue.Enqueue(obj);
                break;  
        }

        // ������Ʈ ��Ȱ��ȭ
        obj.SetActive(false);
        // ������Ʈ �ӵ� 0���� �ʱ�ȭ
        obj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void GetQueue(CloundType type)
    {
        // ���� ������Ʈ �ӽ� ���� ����
        GameObject obj = null;

        // �ź��� type�� ���� ���� ����
        switch(type)
        {
            // type�� CloudA���
            case CloundType.CloudA:
                // cloudAQueue���� ������Ʈ ����
                obj = cloudAQueue.Dequeue();
                break;
            // tpye�� CloudB���
            case CloundType.CloudB:
                // cloudBQueue���� ������Ʈ ����
                obj = cloudBQueue.Dequeue();
                break;
            // �� ���� type�� �Ѿ����
            default:
                // ���� �޽��� ���
                throw new System.Exception("�Ű����� �� �̻�");
        }

        // ������ ������Ʈ Ȱ��ȭ
        obj.SetActive(true);

        // ��� ��ȯ���� ���� ���� ����
        int value = Random.Range(0, 2);

        if (value == 0)
        {
            // ������Ʈ�� ��ġ�� ������ �� �������� �̵�
            obj.transform.position = new Vector2(Random.Range(leftTop.x, leftBottom.x), Random.Range(leftTop.y, leftBottom.y));
            // ������Ʈ�� Clound ��ũ��Ʈ�� SetUp�޼��� ����
            obj.GetComponent<Clound>().SetUp(true);
        }
        else
        {
            // ������Ʈ�� ��ġ�� ������ �� �������� �̵�
            obj.transform.position = new Vector2(Random.Range(rightTop.x, rightBottom.x), Random.Range(rightTop.y, rightBottom.y));
            // ������Ʈ�� Clound ��ũ��Ʈ�� SetUp�޼��� ����
            obj.GetComponent<Clound>().SetUp(false);
        }
    }

}
