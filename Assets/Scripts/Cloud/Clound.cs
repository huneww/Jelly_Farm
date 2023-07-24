using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Clound : MonoBehaviour
{
    [SerializeField, Tooltip("���� �̵� �ӵ�")] private float speed;
    [SerializeField, Tooltip("���� Ÿ��")] private CloundType type;

    private Rigidbody2D rigid;
    private bool isLeft;

    /// <summary>
    /// ������Ʈ �ӵ� ���� �޼���
    /// </summary>
    /// <param name="_isLeft">���ʿ� �����ߴ��� Ȯ��</param>
    public void SetUp(bool _isLeft) 
    {
        // rigid ������Ʈ�� ���ٸ� ȹ��
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();

        // �Ű� ���� ����
        isLeft = _isLeft;

        // ���ʿ� ��ȯ�ƴٸ�
        if (_isLeft)
        {
            // rigid�� �ӵ� ����
            rigid.velocity = Vector3.right * speed;
        }
        else
        {
            // rigid�� �ӵ� ����
            rigid.velocity = Vector3.left * speed;
        }

        // �����ð��� ������ƮǮ�� ��ȯ
        Invoke("ReturnPool", CloundSpawner.Instance.returnTime);

    }

    private void ReturnPool()
    {
        // ���� ������Ʈ�� ������Ʈ Ÿ���� �μ��� Ǯ�� ��ȯ
        CloundSpawner.Instance.InsertQueue(this.gameObject, type);
    }

}
