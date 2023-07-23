using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

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

    [Tooltip("젤리 이동 영역")]
    public BoxCollider2D jellyBoundBox;
    [SerializeField, Tooltip("마우스 클릭시 추출할 레이어")]
    private LayerMask jellyMask;
    [SerializeField, Tooltip("재화가 증가하는데 걸리는 시간")]
    private float moneyIncreaseTime = 1f;
    [Tooltip("젤리 애니메이터 컨트롤러")]
    public AnimatorController[] jellyAnimator;
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

        jellyText.text = "0";
        goldText.text = "0";
    }

    /// <summary>
    /// 젤리 재화 증가 코루틴
    /// </summary>
    /// <param name="beforJelly">증가하기 이전의 재화</param>
    /// <param name="curJelly">현재 재화</param>
    /// <returns></returns>
    public IEnumerator UpJellyText(int beforJelly, int curJelly)
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
    public IEnumerator UpGoldText(int beforGold, int curGold, GameObject jelly)
    {
        // 경과 시간 저장 변수
        float curTime = 0f;
        // 현재 진행 상황 저장 변수
        float percent = 0f;
        // 현재 까지 증가한 재화 저장 변수
        int gold = 0;

        // 젤리 알파값 0으로 조정
        Color color = jelly.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        jelly.GetComponent<SpriteRenderer>().color = color;

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

        // 돈이 전부 올라가면 그때 젤리 오브젝트 파괴
        Destroy(jelly);
    }
}
