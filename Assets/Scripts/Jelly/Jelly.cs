using Date;
using System.Collections;
using System.Collections.Generic;
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

    [Min(1), Tooltip("젤리 레벨")]
    public int level = 1;

    [Tooltip("레벨별 젤리 획득 양")]
    public int[] jelly;

    [Tooltip("레벨별 골드 획득 양")]
    public int[] gold;

    [HideInInspector]
    // 젤리 인덱스값 저장 변수
    public int index;
    [HideInInspector]
    // 젤리 저장에 쓰일 비트값 저장 변수
    public int bitValue;
    
    private BoxCollider2D boxcollider;
    private Animator animator;
    private Vector2 movePos;
    private JellyTouch jellyTouch;

    // Move코루틴 메서드 실행, 정지시키는 Action 변수
    public static System.Action StartMoveCoroutine;
    public static System.Action StopMoveCoroutine;
    // Move코루틴 저장 변수
    private IEnumerator moveCoroutine;

    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        jellyTouch = GetComponent<JellyTouch>();

        // Action변수에 메서드 저장
        StartMoveCoroutine = () => { StartCoroutineMethod(); };
        StopMoveCoroutine = () => { StopCoroutineMethod(); };
        // 젤리 소환시 데이터 저장
        DateSave.SetJellyDate(level, jellyTouch.TouchCount, index, bitValue);
        // Move 코루틴 실행 메서드
        StartCoroutineMethod();

    }

    private void StartCoroutineMethod()
    {
        // Move 코루틴 저장
        moveCoroutine = Move();
        // 코루틴 실행
        StartCoroutine(moveCoroutine);
    }

    private void StopCoroutineMethod()
    {
        // 애니메이션 변경
        animator.SetBool("isWalk", false);
        // 코루틴 정지
        StopCoroutine(moveCoroutine);
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

        // 애니메이션 변경
        animator.SetBool("isWalk", true);

        while (percent < 1f)
        {
            // 젤리가 드래그 상태가 되면
            if (GetComponent<JellyTouch>().IsDrag)
            {
                // 애니메이션 변경
                animator.SetBool("isWalk", false);
                // 코루틴 메서드 중지
                yield break;
            }

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
