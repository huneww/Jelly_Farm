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
    /// <param name="zFixation">Z���� ���� ���� ���� true = Z�� ����</param>
    public static void RandomPosition(this Transform tf, Vector3 min, Vector3 max, bool zFixation = false)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = zFixation ? tf.position.z : Random.Range(min.z, max.z);

        tf.position = new Vector3(x, y, z);
    }
}
