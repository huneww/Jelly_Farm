using Date;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyTouch : MonoBehaviour
{

    [SerializeField, Tooltip("��ġ���� �巡�׷� ����Ǵµ� �ɸ��� �ð�")]
    private float dragTime = 1f;

    [SerializeField, Tooltip("������Ʈ ��ġ�� ���콺 ��ǥ ����")]
    private float yOffset = 0.5f;

    // ���� ��ġ Ƚ��
    private int touchCount = 0;
    // ���� ��ġ Ƚ�� ������Ƽ
    public int TouchCount
    {
        get
        {
            return touchCount;
        }
        set
        {
            touchCount = value;
        }
    }
    // �� �ʵ��� ������ �־����� Ȯ�� ����
    private float dragCurTime = 0;
    // ���� �巡�� �������� Ȯ�� ����
    private bool isDrag = false;
    // �巡�� �������� Ȯ�� ���� ������Ƽ
    public bool IsDrag
    {
        get
        {
            return isDrag;
        }
    }

    // �巡�׽� ������ġ ����
    private Vector2 beforPos;
    // Jelly ��ũ��Ʈ ���� ����
    private Jelly jelly;
    // animator���� ����
    private Animator animator;

    private void Start()
    {
        jelly = GetComponent<Jelly>();
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        // dragCurTime 0���� �ʱ�ȭ
        dragCurTime = 0;
        // beforPos�� ���� ��ġ�� ����
        beforPos = transform.position;
    }

    private void OnMouseDrag()
    {
        // dragCurTime�� deltaTime�� ��� ���Ѵ�.
        dragCurTime += Time.deltaTime;

        // dragCurTime�� dragTime �̻��� �Ǹ�
        if (dragCurTime >= dragTime)
        {
            // isDrag ���� ����
            if (!isDrag)
                isDrag = true;

            // ���� �Ŵ����� SelectJelly ���� ���� ������Ʈ�� ����
            if (GameManager.Instance.SelectJelly == null)
                GameManager.Instance.SelectJelly = this.gameObject;

            // ������Ʈ�� ��ġ�� ���콺 ��ġ�� ����
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // ������Ʈ�� Y���� yOffset�� ��ŭ ���� ����
            transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, 0);
            // �����̴� �ڷ�ƾ ����
            Jelly.StopMoveCoroutine();
        }
    }

    private void OnMouseUp()
    {
        // isDrag���¶��
        if (isDrag)
        {
            // isDrag�� ����
            isDrag = false;

            // ���� �Ǹ� UI�� �����ִٸ�
            if (GameManager.Instance.IsSell)
            {
                // ���� ���� ��� ���� ���� �Ǹ� �ݾ׸�ŭ ����
                GameManager.Instance.GoldMoney += jelly.gold[jelly.level - 1];
                // ��� ���� �ڷ�ƾ ����
                StartCoroutine(GameManager.Instance.UpDownGoldText(GameManager.Instance.GoldMoney - jelly.gold[jelly.level - 1],
                                       GameManager.Instance.GoldMoney, this.gameObject));
                // ���� ���� ���� ���� ����
                GameManager.Instance.CurJellyVolume--;

                // �Ǹ� ȿ���� ���
                AudioManager.PlaySFXAudioSource(SFX.Sell);
            }
            else
            {
                // ������Ʈ�� ��ġ�� Ư�� ������ �����
                if (transform.position.x < GameManager.Instance.jellyBoundBox.bounds.min.x ||
                transform.position.x > GameManager.Instance.jellyBoundBox.bounds.max.x ||
                transform.position.y < GameManager.Instance.jellyBoundBox.bounds.min.y ||
                transform.position.y > GameManager.Instance.jellyBoundBox.bounds.max.y)
                {
                    // ���� ��ġ�� ���� ��ġ�� ����
                    transform.position = beforPos;
                }
                // ���� �Ŵ����� SelectJelly ���� null�� ����
                if (GameManager.Instance.SelectJelly != null)
                {
                    GameManager.Instance.SelectJelly = null;
                }
                // �ٽ� �����̴� �ڷ�ƾ ����
                Jelly.StartMoveCoroutine();
            }
            return;
        }

        Touch();
    }

    /// <summary>
    /// ���� ��ġ�� ȣ��Ǵ� �޼���
    /// </summary>
    public void Touch()
    {
        // ��ġ ȿ���� ���
        AudioManager.PlaySFXAudioSource(SFX.Touch);

        // �ִϸ��̼� ����
        animator.SetTrigger("doTouch");

        // ���� ���� ��ȭ ����
        int count = jelly.jelly[jelly.level - 1] * GameManager.Instance.ClickCount;
        GameManager.Instance.JellyMoney += count;

        // ���� �Ŵ����� UpJellyText �ڷ�ƾ ����
        StartCoroutine(GameManager.Instance.UpDownJellyText(
                       GameManager.Instance.JellyMoney - count,
                       GameManager.Instance.JellyMoney));

        // ���� ��ġ ī��Ʈ ����
        touchCount++;

        touchCount = touchCount >= 50 ? 50 : touchCount;

        // ��ġ ī��Ʈ�� 20�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 1���� ��Ʈ�ѷ����
        if (touchCount >= 20 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[0])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[1];
            // ���� ���� ����
            jelly.level = 2;
            // ���� ȿ���� ���
            AudioManager.PlaySFXAudioSource(SFX.Grow);
        }
        // ��ġ ī��Ʈ�� 50�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ����
        if (touchCount >= 50 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[1])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[2];
            // ���� ���� ����
            jelly.level = 3;
            // ���� ȿ���� ���
            AudioManager.PlaySFXAudioSource(SFX.Grow);
        }

        // ���� ���� ����
        DateSave.SetJellyDate(jelly.level, touchCount, jelly.index, jelly.bitValue);
    }

}
