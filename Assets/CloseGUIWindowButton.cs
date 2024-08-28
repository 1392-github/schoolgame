using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGUIWindowButton : MonoBehaviour
{
    public bool isTutorialEnd;
    public void Click()
    {
        Player player = GameObject.Find("Player")?.GetComponent<Player>();
        if (player != null && player.tutorial && player.scores.Count != 0 && isTutorialEnd)
        {
            player.End2();
        }
        transform.parent.gameObject.SetActive(false);
    }
}
