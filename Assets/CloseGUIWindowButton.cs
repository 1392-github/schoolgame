using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGUIWindowButton : MonoBehaviour
{
    public bool isTutorialEnd;
    public void Click()
    {
        Player player = GameObject.Find("Player")?.GetComponent<Player>();
        if (isTutorialEnd && player != null && GameData.tutorial && GameData.scores.Count != 0)
        {
            player.End2();
        }
        transform.parent.gameObject.SetActive(false);
    }
}
