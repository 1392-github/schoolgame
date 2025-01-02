using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction2
{
    left,
    right,
    up,
    down
}
public class Door : MonoBehaviour
{
    public string map;
    public int args;
    public Vector3 pos;
    public bool enable = true;
    public int destDoorID = -1;
    public Direction2 direction;
    public int doorID;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enable && collision.gameObject.name == "Player")
        {
            player.Move(map, args, pos, destDoorID, direction);
        }
    }
}
