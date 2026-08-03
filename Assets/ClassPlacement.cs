using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClassPlacement : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void OnSubmit(string text)
    {
        GameData.clas = Enumerable.Repeat(-1, 1000).ToArray();
        List<int> students = new List<int>();
        if (text != "")
        {
            foreach (string s in text.Split(' '))
            {
                if (int.TryParse(s, out int a))
                {
                    if (a <= 0 || a >= 330)
                    {
                        player.OpenDialog("1~329 사이에서 입력하세요\n(예 : 123 가능, 456 불가능)");
                        return;
                    }
                    if (students.Contains(a))
                    {
                        player.OpenDialog("같은 숫자를 여러 개 입력하지 마세요");
                        return;
                    }
                    students.Add(a);
                }
                else
                {
                    player.OpenDialog("숫자와 공백만 입력하세요\n(예 : 1 2 3 가능, a b c 불가능)");
                    return;
                }
            }
            if (students.Count > 32)
            {
                player.OpenDialog("최대 32개까지만 입력하세요");
                return;
            }
        }
        GameData.clas[0] = Random.Range(0, 10);
        foreach (int s in students)
        {
            if (Random.Range(0, 100) < player.classPlacementChance)
            {
                GameData.clas[s] = GameData.clas[0];
            }
            else
            {
                GameData.clas[s] = Random.Range(0, 9);
                if (GameData.clas[s] == GameData.clas[0])
                {
                    GameData.clas[s] = 9;
                }
            }
        }
        for (int i = 1; i < 330; i++)
        {
            if (students.Contains(i))
            {
                continue;
            }
            do
            {
                GameData.clas[i] = Random.Range(0, 10);
            } while (GameData.clas.Count(c => c == GameData.clas[i]) > 33);
        }
        for (int i = 330; i < 660; i++)
        {
            do
            {
                GameData.clas[i] = Random.Range(10, 20);
            } while (GameData.clas.Count(c => c == GameData.clas[i]) > 33);
        }
        for (int i = 660; i < 1000; i++)
        {
            do
            {
                GameData.clas[i] = Random.Range(20, 30);
            } while (GameData.clas.Count(c => c == GameData.clas[i]) > 34);
        }
        GameData.timeSpeed = new System.TimeSpan(0, 0, 30);
        transform.parent.gameObject.SetActive(false);
    }
}
