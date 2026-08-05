using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    SaveBuffer sb;
    public SaveFile4 tutorialDefaultSave;

    public Items items;
    public Stats stats;
    void Start()
    {
        sb = GameObject.Find("SaveData").GetComponent<SaveBuffer>();
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
        if (!GameData.init)
        {
            GameData.init = true;
            GameData.items = items.items;
            for (int i = 0; i < 50; i++)
            {
                int i2 = i;
                GameData.items[i].descExt = () => ItemScripts.Item1Desc(i2 % 10 + 1);
                GameData.items[i].use = () => ItemScripts.UseItem1(i2);
            }
            GameData.statTypes = stats.stats;
            GameData.statTypes[0].onUpgrade = StatOnUpgradeScripts.OnStudyUpgrade;
            GameData.statTypes[4].onUpgrade = StatOnUpgradeScripts.OnQuestMaxUpgrade;
            GameData.statTypes[5].onUpgrade = StatOnUpgradeScripts.OnQuestTimeUpgrade;
        }
    }
    public void Click(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/@Á¶¿îÇõ-c7n");
    }
    public void OfficalSite()
    {
        Application.OpenURL("https://1392year.pythonanywhere.com/w/ÇÐ±³3");
    }
    public void Tutorial()
    {
        sb.tutorial = true;
        tutorialDefaultSave.startTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //sb.save = tutorialDefaultSave;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GlobalScene");
    }
    #if UNITY_ANDROID
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endif
}
