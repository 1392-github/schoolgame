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
        if (player.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            door.destDoorID += player.mapArgs;
        }
        else
        {
            door.destDoorID = -1;
        }
        door.args = player.mapArgs / 10;
        door.pos = new Vector3(1 + player.mapArgs % 10 * 3, 0.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        door.enable = !player.inClass;
    }
}
