using Date;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            GameObject obj = GameObject.Find("GameManager");
            if (obj == null)
            {
                obj = new GameObject("GameManager");
                instance = obj.AddComponent<UIManager>();
            }
            else
            {
                instance = obj.GetComponent<UIManager>();
            }

            return instance;
        }
    }

    [Header("UI 판넬")]
    [SerializeField]
    private Animator jellyAnimator;
    private bool jellyPanelIsShow = false;
    [SerializeField]
    private Animator plateAnimator;
    private bool platePanelIsShow = false;
    [SerializeField]
    private GameObject optionObject;
    private bool optionPanelIsShow = false;

    [Space(20), Header("젤리 정보")]

    [SerializeField, Tooltip("젤리 스파리이트")]
    public Sprite[] jellySpriteList;
    [SerializeField, Tooltip("젤리 이름")]
    private string[] jellyNameList;
    [SerializeField, Tooltip("젤리 언락 비용")]
    private int[] jellyUnlockPayList;
    [SerializeField, Tooltip("젤리 구매 비용")]
    private int[] jellyBuyPayList;

    [Space(20), Header("젤리 상점")]

    [Space(10), Header("언락된 젤리 정보 표시 UI")]
    [SerializeField, Tooltip("젤리 이미지")]
    private Image jellyImage;
    [SerializeField, Tooltip("젤리 이름")]
    private Text jellyName;
    [SerializeField, Tooltip("젤리 구매 비용 텍스트")]
    private Text jellyPay;
    [SerializeField, Tooltip("현재 젤리 인덱스")]
    private Text curJellyIndex;

    [Space(10), Header("락된 젤리 정보 표시 UI")]
    [SerializeField, Tooltip("락 UI 그룹")]
    private GameObject lockGroup;
    [SerializeField, Tooltip("락된 젤리 이미지")]
    private Image lockJellyImage;
    [SerializeField, Tooltip("락 해제 비용")]
    private Text unlockPay; 
    [Min(0)]
    private int curJellyView = 0;
    public bool[] isLock;

    [Space(20), Header("플레이어 능력 관련")]
    [SerializeField, Tooltip("아파트 업그레이드 비용")]
    private int[] jellySizeGoldList;
    [SerializeField, Tooltip("꾹꾹이 업그레이드 비용")]
    private int[] clickGoldList;
    [SerializeField, Tooltip("아파트 현재 레벨"), Min(1)]
    private int jellySizeLevel = 1;
    [SerializeField, Tooltip("꾹꾹이 현재 레벨"), Min(1)]
    private int clickLevel = 1;
    [SerializeField, Tooltip("젤리 보유량 서브 텍스트")]
    private Text jellySizeSubText;
    [SerializeField, Tooltip("젤리 보유량 비용 텍스트")]
    private Text jellySizePayText;
    [SerializeField, Tooltip("꾹꾹이 서브 텍스트")]
    private Text clickSubText;
    [SerializeField, Tooltip("꾹꾹이 비용 텍스트")]
    private Text clickPayText;


    // 젤리 상점 버튼 클릭
    public void ClickJellyButton()
    {
        // 젤리 상점 판넬이 보이지 않는다면
        if (!jellyPanelIsShow)
        {
            // 젤리 상점 나오는 애니메이션 실행
            jellyAnimator.SetTrigger("doShow");
            // jellyPanelIsShow 값 변경
            jellyPanelIsShow = true;

            // 만약 플레이트 판넬이 나와있다면
            if (platePanelIsShow)
            {
                // 플레이트 판넬 들어가는 애니메이션 실행
                plateAnimator.SetTrigger("doHide");
                // platePanelIsShow 값 변경
                platePanelIsShow = false;
            }
        }
        else
        {
            // 젤리 상점 들어가는 애니메이션 실행
            jellyAnimator.SetTrigger("doHide");
            // jellyPanelIsShow 값 변경
            jellyPanelIsShow = false;
        }

        // 버튼 클릭 효과음 재생
        AudioManager.PlaySFXAudioSource(SFX.Button);
    }

    // 플레이트 상점 버튼 클릭
    public void ClickPlateButton()
    {
        // 플레이트 판넬이 나와있지 않다면
        if (!platePanelIsShow)
        {
            // 플레이트 판넬 나오는 애니메이션 실행
            plateAnimator.SetTrigger("doShow");
            // platePanelIsShow 값 변경
            platePanelIsShow = true;

            // 젤리 상점이 나와있다면
            if (jellyPanelIsShow)
            {
                // 젤리 상점 들어가는 애니메이션 실행
                jellyAnimator.SetTrigger("doHide");
                // jellyPanelIsShow 값 변경
                jellyPanelIsShow = false;
            }
        }
        else
        {
            // 플레이트 판넬 들어가는 애니메이션 실행
            plateAnimator.SetTrigger("doHide");
            // platePanelIsShow 값 변경
            platePanelIsShow = false;
        }

        // 버튼 클릭 효과음 재생
        AudioManager.PlaySFXAudioSource(SFX.Button);
    }

    // 젤링 상점 오른쪽 활살표 클릭
    public void JellyShopRightButton()
    {
        // 보여지는 페이지 값 변경
        curJellyView++;

        // curJellyView가 젤리 스프라이트 갯수보다 같거나 커지면
        if (curJellyView >= jellySpriteList.Length)
        {
            // curJellyView 값 임의로 설정
            curJellyView = jellySpriteList.Length - 1;
            // 메서드 종료
            return;
        }

        // 버튼 클릭 효과음 재생
        AudioManager.PlaySFXAudioSource(SFX.Button);

        // 젤리 상점 정보 변경
        JellyShopInfoChange();
    }

    // 젤리 상점 왼쪽 화살표 클릭
    public void JellyShopLeftButton()
    {
        // 현재 보여지는 페이지 값 변경
        curJellyView--;

        // curJellyView 가 0보다 작아지면
        if (curJellyView < 0)
        {
            // curJellyView 값 임의로 설정
            curJellyView = 0;
            // 메서드 종료
            return;
        }

        // 버튼 클릭 효과음 재생
        AudioManager.PlaySFXAudioSource(SFX.Button);

        // 젤리 상점 정보 변경
        JellyShopInfoChange();
    }

    // 해금 버튼 클릭시
    public void UnLockButtonClick()
    {
        // 현제 젤리 보유량이 해금 비용보다 크거나 같다면
        if (GameManager.Instance.JellyMoney >= jellyUnlockPayList[curJellyView])
        {
            // 현제 젤리 보유량에서 해금 비용만큼 차감
            GameManager.Instance.JellyMoney -= jellyUnlockPayList[curJellyView];
            // 구매 젤리 해금
            isLock[curJellyView] = false;
            // 상점 정보 초기화
            JellyShopInfoChange();
            // 젤리 텍스트 감소
            StartCoroutine(GameManager.Instance.UpDownJellyText(
                           GameManager.Instance.JellyMoney + jellyUnlockPayList[curJellyView],
                           GameManager.Instance.JellyMoney));
            // 해금 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Unlock);

            // 현재 저장된 isLock비트값을 가져온다
            int bitvalue = DateLoad.GetIntDate(nameof(isLock));
            // 구매한 젤리의 인덱스 번호만큼 비트를 좌측으로 이동시킨후
            // OR 연산자를 이용해서 bitvalue에 저장한다.
            /*
                ex)
                bitvalue = 1;
                비트 표시 : 0000 0001
                curJellyView = 3;
                비트 표시 : 0000 0100

                OR 연산후 : 000 0101   
            */
            // 1에서 비트연산을 하게되면 의도한것보다 한칸더 이동하게 된다
            bitvalue = bitvalue | (0 << curJellyView);
            // 변경된 비트값을 다시 저장
            DateSave.SetDate(nameof(isLock), bitvalue);
        }
        else
        {
            // 구매 실패 사운드 실행
            AudioManager.PlaySFXAudioSource(SFX.Fail);
            Debug.Log("Not Enught Jelly");
        }
    }

    // 구매 버튼 클릭시
    public void BuyButtonClick()
    {
        // 현재 보유 골드량이 해금 비용보다 크거나 같다면
        if (GameManager.Instance.GoldMoney >= jellyBuyPayList[curJellyView] && GameManager.Instance.JellyVolumeCheck())
        {
            // 현재 보유 골드량에서 구매 비용만큼 차감
            GameManager.Instance.GoldMoney -= jellyBuyPayList[curJellyView];
            // 젤리 스폰
            JellySpawner.Instance.JellySpawn(curJellyView);
            // 골드 텍스트 감소
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + jellyBuyPayList[curJellyView],
                           GameManager.Instance.GoldMoney));
            // 현재 수용중인 젤리 수량 증가
            GameManager.Instance.CurJellyVolume++;
            // 구매 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Buy);
        }
        else
        {
            // 구매 실패 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Fail);
        }
    }

    // 젤리 상점 변경
    private void JellyShopInfoChange()
    {
        // 현재 페이지의 젤리가 잠겨있다면 잠겨져있는 UI 활성화
        lockGroup.SetActive(isLock[curJellyView]);

        // 현재 젤리가 해금이되어있다면
        if (!isLock[curJellyView])
        {
            // 젤리 이미지 변경
            jellyImage.sprite = jellySpriteList[curJellyView];
            // 젤리 이름 표시
            jellyName.text = jellyNameList[curJellyView];
            // 젤리 구매 비용 표시
            jellyPay.text = jellyBuyPayList[curJellyView].ToString();
        }
        else
        {
            // 젤리 이미지 변경
            lockJellyImage.sprite = jellySpriteList[curJellyView];
            // 젤리 해금 비용 표시
            unlockPay.text = jellyUnlockPayList[curJellyView].ToString();
        }

        // 현재 페이지가 10보다 작다면
        if (curJellyView + 1 < 10)
            // 젤리 인덱스 번호 앞에 #0을 붙인다.
            curJellyIndex.text = "#0" + (curJellyView + 1).ToString();
        else
            // 젤리 인덱스 번호 앞에 #을 붙인다.
            curJellyIndex.text = "#" + (curJellyView + 1).ToString();
    }

    // 젤리 수용량 증가 버튼
    public void JellySizeButtonClick()
    {
        // 현재 보유 골드가 업그레이드 비용보다 크거나 같으면
        if (GameManager.Instance.GoldMoney >= jellySizeGoldList[jellySizeLevel] && jellySizeLevel <= jellySizeGoldList.Length)
        {
            // 사이즈 레벨 증가
            jellySizeLevel++;
            // 현재 보유 골드량에서 비용 만큼 차감
            GameManager.Instance.GoldMoney -= jellySizeGoldList[jellySizeLevel - 1];
            // 골드 텍스트 감소
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + jellySizeGoldList[jellySizeLevel - 1],
                           GameManager.Instance.GoldMoney));
            // 젤리 최대 수용량 증가
            GameManager.Instance.JellyMaxVolume = jellySizeLevel * 2;
            // 구매 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Buy);
            // 데이터 저장
            DateSave.SetDate(nameof(jellySizeLevel), jellySizeLevel);
        }

        // 플레이트 상점 정보 갱신
        PlayerStateShopInfoChange();

    }

    // 클릭당 획득 수량
    public void ClickCountButtonClick()
    {
        // 현재 보유 골드가 업그레이드 비용보다 크거나 같으면
        if (GameManager.Instance.GoldMoney >= clickGoldList[clickLevel] && clickLevel <= clickGoldList.Length)
        {
            // 클릭 레벨 증가
            clickLevel++;
            // 현재 보유 골드량에서 비용 만큼 차감
            GameManager.Instance.GoldMoney -= jellySizeGoldList[jellySizeLevel - 1];
            // 골드 텍스트 감소
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + clickGoldList[clickLevel - 1],
                           GameManager.Instance.GoldMoney));
            // 클릭시 획득량 배수 변수 증가
            GameManager.Instance.ClickCount = clickLevel;
            // 구매 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Buy);
            // 데이터 저장
            DateSave.SetDate(nameof(clickLevel), clickLevel);
        }

        // 플레이트 상점 정보 갱신
        PlayerStateShopInfoChange();
    }

    // 플레이어 스텟 상점 정보
    private void PlayerStateShopInfoChange()
    {
        jellySizeSubText.text = "젤리 수용량 " + (jellySizeLevel * 2);
        jellySizePayText.text = jellySizeGoldList[jellySizeLevel].ToString();

        clickSubText.text = "클릭 생상량 X " + clickLevel;
        clickPayText.text = clickGoldList[clickLevel].ToString();
    }

    // 옵션 화면
    private void OptionPanel()
    {
        // ESC키를 누르면
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // optionPanelIsShow 값 반전
            optionPanelIsShow = !optionPanelIsShow;
            // 옵션 UI 활성화 또는 비활성화
            optionObject.SetActive(optionPanelIsShow);
            // optionPanelIsShow 값에 따라 timeScale 값 조절
            Time.timeScale = optionPanelIsShow ? 0 : 1;
            // 옵션 UI 활성화 비활성화 효과음 재생
            AudioManager.PlaySFXAudioSource(optionPanelIsShow ? SFX.PauseIn : SFX.PauseOut);
        }
    }

    // 돌아가기 버튼 클릭
    public void OptionBackGameButtonClick()
    {
        // optionPanelIsShow 값 반전
        optionPanelIsShow = !optionPanelIsShow;
        // 옵션 UI 활성화 또는 비활성화
        optionObject.SetActive(optionPanelIsShow);
        // optionPanelIsShow 값에 따라 timeScale 값 조절
        Time.timeScale = optionPanelIsShow ? 0 : 1;
        // 옵션 UI 활성화 비활성화 효과음 재생
        AudioManager.PlaySFXAudioSource(optionPanelIsShow ? SFX.PauseIn : SFX.PauseOut);
    }

    // 게임 종료 버튼 클릭
    public void OptionExitGameButtonClick()
    {
        Application.Quit();
    }

    private void Start()
    {
        // 현재 젤리 스프라이트 리스트 크기만큼 메모리 할당
        isLock = new bool[jellySpriteList.Length];

        // 저장되있던 데이터 저장
        DateLoad.GetIsLockDate(nameof(isLock), out isLock);

        // 상점 정보 초기화
        JellyShopInfoChange();
        // 플레이어 능력 정보 초기화
        PlayerStateShopInfoChange();
    }

    private void Update()
    {
        // 설정창 켜고 끄는 메서드
        OptionPanel();
    }

}
