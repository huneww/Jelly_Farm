using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour
{
    private static JellySpawner instance;

    public static JellySpawner Instance
    {
        get
        {
            GameObject obj = GameObject.Find("JellySpawner");
            if (obj == null)
            {
                obj = new GameObject("JellySpawner");
                instance = obj.AddComponent<JellySpawner>();
            }
            else
            {
                instance = obj.GetComponent<JellySpawner>();
            }

            return instance;
        }
    }

    [SerializeField, Tooltip("���� ������")]
    private GameObject[] jellyPrefabs;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="jellyIndex">������ ���� �ε��� ��ȣ</param>
    public void JellySpawn(int jellyIndex)
    {
        // ���� ������Ʈ ȹ��
        GameObject jelly = Instantiate(jellyPrefabs[jellyIndex]);
        // ������ ���� ��������Ʈ�� ����
        jelly.GetComponent<SpriteRenderer>().sprite = UIManager.Instance.jellySpriteList[jellyIndex];
        // ������ ������Ʈ ��ġ ���� ����
        jelly.transform.RandomPosition(
            GameManager.Instance.jellyBoundBox.bounds.min,
            GameManager.Instance.jellyBoundBox.bounds.max,
            true);
        jelly.transform.parent = this.transform;
        jelly.GetComponent<Jelly>().level = 1;
        jelly.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.jellyAnimator[0];
    }

    /// <summary>
    /// ��ȯ ����
    /// </summary>
    /// <param name="jelly">��ȯ�� ����</param>
    public void JellyDiSpawn(GameObject jelly)
    {
        Destroy(jelly);
    }

}
