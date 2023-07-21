using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public class Jelly : MonoBehaviour
{
    [Tooltip("���� �̵� �ӵ�")]
    public float speed = 1f;
    [Tooltip("���� �� �̵��� �ɸ��� �ð�")]
    public float moveDuration = 1f;
    [Tooltip("�̵� �Ϸ� �� ��� �ð�")]
    public float waitTime = 1f;
    [Tooltip("�� ���� �� ���� �ִϸ�������Ʈ�ѷ�")]
    public AnimatorController[] animatorController;
    [Min(1), Tooltip("���� ����")]
    public int level = 1;

    private BoxCollider2D boxcollider;
    private Animator animator;
    private Vector2 movePos;
    private int touchCount = 0;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // ���� 1 �ִϸ�������Ʈ�ѷ��� ����
        animator.runtimeAnimatorController = animatorController[0];

        // ������ �ڷ�ƾ ����
        StartCoroutine(Move());
    }

    /// <summary>
    /// ���� ��ġ�� ȣ��Ǵ� �޼���
    /// </summary>
    public void Touch()
    {
        // �ִϸ��̼� ����
        animator.SetTrigger("doTouch");
        // ���� ��ġ ī��Ʈ ����
        touchCount++;

        // ��ġ ī��Ʈ�� 20�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 1���� ��Ʈ�ѷ����
        if (touchCount >= 20 && animator.runtimeAnimatorController == animatorController[0])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = animatorController[1];
            // ���� ���� ����
            level = 2;
        }
        // ��ġ ī��Ʈ�� 50�̻��̰� �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ����
        if (touchCount >= 50 && animator.runtimeAnimatorController == animatorController[1])
        {
            // �ִϸ����� ��Ʈ�ѷ��� 2���� ��Ʈ�ѷ��� ����
            animator.runtimeAnimatorController = animatorController[2];
            // ���� ���� ����
            level = 3;
        }
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
