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
