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
        if (GameData.length == 0)
        {
            GetComponent<Text>().text = GameData.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(GameData.time - player.firstDay).TotalDays + 1}老瞒");
        }
        else if (GameData.end)
        {
            GetComponent<Text>().text = GameData.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(GameData.time - player.firstDay).TotalDays + 1}老瞒 (En\\d)");
        }
        else if (GameData.time.Date == player.endTime && GameData.time.TimeOfDay >= new System.TimeSpan(8, 0, 0))
        {
            GetComponent<Text>().text = GameData.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(GameData.time - player.firstDay).TotalDays + 1}老瞒 (D-Da\\y)");
        }
        else
        {
            GetComponent<Text>().text = GameData.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(GameData.time - player.firstDay).TotalDays + 1}老瞒 (D-{(int)(player.endTime - GameData.time.Date).TotalDays + (GameData.time.TimeOfDay < new System.TimeSpan(8, 0, 0) ? 1 : 0)})");
        }
    }
}
