using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorInSchool : MonoBehaviour
{
    Player player;
    Door door;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player.mapArgs != 0)
        {
            Destroy(this);
        }
        door = GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        door.enable = !player.inSchool;
    }
}
