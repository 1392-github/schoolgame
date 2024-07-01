using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetClass2 : MonoBehaviour
{
    Player player;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        text = GetComponent<Text>();
    }

    public void OnValueChanged(string t)
    {
        if (int.TryParse(t, out int v))
        {
            if (v < 1000 && v >= 0)
            {
                int grade;
                if (v < 330)
                {
                    grade = -1;
                }
                else if (v < 660)
                {
                    grade = 9;
                }
                else
                {
                    grade = 19;
                }
                text.text = $"{player.clas[v] - grade}¹Ý";
            }
        }
    }
}
