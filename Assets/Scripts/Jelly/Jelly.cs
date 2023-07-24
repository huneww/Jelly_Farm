using System.Collections;
using System.Collections.Generic;
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

    [Min(1), Tooltip("���� ����")]
    public int level = 1;

    [Tooltip("������ ���� ȹ�� ��")]
    public int[] jelly;

    [Tooltip("������ ��� ȹ�� ��")]
    public int[] gold;

    [Space(20), Header("��Ÿ")]


    private BoxCollider2D boxcollider;
    private Animator animator;
    private Vector2 movePos;

    public static System.Action StartMoveCoroutine;
    public static System.Action StopMoveCoroutine;

    private IEnumerator moveCoroutine;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        StartMoveCoroutine = () => { StartCoroutineMethod(); };
        StopMoveCoroutine = () => { StopCoroutineMethod(); };

        // ���� 1 �ִϸ�������Ʈ�ѷ��� ����
        animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[0];

        StartCoroutineMethod();

    }

    private void StartCoroutineMethod()
    {
        moveCoroutine = Move();
        StartCoroutine(moveCoroutine);
    }

    private void StopCoroutineMethod()
    {
        animator.SetBool("isWalk", false);
        StopCoroutine(moveCoroutine);
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

        // �ִϸ��̼� ����
        animator.SetBool("isWalk", true);

        while (percent < 1f)
        {
            if (GetComponent<JellyTouch>().IsDrag)
            {
                animator.SetBool("isWalk", false);
                yield break;
            }

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
