using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetClass3 : MonoBehaviour
{
    Player player;
    InputField grade;
    InputField clas;
    Text rslt;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        grade = transform.Find("Grade").GetComponent<InputField>();
        clas = transform.Find("Class").GetComponent<InputField>();
        rslt = transform.Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>();
    }
    public void Get()
    {
        if (!int.TryParse(grade.text, out int g))
        {
            return;
        }
        if (!int.TryParse(clas.text, out int c))
        {
            return;
        }
        if (g < 1 || g > 3 || c < 1 || c > 10)
        {
            return;
        }
        int c2 = (g - 1) * 10 + c - 1;
        List<string> s = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            if (player.clas[i] == c2)
            {
                if (player.isFriendable(i))
                {
                    s.Add(i.ToString("D3"));
                }
                else
                {
                    s.Add($"<color=#808080>{i:D3}</color>");
                }
            }
        }
        rslt.text = string.Join('\n', s);
    }
}
