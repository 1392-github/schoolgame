using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI ddayText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] HomeGameManager gameManager;
    [SerializeField] GameObject dialog;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject upgradeButton;
    [SerializeField] Transform upgradeScroll;
    [SerializeField] UpgradePreview upgradePreview;
    [SerializeField] TextMeshProUGUI xpDisplay;
    public Button nextDayButton;
    bool nextDayButtonPrevent;
    bool nextDayButtonPrevent2;
    // Start is called before the first frame update
    void Start()
    {
        //GameData.Save();
        nameText.text = $"{GameData.school}\n{GameData.name}";
        UpdateTimeUI();
        if (Input.GetMouseButton(0)) nextDayButtonPrevent = true;
        ItemScripts.uiManager = this;
        GameData.uiManager = this;
        for (int i = 0; i < GameData.statTypes.Count; i++)
        {
            if (GameData.statTypes[i].experimental != Experimental.NONE && !GameData.ExperimentalCheck(GameData.statTypes[i].experimental))
            {
                continue;
            }
            GameObject g = Instantiate(upgradeButton);
            g.transform.SetParent(upgradeScroll, false);
            StatUpgrade u = g.GetComponent<StatUpgrade>();
            u.id = i;
            u.uiManager = this;
            u.upgradePreview = upgradePreview;
            u.Start2();
            u.UpdateText();
        }
        UpdateXPDisplay();
    }
    void Update()
    {
        if (nextDayButtonPrevent2) nextDayButtonPrevent = false;
        if (nextDayButtonPrevent && Input.GetMouseButtonUp(0)) nextDayButtonPrevent2 = true; // 손 떼고 바로 다음 프레임에 prevent 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameData.Save();
            SceneManager.LoadScene("TitleScene");
        }
    }
    public void UpdateTimeUI()
    {
        timeText.text = $"<size=70>1</size>회차 <size=70>{GameData.grade}</size>학년 <size=70>{GameData.semester}</size>학기 {GameData.time:yyyy-MM-dd(ddd)\nHH:mm:ss}";
        ddayText.text = $"{(GameData.grade == 3 ? "졸업까지" : $"{GameData.grade + 1}학년 진급까지")}\n<size=70>D-{(int)Math.Ceiling((new DateTime(GameData.startYear + GameData.grade, 1, 1) - GameData.time + new TimeSpan(8, 0, 0)).TotalDays)}</size>";
        nextDayButton.interactable = !GameData.inSchool;
    }
    public void ExitHome()
    {
        GameData.currentScene = "Hub";
        GameData.mapArgs = 0;
        GameData.x = -4.5f;
        GameData.y = 1.5f;
        SceneManager.LoadScene("GlobalScene");
    }
    public void NextDayButton()
    {
        if (nextDayButtonPrevent) return; // 모바일 버전에서 이동키 누르다 집 들어가졌을 때 다음날로 눌리는 것 방지
        GameData.time = GameData.time.Date + new TimeSpan(GameData.time.Hour >= 8 ? 1 : 0, 8, 0, 0);
        UpdateTimeUI();
        gameManager.StartDay();
    }
    public void OpenDialog(string text)
    {
        dialog.SetActive(true);
        dialog.transform.SetAsLastSibling();
        dialogText.text = text;
    }
    public void UpdateXPDisplay()
    {
        xpDisplay.text = $"{GameData.exp} XP";
    }
}
