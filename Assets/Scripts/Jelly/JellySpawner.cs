using Date;
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
    public void JellySpawn(int jellyIndex = 0, int bitvalue = 0, int jellyLevel = 1, int jellyTouchCount = 0)
    {
        // 현재 수용중인 젤리 수량 증가
        GameManager.Instance.CurJellyVolume++;
        // 젤리 오브젝트 획득
        GameObject jelly = Instantiate(jellyPrefabs[jellyIndex]);
        // 구매한 젤리 스프라이트로 변경
        jelly.GetComponent<SpriteRenderer>().sprite = UIManager.Instance.jellySpriteList[jellyIndex];
        // 생성된 오브젝트 위치 랜덤 지정
        jelly.transform.RandomPosition(
            GameManager.Instance.jellyBoundBox.bounds.min,
            GameManager.Instance.jellyBoundBox.bounds.max,
            true);

        // 젤리 정보 초기화
        jelly.transform.parent = this.transform;
        jelly.GetComponent<Jelly>().level = jellyLevel;
        jelly.GetComponent<Jelly>().index = jellyIndex;
        jelly.GetComponent<Jelly>().bitValue = bitvalue == 0 ? DateLoad.GetJellyDateEmptyBit() : bitvalue;
        jelly.GetComponent<JellyTouch>().TouchCount = jellyTouchCount;
        jelly.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.jellyAnimator[jellyLevel - 1];
    }

    /// <summary>
    /// 젤리 판매 메서드
    /// </summary>
    /// <param name="jelly">반환할 젤리</param>
    public void JellyDiSpawn(GameObject jelly)
    {
        // 저장된 데이터 삭제
        PlayerPrefs.DeleteKey("Jelly" + jelly.GetComponent<Jelly>().bitValue);
        // 젤리 데이터 비트값 획득
        int bitValue = DateLoad.GetIntDate("jellyDateBit");
        // 삭제할 젤리의 비트값과 XOR연산자로 계산
        bitValue = bitValue ^ jelly.GetComponent<Jelly>().bitValue;
        // 계산한 비트값 저장
        DateSave.SetDate("jellyDateBit", bitValue);
        // 오브젝트 삭제
        Destroy(jelly);
    }

}
