using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScene : MonoBehaviour
{
    SaveBuffer sb;
    public SaveFile4 tutorialDefaultSave;
    public Transform noticeList;
    public GameObject noticeItem;
    public GameObject notice;
    void Start()
    {
        sb = GameObject.Find("SaveData").GetComponent<SaveBuffer>();
        List<NameAndVal<string>> notice = Util.SendJSON2<Wrap<List<NameAndVal<string>>>>("notice")?.value;
        if (notice == null || notice.Count == 0)
        {
            return;
        }
        this.notice.SetActive(true);
        foreach (NameAndVal<string> n in notice)
        {
            Transform t = Instantiate(noticeItem, noticeList).transform;
            t.Find("Name").GetComponent<Text>().text = n.name;
            t.Find("Desc").GetComponent<Text>().text = n.value;
        }
    }
    public void Click(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/@user-vb9by5mp9f");
    }
    public void OfficalSite()
    {
        Application.OpenURL("https://1392year.pythonanywhere.com/w/ÇÐ±³3");
    }
    public void Tutorial()
    {
        sb.tutorial = true;
        tutorialDefaultSave.startTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sb.save = tutorialDefaultSave;
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
