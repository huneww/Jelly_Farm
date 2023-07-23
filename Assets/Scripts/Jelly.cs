using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public class Jelly : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField, Tooltip("���� �̵� �ӵ�")]
    private float speed = 1f;

    [SerializeField, Tooltip("���� �� �̵��� �ɸ��� �ð�")]
    private float moveDuration = 1f;

    [SerializeField, Tooltip("�̵� �Ϸ� �� ��� �ð�")]
    private float waitTime = 1f;

    [Space(20), Header("���� ����")]

    [SerializeField, Min(1), Tooltip("���� ����")]
    private int level = 1;

    [SerializeField, Tooltip("������ ���� ȹ�� ��")]
    private int[] jelly;

    [SerializeField, Tooltip("������ ��� ȹ�� ��")]
    private int[] gold;

    [Space(20), Header("��Ÿ")]

    [SerializeField, Tooltip("��ġ���� �巡�׷� ����Ǵµ� �ɸ��� �ð�")]
    private float dragTime = 1f;

    [SerializeField, Tooltip("������Ʈ ��ġ�� ���콺 ��ǥ ����")]
    private float yOffset = 0.5f;

    private BoxCollider2D boxcollider;
    private Animator animator;
    private Vector2 movePos;
    private int touchCount = 0;
    private float dragCurTime = 0;
    private bool isDrag = false;
    private Vector2 beforPos;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // ���� 1 �ִϸ�������Ʈ�ѷ��� ����
        animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[0];

        // ������ �ڷ�ƾ ����
        //StartCoroutine(Move());
    }

    /// <summary>
    /// ���� ��ġ�� ȣ��Ǵ� �޼���
    /// </summary>
    public void Touch()
    {
        // ���� ���� ��ȭ ����
        GameManager.Instance.JellyMoney += jelly[level - 1];
        // ���� �Ŵ����� UpJellyText �ڷ�ƾ ����
        StartCoroutine(GameManager.Instance.
                       UpJellyText(GameManager.Instance.JellyMoney - jelly[level - 1], GameManager.Instance.JellyMoney));

        // �ִϸ��̼� ����
        animator.SetTrigger("doTouch");
        // ���� ��ġ ī��Ʈ ����
        touchCount++;
        // ��ġ ī��Ʈ�� 20�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 1���� ��Ʈ�ѷ����
        if (touchCount >= 20 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[0])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[1];
            // ���� ���� ����
            level = 2;
        }
        // ��ġ ī��Ʈ�� 50�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ����
        if (touchCount >= 50 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[1])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[2];
            // ���� ���� ����
            level = 3;
        }
    }

    private void OnMouseDown()
    {
        dragCurTime = 0;
        beforPos = transform.position;
    }

    private void OnMouseDrag()
    {
        dragCurTime += Time.deltaTime;

        if (dragCurTime >= dragTime)
        {
            if (!isDrag)
                isDrag = true;

            if (GameManager.Instance.SelectJelly == null)
                GameManager.Instance.SelectJelly = this.gameObject;

            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, 0);
        }
    }

    private void OnMouseUp()
    {
        if (isDrag)
        {
            isDrag = false;

            if (GameManager.Instance.IsSell)
            {
                GameManager.Instance.GoldMoney += gold[level - 1];
                StartCoroutine(GameManager.Instance.UpGoldText(GameManager.Instance.GoldMoney - gold[level - 1],
                                       GameManager.Instance.GoldMoney, this.gameObject));
            }
            else
            {
                if (transform.position.x < GameManager.Instance.jellyBoundBox.bounds.min.x ||
                transform.position.x > GameManager.Instance.jellyBoundBox.bounds.max.x ||
                transform.position.y < GameManager.Instance.jellyBoundBox.bounds.min.y ||
                transform.position.y > GameManager.Instance.jellyBoundBox.bounds.max.y)
                {
                    transform.position = beforPos;
                }
                if (GameManager.Instance.SelectJelly != null)
                {
                    GameManager.Instance.SelectJelly = null;
                }
            }
            return;
        }

        Touch();
    }

    private IEnumerator Move()
    {
        // ���� �ð� ��� �� ����
        yield return new WaitForSeconds(Random.Range(0.5f, waitTime));

        // ���� �̵� �������� �� ���� ���� ����
        movePos = new Vector2(Random.Range(GameManager.Instance.jellyBoundBox.bounds.min.x + (boxcollider.size.x / 2),
                                           GameManager.Instance.jellyBoundBox.bounds.max.x - (boxcollider.size.x / 2)),
                              Random.Range(GameManager.Instance.jellyBoundBox.bounds.min.y + (boxcollider.size.y / 2),
                                           GameManager.Instance.jellyBoundBox.bounds.max.y - (boxcollider.size.y / 2)));

        // ��������Ʈ������ ������Ʈ ȹ��
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        // �̵� ��ġ�� ���� ��ġ �� ��, ��������Ʈ ���� ����
        renderer.flipX = movePos.x < transform.position.x;

        // ���� ��ġ ����
        Vector2 nowPos = transform.position;

        // ��� �ð� ���� ����
        float currentTime = 0f;
        // �̵� �ۼ�Ʈ
        float percent = 0f;

        while (percent < 1f)
        {
            // �ִϸ��̼� ����
            animator.SetBool("isWalk", true);

            currentTime += Time.deltaTime;
            percent = currentTime / moveDuration;

            Vector2 pos;

            // pos ���� ���� ��ġ�� �̵� ��ġ�� percent��ŭ�� ������������ �� ����
            pos.x = Mathf.Lerp(nowPos.x, movePos.x, percent);
            pos.y = Mathf.Lerp(nowPos.y, movePos.y, percent);

            // ������������ ����� ��ġ ����
            transform.position = pos;

            yield return null;
        }

        // �ִϸ��̼� Idle ���·� ��ȯ
        animator.SetBool("isWalk", false);

        // �̵� ��ġ�� ���� �ٽ� �ڷ�ƾ ����
        StartCoroutine(Move());
    }
}
