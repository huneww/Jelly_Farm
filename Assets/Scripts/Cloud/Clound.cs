using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Clound : MonoBehaviour
{
    [SerializeField, Tooltip("구름 이동 속도")] private float speed;
    [SerializeField, Tooltip("구름 타입")] private CloundType type;

    private Rigidbody2D rigid;
    private bool isLeft;

    /// <summary>
    /// 오브젝트 속도 변경 메서드
    /// </summary>
    /// <param name="_isLeft">왼쪽에 스폰했는지 확인</param>
    public void SetUp(bool _isLeft) 
    {
        // rigid 컴포넌트가 없다면 획득
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();

        // 매개 변수 저장
        isLeft = _isLeft;

        // 왼쪽에 소환됐다면
        if (_isLeft)
        {
            // rigid의 속도 변경
            rigid.velocity = Vector3.right * speed;
        }
        else
        {
            // rigid의 속도 변경
            rigid.velocity = Vector3.left * speed;
        }

        // 일정시간후 오브젝트풀에 반환
        Invoke("ReturnPool", CloundSpawner.Instance.returnTime);

    }

    private void ReturnPool()
    {
        // 현재 오브젝트와 오브젝트 타입을 인수로 풀에 반환
        CloundSpawner.Instance.InsertQueue(this.gameObject, type);
    }

}
