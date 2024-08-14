using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToNextTutorial : MonoBehaviour
{
    public int chat;
    public int ele;
    Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Click()
    {
        if (player.currentChat == chat && player.currentChatElement == ele)
        {
            player.NextChat2();
        }
    }
}
