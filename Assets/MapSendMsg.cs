using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSendMsg : MonoBehaviour
{
    [TextArea]
    public string msg;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Send()
    {
        player.OpenDialog(msg);
    }
}
