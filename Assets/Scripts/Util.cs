using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    /// <summary>
    /// 랜더 위치 지정
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="min">선택될수 있는 최소</param>
    /// <param name="max">선택될수 있는 최대</param>
    /// <param name="zFixation">Z축을 고정 할지 여부 true = Z축 고정</param>
    public static void RandomPosition(this Transform tf, Vector3 min, Vector3 max, bool zFixation = false)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = zFixation ? tf.position.z : Random.Range(min.z, max.z);

        tf.position = new Vector3(x, y, z);
    }
}
