using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Date
{
    public class DateSave
    {
        public static void SetDate(string key, float savevalue)
        {
            PlayerPrefs.SetFloat(key, savevalue);
        }

        public static void SetDate(string key, int savevalue)
        {
            PlayerPrefs.SetInt(key, savevalue);
        }
    }

    public class DateLoad
    {
        public static int GetIntDate(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
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
                }
            }

            return PlayerPrefs.GetInt(key);
        }

        public static void GetIsLockDate(string key, out bool[] isLock)
        {
            bool[] returnbool = new bool[UIManager.Instance.jellySpriteList.Length];

            int bitvalue = 1;

            for (int i = 0; i < UIManager.Instance.jellySpriteList.Length; i++)
            {
                int checkvalue = PlayerPrefs.GetInt(key) & bitvalue;
                if (checkvalue == bitvalue)
                {
                    returnbool[i] = true;
                }
                else
                {
                    returnbool[i] = false;
                }
                bitvalue = bitvalue << 1;
            }

            isLock = returnbool;
        }

        public static float GetFloatDate(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                switch (key)
                {
                    case "bgmVolume":
                    case "sfxVolume":
                        PlayerPrefs.SetFloat(key, 1);
                        break;
                }
            }

            return PlayerPrefs.GetFloat(key);
        }
    }

}

