using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDialog : MonoBehaviour
{
    [TextArea]
    public string text;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Click()
    {
        player.OpenDialog(text);
    }
}
