using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Date;

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

    [Header("��ȭ ����")]
    [SerializeField, Tooltip("���� ���� ��ȭ")]
    private int jellyMoney = 0;
    [SerializeField, Tooltip("���� ��� ��ȭ")]
    private int goldMoney = 0;
    // ���� ��ȭ ������Ƽ
    public int JellyMoney
    {
        get
        {
            return jellyMoney;
        }
        set
        {
            jellyMoney = value;
        }
    }
    // ��� ��ȭ ������Ƽ
    public int GoldMoney
    {
        get
        {
            return goldMoney;
        }
        set
        {
            goldMoney = value;
        }
    }
    [SerializeField, Tooltip("���� ��ȭ �ؽ�Ʈ")]
    private Text jellyText;
    [SerializeField, Tooltip("��� ��ȭ �ؽ�Ʈ")]
    private Text goldText;

    [Space(20), Header("���� ����")]
    [SerializeField, Tooltip("���� ���� ���� ��")]
    private int jellyMaxVolume = 2;
    // ���� �ִ� ������ ������Ƽ
    public int JellyMaxVolume
    {
        get
        {
            return jellyMaxVolume;
        }
        set
        {
            jellyMaxVolume = value;
        }
    }

    [SerializeField, Tooltip("���� ���� ���� ��")]
    private int curJellyVolume = 0;
    // ���� ���� ������ ������Ƽ
    public int CurJellyVolume
    {
        get
        {
            return curJellyVolume;
        }
        set
        {
            curJellyVolume = value;
        }
    }

    [SerializeField, Tooltip("���� Ŭ���� ���� ȹ�淮 ���")]
    private int clickCount;
    // ���� Ŭ���� ���� ȹ�淮 ���
    public int ClickCount
    {
        get
        {
            return clickCount;
        }
        set
        {
            clickCount = value;
        }
    }

    [Tooltip("���� �̵� ����")]
    public BoxCollider2D jellyBoundBox;

    [SerializeField, Tooltip("��ȭ�� �����ϴµ� �ɸ��� �ð�")]
    private float moneyIncreaseTime = 1f;

    [Tooltip("���� �ִϸ����� ��Ʈ�ѷ�")]
    public RuntimeAnimatorController[] jellyAnimator;
   
    [SerializeField, Tooltip("������ �ȼ� �ִ� �������� Ȯ�� ����")]
    private bool isSell;
    // ������ �ȼ� �ִ� ���� Ȯ�� ������ ������Ƽ
    public bool IsSell
    {
        get
        {
            return isSell;
        }
        set
        {
            isSell = value;
        }
    }

    [SerializeField, Tooltip("�巡�� �ϰ��ִ� ����")]
    private GameObject selectJelly;
    // ������ ���� ������Ƽ
    public GameObject SelectJelly
    {
        get
        {
            return selectJelly;
        }
        set
        {
            selectJelly = value;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // ������ִ� ������ ����
        jellyMoney = DateLoad.GetIntDate(nameof(jellyMoney));
        goldMoney = DateLoad.GetIntDate(nameof(goldMoney));
        jellyMaxVolume = DateLoad.GetIntDate("jellySizeLevel") * 2;
        clickCount = DateLoad.GetIntDate("clickLevel");
        
        // ���� ��ȭ �ؽ�Ʈ ����
        jellyText.text = string.Format("{0:#,###0}", jellyMoney);
        goldText.text = string.Format("{0:#,###0}", goldMoney);

        // ���� ����
        for (int i = 1; i <= jellyMaxVolume; i++)
        {
            int jellyLevel;
            int jellyTouchCount;
            int jellyIndex;

            DateLoad.GetJellyDate("Jelly" + (i * 2), out jellyLevel, out jellyTouchCount, out jellyIndex);

            if (jellyLevel != -1)
            {
                JellySpawner.Instance.JellySpawn(jellyIndex, i * 2, jellyLevel, jellyTouchCount);
            }
        }
    }

    /// <summary>
    /// ���� ��ȭ ���� �ڷ�ƾ
    /// </summary>
    /// <param name="beforJelly">�����ϱ� ������ ��ȭ</param>
    /// <param name="curJelly">���� ��ȭ</param>
    /// <returns></returns>
    public IEnumerator UpDownJellyText(int beforJelly, int curJelly)
    {
        // ��� �ð� ���� ����
        float curTime = 0f;
        // ���� ���൵ ���� ����
        float percent = 0f;
        // ���� ���� ������ ��ȭ ���� ����
        int jelly = 0;

        while (percent <= 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / moneyIncreaseTime;

            // ���� ��ȭ�� ���� ��ȭ�� percent��ŭ�� ������������ ����
            jelly = (int)Mathf.Lerp(beforJelly, curJelly, percent);

            // UI �ؽ�Ʈ�� ����� ��ȭ�� ����
            jellyText.text = string.Format("{0:#,###0}", jelly);

            yield return null;
        }

    }

    /// <summary>
    /// ��� ��ȭ ���� �ڷ�ƾ
    /// </summary>
    /// <param name="beforGold">�����ϱ� ������ ��ȭ</param>
    /// <param name="curGold">���� ��ȭ</param>
    /// <returns></returns>
    public IEnumerator UpDownGoldText(int beforGold, int curGold, GameObject jelly = null)
    {
        // ��� �ð� ���� ����
        float curTime = 0f;
        // ���� ���� ��Ȳ ���� ����
        float percent = 0f;
        // ���� ���� ������ ��ȭ ���� ����
        int gold = 0;

        if (jelly != null)
        {
            // ���� ���İ� 0���� ����
            Color color = jelly.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            jelly.GetComponent<SpriteRenderer>().color = color;
        }

        while (percent <= 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / moneyIncreaseTime;

            // ���� ��ȭ�� ���� ��ȭ�� percent��ŭ�� ������������ ����
            gold = (int)Mathf.Lerp(beforGold, curGold, percent);
            // UI �ؽ�Ʈ�� ����� ��ȭ�� ����
            goldText.text = string.Format("{0:#,###0}", gold);

            yield return null;
        }


        if (jelly != null)
        {
            // ���� ���� �ö󰡸� �׶� ���� ������Ʈ Ǯ�� ��ȯ
            JellySpawner.Instance.JellyDiSpawn(jelly);
        }
    }

    /// <summary>
    /// ���� ���� ���뷮 Ȯ�� �޼���
    /// </summary>
    /// <returns></returns>
    public bool JellyVolumeCheck()
    {
        return curJellyVolume < jellyMaxVolume ? true : false;
    }

}
