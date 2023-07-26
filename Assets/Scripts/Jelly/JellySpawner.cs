using Date;
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
    public void JellySpawn(int jellyIndex = 0, int bitvalue = 0, int jellyLevel = 1, int jellyTouchCount = 0)
    {
        // ���� �������� ���� ���� ����
        GameManager.Instance.CurJellyVolume++;
        // ���� ������Ʈ ȹ��
        GameObject jelly = Instantiate(jellyPrefabs[jellyIndex]);
        // ������ ���� ��������Ʈ�� ����
        jelly.GetComponent<SpriteRenderer>().sprite = UIManager.Instance.jellySpriteList[jellyIndex];
        // ������ ������Ʈ ��ġ ���� ����
        jelly.transform.RandomPosition(
            GameManager.Instance.jellyBoundBox.bounds.min,
            GameManager.Instance.jellyBoundBox.bounds.max,
            true);

        // ���� ���� �ʱ�ȭ
        jelly.transform.parent = this.transform;
        jelly.GetComponent<Jelly>().level = jellyLevel;
        jelly.GetComponent<Jelly>().index = jellyIndex;
        jelly.GetComponent<Jelly>().bitValue = bitvalue == 0 ? DateLoad.GetJellyDateEmptyBit() : bitvalue;
        jelly.GetComponent<JellyTouch>().TouchCount = jellyTouchCount;
        jelly.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.jellyAnimator[jellyLevel - 1];
    }

    /// <summary>
    /// ���� �Ǹ� �޼���
    /// </summary>
    /// <param name="jelly">��ȯ�� ����</param>
    public void JellyDiSpawn(GameObject jelly)
    {
        // ����� ������ ����
        PlayerPrefs.DeleteKey("Jelly" + jelly.GetComponent<Jelly>().bitValue);
        // ���� ������ ��Ʈ�� ȹ��
        int bitValue = DateLoad.GetIntDate("jellyDateBit");
        // ������ ������ ��Ʈ���� XOR�����ڷ� ���
        bitValue = bitValue ^ jelly.GetComponent<Jelly>().bitValue;
        // ����� ��Ʈ�� ����
        DateSave.SetDate("jellyDateBit", bitValue);
        // ������Ʈ ����
        Destroy(jelly);
    }

}
