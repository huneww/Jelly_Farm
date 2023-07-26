using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Date
{
    public class DateSave
    {
        /// <summary>
        /// Float�� ������ ���� �޼���
        /// </summary>
        /// <param name="key">������ ������ Ű</param>
        /// <param name="savevalue">������ ����</param>
        public static void SetDate(string key, float savevalue)
        {
            PlayerPrefs.SetFloat(key, savevalue);
        }

        /// <summary>
        /// Int�� ������ ���� �޼���
        /// </summary>
        /// <param name="key">������ ������ Ű</param>
        /// <param name="savevalue">������ ����</param>
        public static void SetDate(string key, int savevalue)
        {
            PlayerPrefs.SetInt(key, savevalue);
        }

        /// <summary>
        /// ���� ���� ���� �޼���
        /// </summary>
        /// <param name="jellyLevel">���� ����</param>
        /// <param name="jellyTouchCout">���� ��ġ Ƚ��</param>
        /// <param name="jellyIndex">���� �ε��� ��ȣ</param>
        /// <param name="bitValue">������ ����� ��Ʈ</param>
        public static void SetJellyDate(int jellyLevel, int jellyTouchCout, int jellyIndex, int bitValue)
        {
            // ���� ������ Ư�������� ����
            float saveDate = ((((jellyLevel * 100) + jellyTouchCout) * 100) + jellyIndex) / 10000;
            // �Ű������ο� ��Ʈ���� ���� ������ ����
            SetDate("Jelly" + bitValue, saveDate);
        }

    }

    public class DateLoad
    {
        /// <summary>
        /// Int�� ������ ȹ�� �޼���
        /// </summary>
        /// <param name="key">ȹ���� ������ Ű</param>
        /// <returns></returns>
        public static int GetIntDate(string key)
        {
            // Ű�� �������� �ʴٸ�
            if (!PlayerPrefs.HasKey(key))
            {
                // Ű������ ������ ����
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

            // Ű�� �´� ������ ��ȯ
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// ���� �ر��� �Ǿ��ִ� Ȯ�� ���� ȹ�� �޼���
        /// </summary>
        /// <param name="key">ȹ���� ������ Ű</param>
        /// <param name="isLock">��ȯ�� �迭 ����</param>
        public static void GetIsLockDate(string key, out bool[] isLock)
        {
            // isLock�� ��ȯ�� ������ ����
            bool[] returnbool = new bool[UIManager.Instance.jellySpriteList.Length];

            // ����� ��Ʈ Ȯ�� ����
            int bitvalue = 1;

            // �����ϴ� ���� ����ŭ �ݺ�
            for (int i = 0; i < UIManager.Instance.jellySpriteList.Length; i++)
            {
                // ����� ��Ʈ���� AND�����ڷ� Ȯ��
                int checkvalue = PlayerPrefs.GetInt(key) & bitvalue;
                // AND�����ڷ� ����ؼ� checkvalue�� bitvalue�� ���� ���ٸ�
                if (checkvalue == bitvalue)
                {
                    // i��°�� ����ִ�
                    returnbool[i] = true;
                }
                else
                {
                    // i��°�� �رݵǾ��ִ�.
                    returnbool[i] = false;
                }
                // ��Ʈ�� ����
                bitvalue = bitvalue << 1;
            }

            // returnbool���� �Ѱ� �ش�
            isLock = returnbool;
        }

        /// <summary>
        /// Float�� ������ ȹ�� �޼���
        /// </summary>
        /// <param name="key">ȹ���� ���� Ű</param>
        /// <returns></returns>
        public static float GetFloatDate(string key)
        {
            // Ű�� �������� �ʴٸ�
            if (!PlayerPrefs.HasKey(key))
            {
                // Ű�� �°� ���� ����
                switch (key)
                {
                    case "bgmVolume":
                    case "sfxVolume":
                        PlayerPrefs.SetFloat(key, 1);
                        break;
                }
            }

            // Ű�� �´� ������ ��ȯ
            return PlayerPrefs.GetFloat(key);
        }

        /// <summary>
        /// ����� ���� ������ ȹ�� �޼���
        /// </summary>
        /// <param name="key">ȹ���� ���� Ű</param>
        /// <param name="jellyLevel">���� ������ ��ȯ ���� ����</param>
        /// <param name="jellyTouchCount">���� ��ġ Ƚ���� ��ȯ ���� ����</param>
        /// <param name="jellyIndex">���� �ε����� ��ȯ ���� ����</param>
        public static void GetJellyDate(string key, out int jellyLevel, out int jellyTouchCount, out int jellyIndex)
        {
            // �ִ밪�� 3.5012 �� �����̵��ִٰ� ����
            float saveValue = GetFloatDate(key);

            // �����Ͱ� �ִ��� Ȯ��
            if (saveValue < 1 || saveValue > 4)
            {
                jellyLevel = -1;
                jellyTouchCount = -1;
                jellyIndex = -1;
                return;
            }

            // �Ҽ����� ��� ������ 3�̶�� ������ ȹ��
            int _jellyLevel = Mathf.FloorToInt(saveValue);
            // 3.5012 - 3�� ���� 100�� ���Ͽ� 50.12�� ȹ���ϰ� �Ҽ����� ��� ���� 50�̶�� ��ġī��Ʈ�� ȹ��
            int _jellyTouchCount =  Mathf.FloorToInt(((saveValue - _jellyLevel) * 100));
            // 50.12 ��� ���� �ӽ÷� ����
            float value = (saveValue - _jellyLevel) * 100;
            // 50.12 ���� ��ġī��Ʈ�� 50�� ���� 100�� ���Ͽ� 12.000�� ȹ�� �Ҽ����� ��� ���� ���� �ε����� 12�� ȹ��
            int _jellyIndex = Mathf.FloorToInt(((value - _jellyTouchCount) * 100));

            // Ȯ�ε� ������ ��ȯ
            jellyLevel = _jellyLevel;
            jellyTouchCount = _jellyTouchCount;
            jellyIndex = _jellyIndex;
        }

        /// <summary>
        /// ���� ���� ������ ��Ʈ���� ����ִ� ��Ʈ���� ȹ���ϴ� �޼���
        /// </summary>
        /// <returns></returns>
        public static int GetJellyDateEmptyBit()
        {
            // ���� ������ ��Ʈ�� ȹ��
            int bitvalue = GetIntDate("jellyDateBit");
            // ȹ���� ������ ��Ʈ�� ���� ����
            int bitCheckValue = 1;

            // 12ȸ �ݺ��ؼ� Ȯ��
            for (int i = 0; i < 12; i++)
            {
                // bitvalue�� bitCheckValue�� AND�����ڷ� ���
                int check = bitvalue & bitCheckValue;
                // bitCheckValue�� check�� �����ʴٸ�
                // ���� ��Ʈ���� �����Ͱ� ���� ���̹Ƿ� check�� ��ȯ
                if (check != bitCheckValue) return check;

                // bitCheckValue�� ����
                bitCheckValue = bitCheckValue << 1;
            }

            // ������� ���ٸ� -1�� ��ȯ
            return -1;
        }
    }

}

