using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchGenerator : MonoBehaviour
{
    public GameObject button;
    public GameObject list;
    public GameObject content;
    Player player;
    Data data;
    object[] para;
    // Start is called before the first frame update
    public void Start2()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        data = GameObject.Find("Data").GetComponent<Data>();
        para = new object[0];
        Tab tab = GetComponent<Tab>();
        Transform gtap = transform.Find("GradeTap");
        for (int i = 0; i < data.achievementGrade.Count; i++)
        {
            GameObject b = Instantiate(button);
            RectTransform brt = b.GetComponent<RectTransform>();
            brt.SetParent(gtap, false);
            brt.anchorMin = new Vector2((float)i / data.achievementGrade.Count, 0);
            brt.anchorMax = new Vector2((float)(i + 1) / data.achievementGrade.Count, 1);
            int i2 = i;
            b.GetComponent<Button>().onClick.AddListener(() => tab.TabButton(i2));
            b.transform.Find("Text (Legacy)").GetComponent<Text>().text = data.achievementGrade[i];
        }
        AchRegen();
    }
    public void AchRegen()
    {
        Tab tab = GetComponent<Tab>();
        foreach (GameObject o in tab.tabs)
        {
            Destroy(o);
        }
        tab.tabs.Clear();
        for (int i = 0; i < data.achievementGrade.Count; i++)
        {
            GameObject l = Instantiate(list);
            if (i != 0)
            {
                l.SetActive(false);
            }
            l.transform.SetParent(transform, false);
            tab.tabs.Add(l);
            List<Achievement> ach = new List<Achievement>();
            List<bool> achc = new List<bool>();
            for (int j = 0; j < data.achievement.Count; j++)
            {
                Achievement a = data.achievement[j];
                if (a.grade == i)
                {
                    ach.Add(a);
                    achc.Add(player.achCompleted[j]);
                }
            }
            Transform c = l.transform.Find("Viewport").Find("Content");
            for (int j = 0; j < ach.Count; j++)
            {
                Achievement a = ach[j];
                GameObject ac = Instantiate(content);
                ac.transform.SetParent(c, false);
                a.para.Invoke();
                string desc = string.Format(a.desc, para);
                ac.transform.Find("Name").GetComponent<Text>().text = achc[j] ? a.name + " (¿Ï·á)" : a.name;
                ac.transform.Find("Desc").GetComponent<Text>().text = desc;
                ac.transform.Find("Reward").GetComponent<Text>().text = a.exp.ToString();
            }
        }
    }
    public void AchRep1Para()
    {
        para = new object[] {player.repeatAll1};
    }
}
