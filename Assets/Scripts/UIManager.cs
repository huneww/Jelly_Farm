using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI �ǳ�")]
    [SerializeField]
    private Animator jellyAnimator;
    private bool jellyPanelIsShow = false;
    [SerializeField]
    private Animator plateAnimator;
    private bool platePanelIsShow = false;
    [SerializeField]
    private GameObject optionObject;
    private bool optionPanelIsShow = false;

    [Space(20), Header("���� ����")]

    [SerializeField, Tooltip("���� ���ĸ���Ʈ")]
    private Sprite[] jellySpriteList;
    [SerializeField, Tooltip("���� �̸�")]
    private string[] jellyNameList;
    [SerializeField, Tooltip("���� ��� ���")]
    private int[] jellyUnlockPayList;
    [SerializeField, Tooltip("���� ���� ���")]
    private int[] jellyBuyPayList;

    [Space(10), Header("����� ���� ���� ǥ�� UI")]
    [SerializeField, Tooltip("���� �̹���")]
    private Image jellyImage;
    [SerializeField, Tooltip("���� �̸�")]
    private Text jellyName;
    [SerializeField, Tooltip("���� ���� ��� �ؽ�Ʈ")]
    private Text jellyPay;
    [SerializeField, Tooltip("���� ���� �ε���")]
    private Text curJellyIndex;

    [Space(10), Header("���� ���� ���� ǥ�� UI")]
    [SerializeField, Tooltip("�� UI �׷�")]
    private GameObject lockGroup;
    [SerializeField, Tooltip("���� ���� �̹���")]
    private Image lockJellyImage;
    [SerializeField, Tooltip("�� ���� ���")]
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
