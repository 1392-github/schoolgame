using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetClass1 : MonoBehaviour
{
    Text text;
    Player player;

    public void generate()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            text = GetComponent<Text>();
        }
        if (GameData.ExperimentalCheck(Experimental.FRIEND_SYSTEM))
        {
            text.text = "===== 1학년 =====\n";
            int grade = -1;
            for (int i = 0; i < 1000; i++)
            {
                if (i == 330)
                {
                    text.text += "===== 2학년 =====\n";
                    grade = 9;
                }
                if (i == 660)
                {
                    text.text += "===== 3학년 =====\n";
                    grade = 19;
                }
                if (player.isFriendable(i))
                {
                    text.text += $"{i:D3} {GameData.clas[i] - grade,2}반";
                }
                else
                {
                    text.text += $"<color=#808080>{i:D3} {GameData.clas[i] - grade,2}반</color>";
                }
                if (i != 999)
                {
                    text.text += "\n";
                }
            }
        }
        else
        {
            text.text = $"000 {GameData.clas[0]+1}반";
        }
    }
}
