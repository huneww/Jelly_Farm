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

    [Header("재화 관련")]
    [SerializeField, Tooltip("현재 젤리 재화")]
    private int jellyMoney = 0;
    [SerializeField, Tooltip("현재 골드 재화")]
    private int goldMoney = 0;
    // 젤리 재화 프로퍼티
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
    // 골드 재화 프로퍼티
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
    [SerializeField, Tooltip("젤리 재화 텍스트")]
    private Text jellyText;
    [SerializeField, Tooltip("골드 재화 텍스트")]
    private Text goldText;

    [Space(20), Header("젤리 관련")]
    [SerializeField, Tooltip("젤리 보유 가능 수")]
    private int jellyMaxVolume = 2;
    // 젤리 최대 보유량 프로퍼티
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

    [SerializeField, Tooltip("현재 제리 보유 수")]
    private int curJellyVolume = 0;
    // 젤리 현재 보유량 프로퍼티
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

    [SerializeField, Tooltip("젤리 클릭시 젤리 획득량 배수")]
    private int clickCount;
    // 젤리 클릭시 젤리 획득량 배수
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

    [Tooltip("젤리 이동 영역")]
    public BoxCollider2D jellyBoundBox;

    [SerializeField, Tooltip("재화가 증가하는데 걸리는 시간")]
    private float moneyIncreaseTime = 1f;

    [Tooltip("젤리 애니메이터 컨트롤러")]
    public RuntimeAnimatorController[] jellyAnimator;
   
    [SerializeField, Tooltip("젤리를 팔수 있는 상태인지 확인 변수")]
    private bool isSell;
    // 젤리를 팔수 있는 상태 확인 변수의 프로퍼티
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

    [SerializeField, Tooltip("드래그 하고있는 젤리")]
    private GameObject selectJelly;
    // 선택한 젤리 프로퍼티
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

        // 저장되있던 데이터 저장
        jellyMoney = DateLoad.GetIntDate(nameof(jellyMoney));
        goldMoney = DateLoad.GetIntDate(nameof(goldMoney));
        jellyMaxVolume = DateLoad.GetIntDate("jellySizeLevel") * 2;
        clickCount = DateLoad.GetIntDate("clickLevel");
        
        // 보유 제화 텍스트 변경
        jellyText.text = string.Format("{0:#,###0}", jellyMoney);
        goldText.text = string.Format("{0:#,###0}", goldMoney);

        // 젤리 스폰
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
    /// 젤리 재화 증가 코루틴
    /// </summary>
    /// <param name="beforJelly">증가하기 이전의 재화</param>
    /// <param name="curJelly">현재 재화</param>
    /// <returns></returns>
    public IEnumerator UpDownJellyText(int beforJelly, int curJelly)
    {
        // 경과 시간 저장 변수
        float curTime = 0f;
        // 현재 진행도 저장 변수
        float percent = 0f;
        // 현재 까지 증가한 재화 저장 변수
        int jelly = 0;

        while (percent <= 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / moneyIncreaseTime;

            // 이전 재화와 현재 재화의 percent만큼의 선형보간으로 저장
            jelly = (int)Mathf.Lerp(beforJelly, curJelly, percent);

            // UI 텍스트를 저장된 재화로 변경
            jellyText.text = string.Format("{0:#,###0}", jelly);

            yield return null;
        }

    }

    /// <summary>
    /// 골드 재화 증가 코루틴
    /// </summary>
    /// <param name="beforGold">증가하기 이전의 재화</param>
    /// <param name="curGold">현재 재화</param>
    /// <returns></returns>
    public IEnumerator UpDownGoldText(int beforGold, int curGold, GameObject jelly = null)
    {
        // 경과 시간 저장 변수
        float curTime = 0f;
        // 현재 진행 상황 저장 변수
        float percent = 0f;
        // 현재 까지 증가한 재화 저장 변수
        int gold = 0;

        if (jelly != null)
        {
            // 젤리 알파값 0으로 조정
            Color color = jelly.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            jelly.GetComponent<SpriteRenderer>().color = color;
        }

        while (percent <= 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / moneyIncreaseTime;

            // 이전 재화와 현재 재화의 percent만큼의 선형보간으로 저장
            gold = (int)Mathf.Lerp(beforGold, curGold, percent);
            // UI 텍스트를 저장된 재화로 변경
            goldText.text = string.Format("{0:#,###0}", gold);

            yield return null;
        }


        if (jelly != null)
        {
            // 돈이 전부 올라가면 그때 젤리 오브젝트 풀에 반환
            JellySpawner.Instance.JellyDiSpawn(jelly);
        }
    }

    /// <summary>
    /// 현재 젤리 수용량 확인 메서드
    /// </summary>
    /// <returns></returns>
    public bool JellyVolumeCheck()
    {
        return curJellyVolume < jellyMaxVolume ? true : false;
    }

}
