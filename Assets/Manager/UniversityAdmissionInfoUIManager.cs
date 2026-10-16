using System.Linq;
using UnityEngine;
using TMPro;

public class UniversityAdmissionInfoUIManager : MonoBehaviour
{
    public static string universityName;
    public static int registerFee;
    public static UniversityAdmissionInfo info;

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI[] weightTexts;
    [SerializeField] TextMeshProUGUI suneungMinText;
    [SerializeField] TextMeshProUGUI earlyApplicationDateText;
    [SerializeField] TextMeshProUGUI regularGroupText;
    [SerializeField] TextMeshProUGUI registerFeeText;
    [SerializeField] TextMeshProUGUI[] chanceTexts;
    void Start()
    {
        titleText.text = universityName + "대학교\n모 집 요 강";
        // 가중치 표시
        for (int i = 0; i < 5; i++)
        {
            weightTexts[i].text = info.weight[i].ToString();
        }
        weightTexts[5].text = info.weight.Sum().ToString();
        if (info.suneungMinSubjectCount == 0)
        {
            suneungMinText.text = "없음";
        }
        else
        {
            suneungMinText.text = $"{info.suneungMinSubjectCount}개 영역 합 {info.suneungMinGradeSum}";
        }
        earlyApplicationDateText.text = $"{info.earlyApplicationStartDay:MM월 dd일} ~ {info.earlyApplicationEndDay2:MM월 dd일}";
        regularGroupText.text = UniversityManager.regularGroupNames[info.regularGroup] + "군";
        registerFeeText.text = registerFee.ToString("N0") + "원";
        for (int i = 0; i < 20; i++)
        {
            int chance = info.chance[i];
            chanceTexts[i].text = chance.ToString() + "%";
            if (chance >= 80)
            {
                chanceTexts[i].color = Color.green;
            }
            else if (chance >= 60)
            {
                chanceTexts[i].color = new Color32(128, 255, 0, 255);
            }
            else if (chance >= 40)
            {
                chanceTexts[i].color = Color.yellow;
            }
            else if (chance >= 20)
            {
                chanceTexts[i].color = new Color32(255, 128, 0, 255);
            }
            else if (chance >= 1)
            {
                chanceTexts[i].color = Color.red;
            }
            else
            {
                chanceTexts[i].color = Color.black;
            }
        }
    }
}
