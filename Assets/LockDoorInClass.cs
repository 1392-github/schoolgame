using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorInClass : MonoBehaviour
{
    Player player;
    Door door;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        door = GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        door.enable = !player.inClass;
    }
}
