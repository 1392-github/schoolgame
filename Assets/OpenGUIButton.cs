using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGUIButton : MonoBehaviour
{
    public GameObject target;
    public void Click()
    {
        target.SetActive(true);
    }
}
