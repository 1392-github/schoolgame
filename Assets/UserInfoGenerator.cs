using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UserInfoGenerator : MonoBehaviour
{
    public GameObject userInfo;
    public Transform list;
    public List<string> modeName;
    public List<Color> gradeColor;
    public List<string> dispName;
    public List<string> unitName;
    public GameObject error;
    public GameObject error2;
    public Text userName;
    void Start()
    {
        GameObject uds = GameObject.Find("UserDataSender");
        string user = uds.GetComponent<UserDataSender>().user;
        Destroy(uds);
        userName.text = user;
        List<UserInfo> data = Util.SendJSON2<Wrap<List<UserInfo>>>($"user/{user}")?.value;
        if (data == null)
        {
            error.SetActive(true);
            return;
        }
        if (data.Count == 0)
        {
            error2.SetActive(true);
            return;
        }
        for (int i = 0; i < 6; i++)
        {
            UserInfo d = data[i];
            Transform t = Instantiate(userInfo, list).transform;
            t.Find("Mode").GetComponent<Text>().text = modeName[i];
            t.Find("RatingDisplay").GetComponent<Text>().text = d.rating.ToString();
            Text te = t.Find("RatingRankDisplay").GetComponent<Text>();
            int ratingRank = Mathf.Clamp(9 - d.rating / 5, 1, 9);
            te.text = ratingRank.ToString();
            te.color = gradeColor[ratingRank - 1];
            Transform ratingBarInner = t.Find("RatingBar").Find("RatingBarInner");
            ratingBarInner.GetComponent<RectTransform>().anchorMax = new Vector2(d.rating / 50f, 1);
            ratingBarInner.GetComponent<Image>().color = gradeColor[ratingRank - 1];
            if (i >= 4) // 일 단위 제거 (1년/3년)
            {
                for (int j = 0; j < 4; j++)
                {
                    Destroy(t.Find(unitName[0] + dispName[j]).gameObject);
                    Destroy(t.Find($"Text (Legacy) ({2 + j * 4})").gameObject);
                }
                for (int j = 1; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        t.Find(unitName[j] + dispName[k]).GetComponent<RectTransform>().anchorMin = new Vector2((j - 1) / 3f, 1);
                        t.Find($"Text (Legacy) ({2 + k * 4 + j})").GetComponent<RectTransform>().anchorMin = new Vector2((j - 1) / 3f, 1);
                    }
                }
            }
            if (i == 5) // 월 단위를 사분기 단위로 변경
            {
                for (int j = 0; j < 4; j++)
                {
                    Text tmp = t.Find($"Text (Legacy) ({3 + j * 4})").GetComponent<Text>();
                    tmp.text = tmp.text.Replace("월", "사분기");
                }
            }
            if (i == 2 || i == 3) // 일 단위를 주 단위로 변경
            {
                for (int j = 0; j < 4; j++)
                {
                    Text tmp = t.Find($"Text (Legacy) ({2 + j * 4})").GetComponent<Text>();
                    tmp.text = tmp.text.Replace("일", "주");
                }
            }
            Text grdText;
            int grd;
            t.Find("DailyScoreDisplay").GetComponent<Text>().text = d.dayScore == -1 ? "---" : d.dayScore.ToString();
            t.Find("DailyRankDisplay").GetComponent<Text>().text = d.dayRank == -1 ? "---" : d.dayRank.ToString();
            t.Find("DailyPercentDisplay").GetComponent<Text>().text = d.dayPercent == -1 ? "---" : (d.dayPercent * 100).ToString("0.####") + "%";
            grdText = t.Find("DailyGradeDisplay").GetComponent<Text>();
            if (d.dayPercent == -1)
            {
                grd = 9;
            }
            else if (d.dayPercent <= 0.00390625f)
            {
                grd = 1;
            }
            else
            {
                grd = Mathf.CeilToInt(Mathf.Log(d.dayPercent, 2)) + 9;
            }
            grdText.text = grd.ToString();
            grdText.color = gradeColor[grd - 1];
            t.Find("MonthlyScoreDisplay").GetComponent<Text>().text = d.monthScore == -1 ? "---" : d.monthScore.ToString();
            t.Find("MonthlyRankDisplay").GetComponent<Text>().text = d.monthRank == -1 ? "---" : d.monthRank.ToString();
            t.Find("MonthlyPercentDisplay").GetComponent<Text>().text = d.monthPercent == -1 ? "---" : (d.monthPercent * 100).ToString("0.####") + "%";
            grdText = t.Find("MonthlyGradeDisplay").GetComponent<Text>();
            if (d.monthPercent == -1)
            {
                grd = 9;
            }
            else if (d.monthPercent <= 0.00390625f)
            {
                grd = 1;
            }
            else
            {
                grd = Mathf.CeilToInt(Mathf.Log(d.monthPercent, 2)) + 9;
            }
            grdText.text = grd.ToString();
            grdText.color = gradeColor[grd - 1];
            t.Find("YearlyScoreDisplay").GetComponent<Text>().text = d.yearScore == -1 ? "---" : d.yearScore.ToString();
            t.Find("YearlyRankDisplay").GetComponent<Text>().text = d.yearRank == -1 ? "---" : d.yearRank.ToString();
            t.Find("YearlyPercentDisplay").GetComponent<Text>().text = d.yearPercent == -1 ? "---" : (d.yearPercent * 100).ToString("0.####") + "%";
            grdText = t.Find("YearlyGradeDisplay").GetComponent<Text>();
            if (d.yearPercent == -1)
            {
                grd = 9;
            }
            else if (d.yearPercent <= 0.00390625f)
            {
                grd = 1;
            }
            else
            {
                grd = Mathf.CeilToInt(Mathf.Log(d.yearPercent, 2)) + 9;
            }
            grdText.text = grd.ToString();
            grdText.color = gradeColor[grd - 1];
            t.Find("AllScoreDisplay").GetComponent<Text>().text = d.allScore == -1 ? "---" : d.allScore.ToString();
            t.Find("AllRankDisplay").GetComponent<Text>().text = d.allRank == -1 ? "---" : d.allRank.ToString();
            t.Find("AllPercentDisplay").GetComponent<Text>().text = d.allPercent == -1 ? "---" : (d.allPercent * 100).ToString("0.####") + "%";
            grdText = t.Find("AllGradeDisplay").GetComponent<Text>();
            if (d.allPercent == -1)
            {
                grd = 9;
            }
            else if (d.allPercent <= 0.00390625f)
            {
                grd = 1;
            }
            else
            {
                grd = Mathf.CeilToInt(Mathf.Log(d.allPercent, 2)) + 9;
            }
            grdText.text = grd.ToString();
            grdText.color = gradeColor[grd - 1];
        }
    }
}
