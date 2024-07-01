using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBuffer : MonoBehaviour
{
    public bool tutorial;
    public string name;
    public SaveFile1 save;
    public static bool generated;
    void Awake()
    {
        if (generated)
        {
            Destroy(gameObject);
        }
        else
        {
            generated = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
