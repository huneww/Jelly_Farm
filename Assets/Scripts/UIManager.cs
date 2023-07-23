using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    private Sprite[] jellySpriteList;
    [SerializeField, Tooltip("젤리 이름")]
    private string[] jellyNameList;
    [SerializeField, Tooltip("젤리 언락 비용")]
    private int[] jellyUnlockPayList;
    [SerializeField, Tooltip("젤리 구매 비용")]
    private int[] jellyBuyPayList;

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

    public void ClickJellyButton()
    {
        if (!jellyPanelIsShow)
        {
            jellyAnimator.SetTrigger("doShow");
            jellyPanelIsShow = true;

            if (platePanelIsShow)
            {
                plateAnimator.SetTrigger("doHide");
                platePanelIsShow = false;
            }
        }
        else
        {
            jellyAnimator.SetTrigger("doHide");
            jellyPanelIsShow = false;
        }
    }

    public void ClickPlateButton()
    {
        if (!platePanelIsShow)
        {
            plateAnimator.SetTrigger("doShow");
            platePanelIsShow = true;

            if (jellyPanelIsShow)
            {
                jellyAnimator.SetTrigger("doHide");
                jellyPanelIsShow = false;
            }
        }
        else
        {
            plateAnimator.SetTrigger("doHide");
            platePanelIsShow = false;
        }
    }

    public void JellyShopRightButton()
    {
        curJellyView++;

        if (curJellyView >= jellySpriteList.Length)
        {
            curJellyView = jellySpriteList.Length - 1;
            return;
        }

        JellyShopInfoChange();
    }

    public void JellyShopLeftButton()
    {
        curJellyView--;

        if (curJellyView < 0)
        {
            curJellyView = 0;
            return;
        }

        JellyShopInfoChange();
    }

    private void JellyShopInfoChange()
    {
        lockGroup.SetActive(isLock[curJellyView]);

        if (!isLock[curJellyView])
        { 
            jellyImage.sprite = jellySpriteList[curJellyView];
            jellyName.text = jellyNameList[curJellyView];
            jellyPay.text = jellyBuyPayList[curJellyView].ToString();
        }
        else
        {
            lockJellyImage.sprite = jellySpriteList[curJellyView];
            unlockPay.text = jellyUnlockPayList[curJellyView].ToString();
        }

        if (curJellyView + 1 < 10)
            curJellyIndex.text = "#0" + (curJellyView + 1).ToString();
        else
            curJellyIndex.text = "#" + (curJellyView + 1).ToString();
    }

    public void BuyButtonClick()
    {
        if (isLock[curJellyView])
        {
            if (GameManager.Instance.JellyMoney >= jellyUnlockPayList[curJellyView])
            {
                GameManager.Instance.JellyMoney -= jellyUnlockPayList[curJellyView];
                isLock[curJellyView] = false;
                JellyShopInfoChange();
                Debug.Log("Buy Jelly");
            }
            else
            {
                Debug.Log("Not Enught Jelly");
            }
        }
        else
        {
            Debug.Log("Buy Gold");
        }
    }

    private void OptionPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanelIsShow = !optionPanelIsShow;
            optionObject.SetActive(optionPanelIsShow);
            Time.timeScale = optionPanelIsShow ? 0 : 1;
        }
    }

    private void Start()
    {
        isLock = new bool[12];
        isLock[0] = false;
        for (int i = 1; i < isLock.Length; i++)
            isLock[i] = true;
        JellyShopInfoChange();
    }

    private void Update()
    {
        OptionPanel();
    }

}
