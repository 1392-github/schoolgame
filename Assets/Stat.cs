using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    Player player;
    Text text;
    string TimeSpanToString(TimeSpan t) => $"{t.Days}일 {t.Hours}시간 {t.Minutes}분 {t.Seconds}초";
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        string examStat = "";
        for (int i = 7; i >= 0; i--)
        {
            examStat += $"{i + 1}등급 이상 처음 날짜 : {player.firstGrade[i]}\n";
        }
        for (int i = 7; i >= 0; i--)
        {
            examStat += $"{i + 1}등급 이상 연속 횟수 : {player.repeatGrade[i]}번 (최대 {player.repeatGradeMax[i]}번)\n";
        }
        text.text = $@"시작 시간 : {player.startTime:yyyy년 M월 d일 H시 m분 s초}
전체 플레이 타임 : {TimeSpanToString(DateTime.Now - player.startTime)}
순수 플레이 타임 : {TimeSpanToString(player.totalPlayTime)}
모든 능력치 합 : {player.studyExp.Sum()}
{examStat}";
    }
}
