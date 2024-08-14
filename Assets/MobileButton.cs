using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileButton : MonoBehaviour
{
    Player player;
    public KeyCode key;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Click()
    {
        player.currentPressingButton.Add(key);
    }
}
