using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RankingGenerator : MonoBehaviour
{
    public List<Color> gradeColor;
    public int year;
    public int month;
    public int day;
    public int mode;
    public Dropdown yearD;
    public Dropdown monthD;
    public Dropdown dayD;
    public GameObject rankingItem;
    public Text stat;
    public Button statButton;
    public Text dayL;
    public Text reamingTimeDisplay;
    List<Dropdown.OptionData>[] dayDropDown;
    List<Dropdown.OptionData> monthDropDown;
    List<Dropdown.OptionData> quarterDropDown;
    DateTime today;
    DateTime? end;
    List<DateTime> weekEnd;
    bool ena = true;
    // Start is called before the first frame update
    void Start()
    {
        today = DateTime.Today;
        weekEnd = new List<DateTime>();
        List<Dropdown.OptionData> option = new List<Dropdown.OptionData>() { new Dropdown.OptionData("(전체)") };
        for (int i = 2024; i <= 2030; i++)
        {
            option.Add(new Dropdown.OptionData($"{i}년"));
        }
        yearD.options = option;
        monthDropDown = new List<Dropdown.OptionData>() { new Dropdown.OptionData("(전체)") };
        for (int i = 1; i <= 12; i++)
        {
            monthDropDown.Add(new Dropdown.OptionData($"{i}월"));
        }
        monthD.options = monthDropDown;
        quarterDropDown = new List<Dropdown.OptionData>() { new Dropdown.OptionData("(전체)") };
        for (int i = 1; i <= 4; i++)
        {
            quarterDropDown.Add(new Dropdown.OptionData($"{i}사분기 ({i*3-2}~{i*3}월)"));
        }
        dayDropDown = new List<Dropdown.OptionData>[4];
        for (int i = 0; i < 4; i++)
        {
            dayDropDown[i] = new List<Dropdown.OptionData>() { new Dropdown.OptionData("(전체)") };
            for (int j = 1; j <= 28 + i; j++)
            {
                dayDropDown[i].Add(new Dropdown.OptionData($"{j}일"));
            }
        }
        yearD.value = today.Year - 2023;
        monthD.value = today.Month;
        dayD.value = today.Day;
    }
    void Update()
    {
        if (end.HasValue)
        {
            TimeSpan r = (TimeSpan)(end - DateTime.Now);
            if (r < TimeSpan.Zero)
            {
                reamingTimeDisplay.text = "0일 0시간 0분 0초 000 남음";
            }
            else
            {
                reamingTimeDisplay.text = $"{(int)r.TotalDays}일 {r.Hours}시간 {r.Minutes}분 {r.Seconds}초 {r.Milliseconds:000} 남음";
            }
        }
        else
        {
            reamingTimeDisplay.text = "";
        }
    }
    public void Load()
    {
        List<Ranking> score = ((List<Ranking>)Util.SendJSON<Wrap<List<Ranking>>>("ranking", new GetRanking() { mode = mode, year = year, month = month, day = day })).OrderByDescending(c => c.score).ToList();
        stat.text = "";
        foreach (Transform item in transform)
        {
            if (item.gameObject.name != "Warning")
            {
                Destroy(item.gameObject);
            }
        }
        if (score.Count == 0)
        {
            statButton.interactable = false;
            return;
        }
        int[] gradeCut = Enumerable.Repeat(-1, 8).ToArray();
        int rank = 0;
        int rankSc = -1;
        for (int i = 0; i < score.Count; i++)
        {
            if (score[i].score != rankSc)
            {
                rank = i;
                rankSc = score[i].score;
            }
            float rate = (float)rank / score.Count;
            int grade;
            if (rate <= 0.00390625f)
            {
                grade = 1;
            }
            else
            {
                grade = Mathf.CeilToInt(Mathf.Log(rate, 2)) + 9;
            }
            Transform t = Instantiate(rankingItem, transform).transform;
            t.Find("Rank").GetComponent<Text>().text = (rank + 1).ToString();
            Text gt = t.Find("Grade").GetComponent<Text>();
            gt.text = grade.ToString();
            gt.color = gradeColor[grade - 1];
            t.Find("Percent").GetComponent<Text>().text = $"{rate * 100}%";
            t.Find("ID").GetComponent<Text>().text = score[i].name;
            t.Find("Score").GetComponent<Text>().text = score[i].score.ToString();
            if (grade != 9)
            {
                gradeCut[grade - 1] = score[i].score;
            }
        }
        for (int i = 1; i < 8; i++)
        {
            if (gradeCut[i] == -1)
            {
                gradeCut[i] = gradeCut[i - 1];
            }
        }
        for (int i = 0; i < 8; i++)
        {
            stat.text += $"{i + 1}등급컷 {gradeCut[i]}\n";
        }
        statButton.interactable = true;
    }
    public void ChangeYear(int i)
    {
        if (i == 0)
        {
            year = 0;
            monthD.value = 0;
            monthD.interactable = false;
        }
        else
        {
            if (ena && mode <= 3 && month != 0)
            {
                if (mode >= 2)
                {
                    ChangeWeek();
                }
                else
                {
                    dayD.options = dayDropDown[System.DateTime.DaysInMonth(year, month) - 28];
                }
            }
            monthD.interactable = true;
            year = i + 2023;
        }
        ChangeEnd();
    }
    public void ChangeMonth(int i)
    {
        if (i == 0)
        {
            month = 0;
            dayD.value = 0;
            dayD.interactable = false;
        }
        else
        {
            month = i;
            if (mode <= 3)
            {
                dayD.interactable = true;
                if (ena)
                {
                    if (mode <= 1)
                    {
                        dayD.options = dayDropDown[DateTime.DaysInMonth(year, month) - 28];
                    }
                    else if (mode <= 3)
                    {
                        ChangeWeek();
                    }
                }
            }
        }
        ChangeEnd();
    }
    public void ChangeDay(int i)
    {
        day = i;
        ChangeEnd();
    }
    /*
    0 : 1주 (일)
    1 : 1개월 (일)
    2 : 3개월 (주)
    3 : 6개월 (주)
    4 : 1년 (월)
    5 : 3년 (3월)
    */
    public void ChangeMode(int mode)
    {
        this.mode = mode;
        yearD.value = today.Year - 2023;
        if (mode == 5)
        {
            monthD.options = quarterDropDown;
            monthD.value = (today.Month + 2) / 3;
        }
        else
        {
            monthD.options = monthDropDown;
            monthD.value = today.Month;
        }
        if (mode <= 1)
        {
            dayD.options = dayDropDown[DateTime.DaysInMonth(year, month) - 28];
            dayD.interactable = true;
            dayL.fontSize = 50;
            dayD.value = today.Day;
        }
        else if (mode <= 3)
        {
            ChangeWeek(true);
            dayL.fontSize = 45;
            dayD.interactable = true;
        }
        else
        {
            dayD.interactable = false;
            dayD.value = 0;
        }
        ChangeEnd();
    }
    public void ChangeWeek(bool td = false)
    {
        ena = false;
        List<Dropdown.OptionData> week = new List<Dropdown.OptionData>();
        week.Add(new Dropdown.OptionData("(전체)"));
        int w = 1;
        DateTime a = new DateTime(year, month, 4);
        int b = (int)a.DayOfWeek;
        bool notfound = true;
        DateTime start = a - new TimeSpan(b == 0 ? 6 : b - 1, 0, 0, 0);
        weekEnd.Clear();
        if (td && start > today)
        {
            if (month == 1)
            {
                year--;
                yearD.value = year - 2023;
                monthD.value = 12;
            }
            else
            {
                monthD.value--;
            }
            ChangeWeek(true);
            return;
        }
        DateTime end = start + new TimeSpan(6, 0, 0, 0);
        while (true)
        {
            week.Add(new Dropdown.OptionData($"{w}주차 ({start:M-d}~{end:M-d})"));
            weekEnd.Add(end + DateTimeCalc2.day);
            if (td && today >= start && today <= end)
            {
                notfound = false;
                dayD.value = w;
            }
            start += new TimeSpan(7, 0, 0, 0);
            end += new TimeSpan(7, 0, 0, 0);
            if (end.Month != month && end.Day >= 4)
            {
                if (td && notfound)
                {
                    if (month == 12)
                    {
                        monthD.value = 1;
                        yearD.value = year - 2022;
                    }
                    else
                    {
                        monthD.value++;
                    }
                    ChangeWeek(true);
                    return;
                }
                break;
            }
            w++;
        }
        dayD.options = week;
        ena = true;
    }
    void ChangeEnd()
    {
        if (!ena)
        {
            return;
        }
        if (year == 0)
        {
            end = null;
        }
        else if (month == 0)
        {
            end = new DateTime(year + 1, 1, 1);
        }
        else if (day == 0)
        {
            if (mode == 2 || mode == 3)
            {
                end = weekEnd[^1];
            }
            else if (mode == 5)
            {
                end = new DateTime(month == 4 ? year + 1 : year, month == 4 ? 1 : month * 3 + 1, 1);
            }
            else
            {
                end = new DateTime(month == 12 ? year + 1 : year, month == 12 ? 1 : month + 1, 1);
            }
        }
        else
        {
            if (mode == 2 || mode == 3)
            {
                end = weekEnd[day - 1];
            }
            else
            {
                end = new DateTime(year, month, day) + DateTimeCalc2.day;
            }
        }
    }
    public void UserInfo(string name)
    {
        GameObject uda = GameObject.Find("UserDataSender");
        DontDestroyOnLoad(uda);
        uda.GetComponent<UserDataSender>().user = name;
        UnityEngine.SceneManagement.SceneManager.LoadScene("UserInfoScene");
    }
    public void DownloadRankingVersion()
    {
        Application.OpenURL("https://1392year.pythonanywhere.com/sch/downloadrankingversion");
    }
}
