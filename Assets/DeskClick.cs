using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskClick : MonoBehaviour
{
    public void Click()
    {
        if (GameData.inSchool && !GameData.inClass)
        {
            GameData.timeSpeed = new System.TimeSpan(0, 5, 0);
        }
    }
}
