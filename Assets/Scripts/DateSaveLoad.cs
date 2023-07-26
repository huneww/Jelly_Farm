using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Date
{
    public class DateSave
    {
        /// <summary>
        /// Float형 데이터 저장 메서드
        /// </summary>
        /// <param name="key">저장할 벨류의 키</param>
        /// <param name="savevalue">저장할 벨류</param>
        public static void SetDate(string key, float savevalue)
        {
            PlayerPrefs.SetFloat(key, savevalue);
        }

        /// <summary>
        /// Int형 데이터 저장 메서드
        /// </summary>
        /// <param name="key">저장할 벨류의 키</param>
        /// <param name="savevalue">저장할 벨류</param>
        public static void SetDate(string key, int savevalue)
        {
            PlayerPrefs.SetInt(key, savevalue);
        }

        /// <summary>
        /// 젤리 정보 저장 메서드
        /// </summary>
        /// <param name="jellyLevel">젤리 레벨</param>
        /// <param name="jellyTouchCout">젤리 터치 횟수</param>
        /// <param name="jellyIndex">젤리 인덱스 번호</param>
        /// <param name="bitValue">젤리가 저장될 비트</param>
        public static void SetJellyDate(int jellyLevel, int jellyTouchCout, int jellyIndex, int bitValue)
        {
            // 젤리 정보를 특정값으로 변경
            float saveDate = ((((jellyLevel * 100) + jellyTouchCout) * 100) + jellyIndex) / 10000;
            // 매개변수로온 비트값에 따라 데이터 저장
            SetDate("Jelly" + bitValue, saveDate);
        }

    }

    public class DateLoad
    {
        /// <summary>
        /// Int형 데이터 획득 메서드
        /// </summary>
        /// <param name="key">획득할 벨류의 키</param>
        /// <returns></returns>
        public static int GetIntDate(string key)
        {
            // 키가 존재하지 않다면
            if (!PlayerPrefs.HasKey(key))
            {
                // 키에따라 데이터 생성
                switch (key)
                {
                    case "jellyMoney":
                    case "goldMoney":
                        PlayerPrefs.SetInt(key, 1000);
                        break;
                    case "isLock":
                        PlayerPrefs.SetInt(key, 1);
                        break;
                    case "jellySizeLevel":
                    case "clickLevel":
                        PlayerPrefs.SetInt(key, 1);
                        break;
                    case "jellyDateBit":
                        PlayerPrefs.SetInt(key, 0);
                        break;
                }
            }

            // 키에 맞는 데이터 반환
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// 젤리 해금이 되어있는 확인 변수 획득 메서드
        /// </summary>
        /// <param name="key">획득할 벨류의 키</param>
        /// <param name="isLock">반환할 배열 변수</param>
        public static void GetIsLockDate(string key, out bool[] isLock)
        {
            // isLock에 반환할 데이터 생성
            bool[] returnbool = new bool[UIManager.Instance.jellySpriteList.Length];

            // 저장된 비트 확인 변수
            int bitvalue = 1;

            // 존재하는 젤리 수만큼 반복
            for (int i = 0; i < UIManager.Instance.jellySpriteList.Length; i++)
            {
                // 저장된 비트값과 AND연산자로 확인
                int checkvalue = PlayerPrefs.GetInt(key) & bitvalue;
                // AND연산자로 계산해서 checkvalue와 bitvalue의 값이 같다면
                if (checkvalue == bitvalue)
                {
                    // i번째는 잠겨있다
                    returnbool[i] = true;
                }
                else
                {
                    // i번째는 해금되어있다.
                    returnbool[i] = false;
                }
                // 비트값 변경
                bitvalue = bitvalue << 1;
            }

            // returnbool값을 넘겨 준다
            isLock = returnbool;
        }

        /// <summary>
        /// Float형 데이터 획득 메서드
        /// </summary>
        /// <param name="key">획득할 변수 키</param>
        /// <returns></returns>
        public static float GetFloatDate(string key)
        {
            // 키가 존재하지 않다면
            if (!PlayerPrefs.HasKey(key))
            {
                // 키에 맞게 새로 생성
                switch (key)
                {
                    case "bgmVolume":
                    case "sfxVolume":
                        PlayerPrefs.SetFloat(key, 1);
                        break;
                }
            }

            // 키에 맞는 벨류값 반환
            return PlayerPrefs.GetFloat(key);
        }

        /// <summary>
        /// 저장된 젤리 데이터 획득 메서드
        /// </summary>
        /// <param name="key">획득할 벨류 키</param>
        /// <param name="jellyLevel">젤리 레벨을 반환 받을 변수</param>
        /// <param name="jellyTouchCount">젤리 터치 횟수를 반환 받을 변수</param>
        /// <param name="jellyIndex">젤리 인덱스를 반환 받을 변수</param>
        public static void GetJellyDate(string key, out int jellyLevel, out int jellyTouchCount, out int jellyIndex)
        {
            // 최대값인 3.5012 가 저장이되있다고 가정
            float saveValue = GetFloatDate(key);

            // 데이터가 있는지 확인
            if (saveValue < 1 || saveValue > 4)
            {
                jellyLevel = -1;
                jellyTouchCount = -1;
                jellyIndex = -1;
                return;
            }

            // 소숫점을 모두 버리고 3이라는 레벨을 획득
            int _jellyLevel = Mathf.FloorToInt(saveValue);
            // 3.5012 - 3을 한후 100을 곱하여 50.12을 획득하고 소숫점을 모두 버려 50이라는 터치카운트를 획득
            int _jellyTouchCount =  Mathf.FloorToInt(((saveValue - _jellyLevel) * 100));
            // 50.12 라는 값을 임시로 저장
            float value = (saveValue - _jellyLevel) * 100;
            // 50.12 에서 터치카운트이 50을 빼고 100을 곱하여 12.000을 획득 소숫점을 모두 버려 젤리 인덱스인 12를 획득
            int _jellyIndex = Mathf.FloorToInt(((value - _jellyTouchCount) * 100));

            // 확인덴 변수를 반환
            jellyLevel = _jellyLevel;
            jellyTouchCount = _jellyTouchCount;
            jellyIndex = _jellyIndex;
        }

        /// <summary>
        /// 현재 젤리 데이터 비트에서 비어있는 비트값을 획득하는 메서드
        /// </summary>
        /// <returns></returns>
        public static int GetJellyDateEmptyBit()
        {
            // 젤리 데이터 비트를 획득
            int bitvalue = GetIntDate("jellyDateBit");
            // 획득한 데이터 비트와 비교할 변수
            int bitCheckValue = 1;

            // 12회 반복해서 확인
            for (int i = 0; i < 12; i++)
            {
                // bitvalue와 bitCheckValue를 AND연산자로 계산
                int check = bitvalue & bitCheckValue;
                // bitCheckValue와 check가 같지않다면
                // 현재 비트에는 데이터가 없는 것이므로 check를 반환
                if (check != bitCheckValue) return check;

                // bitCheckValue값 변경
                bitCheckValue = bitCheckValue << 1;
            }

            // 빈공간이 없다면 -1을 반환
            return -1;
        }
    }

}

