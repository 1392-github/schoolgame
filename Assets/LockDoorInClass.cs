using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorInClass : MonoBehaviour
{
    Door door;
    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Door>();
        if (GameData.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            door.destDoorID += GameData.mapArgs;
        }
        else
        {
            door.destDoorID = -1;
        }
        door.args = GameData.mapArgs / 10;
        door.pos = new Vector3(1 + GameData.mapArgs % 10 * 3, 0.8f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        door.enable = !GameData.inClass;
    }
}
