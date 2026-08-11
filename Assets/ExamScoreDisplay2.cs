using System.Text;
using UnityEngine;
using TMPro;

public class ExamScoreDisplay2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI examName;
    [SerializeField] TextMeshProUGUI[] rawScore;
    [SerializeField] TextMeshProUGUI[] standardScore;
    [SerializeField] TextMeshProUGUI[] percentile;
    [SerializeField] TextMeshProUGUI[] grade;
    [SerializeField] HoverText[] gradeCut;
    [SerializeField] TextMeshProUGUI[] rank;
    [SerializeField] TextMeshProUGUI[] average;
    [SerializeField] TextMeshProUGUI[] deviation;
    void Start()
    {
        examName.text = $"{ExamManager.GetExamName(ExamManager.openExamType, ExamManager.openExam)}\n성적 통지표";
        ExamScore score = ExamManager.GetExamScore(ExamManager.openExamType, ExamManager.openExam);
        for (int i = 0; i < 5; i++)
        {
            rawScore[i].text = score.rawScore[i].ToString();
            standardScore[i].text = score.standardScore[i].ToString();
            percentile[i].text = score.percentile[i].ToString();
            grade[i].text = score.grade[i].ToString();
            StringBuilder stringBuilder = new StringBuilder($"이번 시험에서 {Util.subjectName[i]} 영역의 등급 구분점수는 다음과 같습니다.\n");
            for (int j = 0; j < 8; j++)
            {
                stringBuilder.AppendLine($"{j+1}등급 구분점수: {score.gradeCut[i*8+j]}점");
            }
            stringBuilder.Append("<color=grey>(등급 구분점수는 평균, 표준편차에 따라 매번 변동됩니다)</color>");
            gradeCut[i].text = stringBuilder.ToString();
            rank[i].text = $"{score.rank[i]}/300";
            average[i].text = score.average[i].ToString();
            deviation[i].text = score.deviation[i].ToString();
        }
    }
}
