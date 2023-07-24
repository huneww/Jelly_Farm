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
    public Sprite[] jellySpriteList;
    [SerializeField, Tooltip("���� �̸�")]
    private string[] jellyNameList;
    [SerializeField, Tooltip("���� ��� ���")]
    private int[] jellyUnlockPayList;
    [SerializeField, Tooltip("���� ���� ���")]
    private int[] jellyBuyPayList;

    [Space(20), Header("���� ����")]

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

    [Space(20), Header("�÷��̾� �ɷ� ����")]
    [SerializeField, Tooltip("����Ʈ ���׷��̵� ���")]
    private int[] jellySizeGoldList;
    [SerializeField, Tooltip("�ڲ��� ���׷��̵� ���")]
    private int[] clickGoldList;
    [SerializeField, Tooltip("����Ʈ ���� ����"), Min(1)]
    private int jellySizeLevel = 1;
    [SerializeField, Tooltip("�ڲ��� ���� ����"), Min(1)]
    private int clickLevel = 1;
    [SerializeField, Tooltip("���� ������ ���� �ؽ�Ʈ")]
    private Text jellySizeSubText;
    [SerializeField, Tooltip("���� ������ ��� �ؽ�Ʈ")]
    private Text jellySizePayText;
    [SerializeField, Tooltip("�ڲ��� ���� �ؽ�Ʈ")]
    private Text clickSubText;
    [SerializeField, Tooltip("�ڲ��� ��� �ؽ�Ʈ")]
    private Text clickPayText;


    // ���� ���� ��ư Ŭ��
    public void ClickJellyButton()
    {
        // ���� ���� �ǳ��� ������ �ʴ´ٸ�
        if (!jellyPanelIsShow)
        {
            // ���� ���� ������ �ִϸ��̼� ����
            jellyAnimator.SetTrigger("doShow");
            // jellyPanelIsShow �� ����
            jellyPanelIsShow = true;

            // ���� �÷���Ʈ �ǳ��� �����ִٸ�
            if (platePanelIsShow)
            {
                // �÷���Ʈ �ǳ� ���� �ִϸ��̼� ����
                plateAnimator.SetTrigger("doHide");
                // platePanelIsShow �� ����
                platePanelIsShow = false;
            }
        }
        else
        {
            // ���� ���� ���� �ִϸ��̼� ����
            jellyAnimator.SetTrigger("doHide");
            // jellyPanelIsShow �� ����
            jellyPanelIsShow = false;
        }
    }

    // �÷���Ʈ ���� ��ư Ŭ��
    public void ClickPlateButton()
    {
        // �÷���Ʈ �ǳ��� �������� �ʴٸ�
        if (!platePanelIsShow)
        {
            // �÷���Ʈ �ǳ� ������ �ִϸ��̼� ����
            plateAnimator.SetTrigger("doShow");
            // platePanelIsShow �� ����
            platePanelIsShow = true;

            // ���� ������ �����ִٸ�
            if (jellyPanelIsShow)
            {
                // ���� ���� ���� �ִϸ��̼� ����
                jellyAnimator.SetTrigger("doHide");
                // jellyPanelIsShow �� ����
                jellyPanelIsShow = false;
            }
        }
        else
        {
            // �÷���Ʈ �ǳ� ���� �ִϸ��̼� ����
            plateAnimator.SetTrigger("doHide");
            // platePanelIsShow �� ����
            platePanelIsShow = false;
        }
    }

    // ���� ���� ������ Ȱ��ǥ Ŭ��
    public void JellyShopRightButton()
    {
        // �������� ������ �� ����
        curJellyView++;

        // curJellyView�� ���� ��������Ʈ �������� ���ų� Ŀ����
        if (curJellyView >= jellySpriteList.Length)
        {
            // curJellyView �� ���Ƿ� ����
            curJellyView = jellySpriteList.Length - 1;
            // �޼��� ����
            return;
        }

        // ���� ���� ���� ����
        JellyShopInfoChange();
    }

    // ���� ���� ���� ȭ��ǥ Ŭ��
    public void JellyShopLeftButton()
    {
        // ���� �������� ������ �� ����
        curJellyView--;

        // curJellyView �� 0���� �۾�����
        if (curJellyView < 0)
        {
            // curJellyView �� ���Ƿ� ����
            curJellyView = 0;
            // �޼��� ����
            return;
        }

        // ���� ���� ���� ����
        JellyShopInfoChange();
    }

    // �ر� ��ư Ŭ����
    public void UnLockButtonClick()
    {
        // ���� ���� �������� �ر� ��뺸�� ũ�ų� ���ٸ�
        if (GameManager.Instance.JellyMoney >= jellyUnlockPayList[curJellyView])
        {
            // ���� ���� ���������� �ر� ��븸ŭ ����
            GameManager.Instance.JellyMoney -= jellyUnlockPayList[curJellyView];
            // ���� ���� �ر�
            isLock[curJellyView] = false;
            // ���� ���� �ʱ�ȭ
            JellyShopInfoChange();
            // ���� �ؽ�Ʈ ����
            StartCoroutine(GameManager.Instance.UpDownJellyText(
                           GameManager.Instance.JellyMoney + jellyUnlockPayList[curJellyView],
                           GameManager.Instance.JellyMoney));
        }
        else
        {
            Debug.Log("Not Enught Jelly");
        }
    }

    // ���� ��ư Ŭ����
    public void BuyButtonClick()
    {
        // ���� ���� ��差�� �ر� ��뺸�� ũ�ų� ���ٸ�
        if (GameManager.Instance.GoldMoney >= jellyBuyPayList[curJellyView] && GameManager.Instance.JellyVolumeCheck())
        {
            // ���� ���� ��差���� ���� ��븸ŭ ����
            GameManager.Instance.GoldMoney -= jellyBuyPayList[curJellyView];
            // ���� ����
            JellySpawner.Instance.JellySpawn(curJellyView);
            // ��� �ؽ�Ʈ ����
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + jellyBuyPayList[curJellyView],
                           GameManager.Instance.GoldMoney));

            GameManager.Instance.CurJellyVolume++;
        }
    }

    // ���� ���� ����
    private void JellyShopInfoChange()
    {
        // ���� �������� ������ ����ִٸ� ������ִ� UI Ȱ��ȭ
        lockGroup.SetActive(isLock[curJellyView]);

        // ���� ������ �ر��̵Ǿ��ִٸ�
        if (!isLock[curJellyView])
        {
            // ���� �̹��� ����
            jellyImage.sprite = jellySpriteList[curJellyView];
            // ���� �̸� ǥ��
            jellyName.text = jellyNameList[curJellyView];
            // ���� ���� ��� ǥ��
            jellyPay.text = jellyBuyPayList[curJellyView].ToString();
        }
        else
        {
            // ���� �̹��� ����
            lockJellyImage.sprite = jellySpriteList[curJellyView];
            // ���� �ر� ��� ǥ��
            unlockPay.text = jellyUnlockPayList[curJellyView].ToString();
        }

        // ���� �������� 10���� �۴ٸ�
        if (curJellyView + 1 < 10)
            // ���� �ε��� ��ȣ �տ� #0�� ���δ�.
            curJellyIndex.text = "#0" + (curJellyView + 1).ToString();
        else
            // ���� �ε��� ��ȣ �տ� #�� ���δ�.
            curJellyIndex.text = "#" + (curJellyView + 1).ToString();
    }

    public void JellySizeButtonClick()
    {
        // ���� ���� ��尡 ���׷��̵� ��뺸�� ũ�ų� ������
        if (GameManager.Instance.GoldMoney >= jellySizeGoldList[jellySizeLevel] && jellySizeLevel <= jellySizeGoldList.Length)
        {
            // ������ ���� ����
            jellySizeLevel++;
            // ���� ���� ��差���� ��� ��ŭ ����
            GameManager.Instance.GoldMoney -= jellySizeGoldList[jellySizeLevel - 1];
            // ��� �ؽ�Ʈ ����
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + jellySizeGoldList[jellySizeLevel - 1],
                           GameManager.Instance.GoldMoney));
            // ���� �ִ� ���뷮 ����
            GameManager.Instance.JellyMaxVolume = jellySizeLevel * 2;
        }

        PlayerStateShopInfoChange();

    }

    public void ClickCountButtonClick()
    {
        // ���� ���� ��尡 ���׷��̵� ��뺸�� ũ�ų� ������
        if (GameManager.Instance.GoldMoney >= clickGoldList[clickLevel] && clickLevel <= clickGoldList.Length)
        {
            // Ŭ�� ���� ����
            clickLevel++;
            // ���� ���� ��差���� ��� ��ŭ ����
            GameManager.Instance.GoldMoney -= jellySizeGoldList[jellySizeLevel - 1];
            // ��� �ؽ�Ʈ ����
            StartCoroutine(GameManager.Instance.UpDownGoldText(
                           GameManager.Instance.GoldMoney + clickGoldList[clickLevel - 1],
                           GameManager.Instance.GoldMoney));
            // Ŭ���� ȹ�淮 ��� ���� ����
            GameManager.Instance.ClickCount = clickLevel;
        }

        PlayerStateShopInfoChange();
    }

    private void PlayerStateShopInfoChange()
    {
        jellySizeSubText.text = "���� ���뷮 " + (jellySizeLevel * 2);
        jellySizePayText.text = jellySizeGoldList[jellySizeLevel].ToString();

        clickSubText.text = "Ŭ�� ���� X " + clickLevel;
        clickPayText.text = clickGoldList[clickLevel].ToString();
    }

    // �ɼ� ȭ��
    private void OptionPanel()
    {
        // ESCŰ�� ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // optionPanelIsShow �� ����
            optionPanelIsShow = !optionPanelIsShow;
            // �ɼ� UI Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            optionObject.SetActive(optionPanelIsShow);
            // optionPanelIsShow ���� ���� timeScale �� ����
            Time.timeScale = optionPanelIsShow ? 0 : 1;
        }
    }

    private void Start()
    {
        // ���� ���� ��������Ʈ ����Ʈ ũ�⸸ŭ �޸� �Ҵ�
        isLock = new bool[jellySpriteList.Length];
        // ù ��° �������� �⺻ �ر�
        isLock[0] = false;
        // �̿��� �������� ���
        for (int i = 1; i < isLock.Length; i++)
            isLock[i] = true;
        // ���� ���� �ʱ�ȭ
        JellyShopInfoChange();
        // �÷��̾� �ɷ� ���� �ʱ�ȭ
        PlayerStateShopInfoChange();
    }

    private void Update()
    {
        // ����â �Ѱ� ���� �޼���
        OptionPanel();
    }

}