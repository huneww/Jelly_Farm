using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour
{
    private static JellySpawner instance;

    public static JellySpawner Instance
    {
        get
        {
            GameObject obj = GameObject.Find("JellySpawner");
            if (obj == null)
            {
                obj = new GameObject("JellySpawner");
                instance = obj.AddComponent<JellySpawner>();
            }
            else
            {
                instance = obj.GetComponent<JellySpawner>();
            }

            return instance;
        }
    }

    [SerializeField, Tooltip("젤리 프리펩")]
    private GameObject[] jellyPrefabs;

    /// <summary>
    /// 구매 젤리 스폰
    /// </summary>
    /// <param name="jellyIndex">스폰할 젤리 인덱스 번호</param>
    public void JellySpawn(int jellyIndex)
    {
        // 젤리 오브젝트 획득
        GameObject jelly = Instantiate(jellyPrefabs[jellyIndex]);
        // 구매한 젤리 스프라이트로 변경
        jelly.GetComponent<SpriteRenderer>().sprite = UIManager.Instance.jellySpriteList[jellyIndex];
        // 생성된 오브젝트 위치 랜덤 지정
        jelly.transform.RandomPosition(
            GameManager.Instance.jellyBoundBox.bounds.min,
            GameManager.Instance.jellyBoundBox.bounds.max,
            true);
        jelly.transform.parent = this.transform;
        jelly.GetComponent<Jelly>().level = 1;
        jelly.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.jellyAnimator[0];
    }

    /// <summary>
    /// 반환 젤리
    /// </summary>
    /// <param name="jelly">반환할 젤리</param>
    public void JellyDiSpawn(GameObject jelly)
    {
        Destroy(jelly);
    }

}
