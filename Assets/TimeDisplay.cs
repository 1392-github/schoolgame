using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = player.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(player.time - new System.DateTime(2024, 3, 3, 8, 0, 0)).TotalDays}ÀÏÂ÷");
    }
}
