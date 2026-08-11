using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGUIWindowButton : MonoBehaviour
{
    public bool isTutorialEnd;
    public void Click()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
