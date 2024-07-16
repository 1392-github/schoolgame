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
        if (player.length == 0)
        {
            GetComponent<Text>().text = player.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(player.time - new System.DateTime(2024, 3, 3, 8, 0, 0)).TotalDays}老瞒");
        }
        else if (player.end)
        {
            GetComponent<Text>().text = player.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(player.time - new System.DateTime(2024, 3, 3, 8, 0, 0)).TotalDays}老瞒 (En\\d)");
        }
        else if (player.time.Date == player.endTime && player.time.TimeOfDay >= new System.TimeSpan(8, 0, 0))
        {
            GetComponent<Text>().text = player.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(player.time - new System.DateTime(2024, 3, 3, 8, 0, 0)).TotalDays}老瞒 (D-Da\\y)");
        }
        else
        {
            GetComponent<Text>().text = player.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(player.time - new System.DateTime(2024, 3, 3, 8, 0, 0)).TotalDays}老瞒 (D-{(int)(player.endTime - player.time.Date).TotalDays + (player.time.TimeOfDay < new System.TimeSpan(8, 0, 0) ? 1 : 0)})");
        }
    }
}
