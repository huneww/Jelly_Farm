using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public class Jelly : MonoBehaviour
{
    [Header("이동 관련")]
    [SerializeField, Tooltip("젤리 이동 속도")]
    private float speed = 1f;

    [SerializeField, Tooltip("젤리 총 이동에 걸리는 시간")]
    private float moveDuration = 1f;

    [SerializeField, Tooltip("이동 완료 후 대기 시간")]
    private float waitTime = 1f;

    [Space(20), Header("레벨 관련")]

    [SerializeField, Min(1), Tooltip("젤리 레벨")]
    private int level = 1;

    [SerializeField, Tooltip("레벨별 젤리 획득 양")]
    private int[] jelly;

    [SerializeField, Tooltip("레벨별 골드 획득 양")]
    private int[] gold;

    [Space(20), Header("기타")]

    [SerializeField, Tooltip("터치에서 드래그로 변경되는데 걸리는 시간")]
    private float dragTime = 1f;

    [SerializeField, Tooltip("오브젝트 위치와 마우스 좌표 차이")]
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

        // 레벨 1 애니메이터컨트롤러로 변경
        animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[0];

        // 움직임 코루틴 실행
        //StartCoroutine(Move());
    }

    /// <summary>
    /// 젤리 터치시 호출되는 메서드
    /// </summary>
    public void Touch()
    {
        // 현재 젤리 재화 증가
        GameManager.Instance.JellyMoney += jelly[level - 1];
        // 게임 매니저의 UpJellyText 코루틴 실행
        StartCoroutine(GameManager.Instance.
                       UpJellyText(GameManager.Instance.JellyMoney - jelly[level - 1], GameManager.Instance.JellyMoney));

        // 애니메이션 변경
        animator.SetTrigger("doTouch");
        // 젤리 터치 카운트 증가
        touchCount++;
        // 터치 카운트가 20이상이고 애니메이터 컨트롤러가 1레벨 컨트롤러라면
        if (touchCount >= 20 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[0])
        {
            // 애니메이터 컨트롤러를 2레벨 컨트롤러로 변경
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[1];
            // 젤리 레벨 증가
            level = 2;
        }
        // 터치 카운트가 50이상이고 애니메이터 컨트롤러가 2레벨 컨트롤러라면
        if (touchCount >= 50 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[1])
        {
            // 애니메이터 컨트롤러를 2레벨 컨트롤러로 변경
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[2];
            // 젤리 레벨 증가
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
        // 일정 시간 대기 후 실행
        yield return new WaitForSeconds(Random.Range(0.5f, waitTime));

        // 젤리 이동 영역에서 한 지점 랜덤 선택
        movePos = new Vector2(Random.Range(GameManager.Instance.jellyBoundBox.bounds.min.x + (boxcollider.size.x / 2),
                                           GameManager.Instance.jellyBoundBox.bounds.max.x - (boxcollider.size.x / 2)),
                              Random.Range(GameManager.Instance.jellyBoundBox.bounds.min.y + (boxcollider.size.y / 2),
                                           GameManager.Instance.jellyBoundBox.bounds.max.y - (boxcollider.size.y / 2)));

        // 스프라이트랜더러 컴포넌트 획득
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        // 이동 위치와 현재 위치 비교 후, 스프라이트 반전 설정
        renderer.flipX = movePos.x < transform.position.x;

        // 현재 위치 저장
        Vector2 nowPos = transform.position;

        // 경과 시간 저장 변수
        float currentTime = 0f;
        // 이동 퍼센트
        float percent = 0f;

        while (percent < 1f)
        {
            // 애니메이션 변경
            animator.SetBool("isWalk", true);

            currentTime += Time.deltaTime;
            percent = currentTime / moveDuration;

            Vector2 pos;

            // pos 값을 현재 위치와 이동 위치의 percent만큼의 선형보간으로 값 저장
            pos.x = Mathf.Lerp(nowPos.x, movePos.x, percent);
            pos.y = Mathf.Lerp(nowPos.y, movePos.y, percent);

            // 선형보간으로 저장된 위치 저장
            transform.position = pos;

            yield return null;
        }

        // 애니메이션 Idle 상태로 전환
        animator.SetBool("isWalk", false);

        // 이동 위치로 가면 다시 코루틴 실행
        StartCoroutine(Move());
    }
}
