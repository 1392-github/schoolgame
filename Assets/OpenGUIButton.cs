using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGUIButton : MonoBehaviour
{
    Player player;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void Click()
    {
        if (target.name == "ScoreBoard")
        {
            player.sbindex = player.scores.Count - 1;
        }
        target.SetActive(true);
    }
}
