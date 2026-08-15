using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverCanvas : MonoBehaviour
{
    public HoverTextObject hoverText;
    public static HoverCanvas instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
