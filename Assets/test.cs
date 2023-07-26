using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    public float a;

    private void Start()
    {
        a = PlayerPrefs.GetFloat("asdfasdfasdfasdf");
    }
}
