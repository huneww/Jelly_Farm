using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// ���� ��ġ ����
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="min">���õɼ� �ִ� �ּ�</param>
    /// <param name="max">���õɼ� �ִ� �ִ�</param>
    /// <param name="zFreeze">Z���� ���� ���� ���� true = Z�� ����</param>
    public static void RandomPosition(this Transform tf, Vector3 min, Vector3 max, bool zFreeze = false)
    {
        // ������ �ּ� �ִ밪 ������ �� ����
        float x = Random.Range(min.x, max.x);
        // ������ �ּ� �ִ밪 ������ �� ����
        float y = Random.Range(min.y, max.y);
        // Z���� �����Ұ����� Ȯ���ϰ�
        // �����Ѵٸ� �ش� ������Ʈ�� Z�� ���� ����
        // �����Ѵٸ� ������ �ּ� �ִ밪 ������ �� ����
        float z = zFreeze ? tf.position.z : Random.Range(min.z, max.z);

        // �ش� ������Ʈ�� ��ġ�� ����
        tf.position = new Vector3(x, y, z);
    }
}
