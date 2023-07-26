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
    /// <param name="zFreeze">Z축을 고정 할지 여부 true = Z축 고정</param>
    public static void RandomPosition(this Transform tf, Vector3 min, Vector3 max, bool zFreeze = false)
    {
        // 지정된 최소 최대값 사이의 값 저장
        float x = Random.Range(min.x, max.x);
        // 지정된 최소 최대값 사이의 값 저장
        float y = Random.Range(min.y, max.y);
        // Z축을 고정할건지를 확인하고
        // 고정한다면 해당 오브젝트의 Z축 값을 저장
        // 변경한다면 지정된 최소 최대값 사이의 값 저장
        float z = zFreeze ? tf.position.z : Random.Range(min.z, max.z);

        // 해당 오브젝트의 위치를 변경
        tf.position = new Vector3(x, y, z);
    }
}
