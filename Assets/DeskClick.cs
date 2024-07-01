using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskClick : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Click()
    {
        if (player.inSchool && !player.inClass)
        {
            player.timeSpeed = new System.TimeSpan(0, 5, 0);
        }
    }
}
