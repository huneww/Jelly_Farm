using Date;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyTouch : MonoBehaviour
{

    [SerializeField, Tooltip("터치에서 드래그로 변경되는데 걸리는 시간")]
    private float dragTime = 1f;

    [SerializeField, Tooltip("오브젝트 위치와 마우스 좌표 차이")]
    private float yOffset = 0.5f;

    // 현재 터치 횟수
    private int touchCount = 0;
    // 현재 터치 횟수 프로퍼티
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
    // 몇 초동안 누르고 있었는지 확인 변수
    private float dragCurTime = 0;
    // 현재 드래그 상태인지 확인 변수
    private bool isDrag = false;
    // 드래그 상태인지 확인 변수 프로퍼티
    public bool IsDrag
    {
        get
        {
            return isDrag;
        }
    }

    // 드래그시 원래위치 저장
    private Vector2 beforPos;
    // Jelly 스크립트 저장 변수
    private Jelly jelly;
    // animator저장 변수
    private Animator animator;

    private void Start()
    {
        jelly = GetComponent<Jelly>();
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        // dragCurTime 0으로 초기화
        dragCurTime = 0;
        // beforPos는 현재 위치로 저장
        beforPos = transform.position;
    }

    private void OnMouseDrag()
    {
        // dragCurTime에 deltaTime을 계속 더한다.
        dragCurTime += Time.deltaTime;

        // dragCurTime이 dragTime 이상이 되면
        if (dragCurTime >= dragTime)
        {
            // isDrag 값을 변경
            if (!isDrag)
                isDrag = true;

            // 게임 매니저의 SelectJelly 값을 현재 오브젝트로 설정
            if (GameManager.Instance.SelectJelly == null)
                GameManager.Instance.SelectJelly = this.gameObject;

            // 오브젝트의 위치를 마우스 위치로 변경
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 오브젝트의 Y축을 yOffset값 만큼 빼서 저장
            transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, 0);
            // 움직이는 코루틴 정지
            Jelly.StopMoveCoroutine();
        }
    }

    private void OnMouseUp()
    {
        // isDrag상태라면
        if (isDrag)
        {
            // isDrag값 변경
            isDrag = false;

            // 현재 판매 UI이 위에있다면
            if (GameManager.Instance.IsSell)
            {
                // 현재 보유 골드 량을 젤리 판매 금액만큼 증가
                GameManager.Instance.GoldMoney += jelly.gold[jelly.level - 1];
                // 골드 증가 코루틴 실행
                StartCoroutine(GameManager.Instance.UpDownGoldText(GameManager.Instance.GoldMoney - jelly.gold[jelly.level - 1],
                                       GameManager.Instance.GoldMoney, this.gameObject));
                // 현재 젤리 보유 수량 감소
                GameManager.Instance.CurJellyVolume--;

                // 판매 효과음 재생
                AudioManager.PlaySFXAudioSource(SFX.Sell);
            }
            else
            {
                // 오브젝트의 위치가 특정 범위를 벗어나면
                if (transform.position.x < GameManager.Instance.jellyBoundBox.bounds.min.x ||
                transform.position.x > GameManager.Instance.jellyBoundBox.bounds.max.x ||
                transform.position.y < GameManager.Instance.jellyBoundBox.bounds.min.y ||
                transform.position.y > GameManager.Instance.jellyBoundBox.bounds.max.y)
                {
                    // 현재 위치를 이전 위치로 변경
                    transform.position = beforPos;
                }
                // 게임 매니저의 SelectJelly 값을 null로 변경
                if (GameManager.Instance.SelectJelly != null)
                {
                    GameManager.Instance.SelectJelly = null;
                }
                // 다시 움직이는 코루틴 실행
                Jelly.StartMoveCoroutine();
            }
            return;
        }

        Touch();
    }

    /// <summary>
    /// 젤리 터치시 호출되는 메서드
    /// </summary>
    public void Touch()
    {
        // 터치 효과음 재생
        AudioManager.PlaySFXAudioSource(SFX.Touch);

        // 애니메이션 변경
        animator.SetTrigger("doTouch");

        // 현재 젤리 재화 증가
        int count = jelly.jelly[jelly.level - 1] * GameManager.Instance.ClickCount;
        GameManager.Instance.JellyMoney += count;

        // 게임 매니저의 UpJellyText 코루틴 실행
        StartCoroutine(GameManager.Instance.UpDownJellyText(
                       GameManager.Instance.JellyMoney - count,
                       GameManager.Instance.JellyMoney));

        // 젤리 터치 카운트 증가
        touchCount++;

        touchCount = touchCount >= 50 ? 50 : touchCount;

        // 터치 카운트가 20이상이고 애니메이터 컨트롤러가 1레벨 컨트롤러라면
        if (touchCount >= 20 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[0])
        {
            // 애니메이터 컨트롤러를 2레벨 컨트롤러로 변경
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[1];
            // 젤리 레벨 증가
            jelly.level = 2;
            // 성장 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Grow);
        }
        // 터치 카운트가 50이상이고 애니메이터 컨트롤러가 2레벨 컨트롤러라면
        if (touchCount >= 50 && animator.runtimeAnimatorController == GameManager.Instance.jellyAnimator[1])
        {
            // 애니메이터 컨트롤러를 2레벨 컨트롤러로 변경
            animator.runtimeAnimatorController = GameManager.Instance.jellyAnimator[2];
            // 젤리 레벨 증가
            jelly.level = 3;
            // 성장 효과음 재생
            AudioManager.PlaySFXAudioSource(SFX.Grow);
        }

        // 젤리 정보 저장
        DateSave.SetJellyDate(jelly.level, touchCount, jelly.index, jelly.bitValue);
    }

}
