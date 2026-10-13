using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UniversityUIManager : MonoBehaviour
{
    [SerializeField] Transform universityList;
    [SerializeField] GameObject universityListItemPrefab;
    [SerializeField] TextMeshProUGUI applicationTitleText;
    [SerializeField] TextMeshProUGUI applicationInfoSuneungMinOrGroupText;
    [SerializeField] Transform applicationList;
    [SerializeField] GameObject applicationListPrefab;
    [SerializeField] TextMeshProUGUI[] applicationedGroupTexts;
    [SerializeField] Color[] chanceColor;
    [SerializeField] GameObject cannotApply;
    [SerializeField] TextMeshProUGUI cannotApplyReasonText;
    [SerializeField] GameObject applicationConfirm;
    [SerializeField] TextMeshProUGUI applicationConfirmText;
    [SerializeField] GameObject applicationFailed;
    [SerializeField] TextMeshProUGUI applicationFailedText;
    [SerializeField] Image[] applicatedUniversitiesBackground;
    [SerializeField] TextMeshProUGUI[] applicatedUniversitiesText;
    [SerializeField] TextMeshProUGUI allFailChanceText;

    AdmissionType admissionType;
    int selectedUniv;
    Color GetChanceColor(int chance)
    {
        if (chance >= 80)
        {
            return chanceColor[5];
        }
        else if (chance >= 60)
        {
            return chanceColor[4];
        }
        else if (chance >= 40)
        {
            return chanceColor[3];
        }
        else if (chance >= 20)
        {
            return chanceColor[2];
        }
        else if (chance >= 1)
        {
            return chanceColor[1];
        }
        else
        {
            return chanceColor[0];
        }
    }
    public void UpdateAdmissionList()
    {
        if (UniversityManager.admissionType == AdmissionType.NONE)
        {
            return;
        }
        int allFailChance = 1;
        for (int i = 0; i < 3; i++)
        {
            int univ = UniversityManager.appliedUniversities[i];
            if (univ == -1)
            {
                allFailChance *= 100;
                continue;
            }
            int score = UniversityManager.GetAdmissionScore(UniversityManager.admissionType, univ);
            int chance = UniversityManager.GetChance(univ, score);
            allFailChance *= 100 - chance;
            applicatedUniversitiesText[i].text = $"{UniversityManager.universities[univ].name}대학교\n\n전형 점수: {score}\n합격 확률: <color=#{ColorUtility.ToHtmlStringRGB(GetChanceColor(chance))}>{chance}%</color>";
            applicatedUniversitiesBackground[i].color = Color.white;
        }
        allFailChanceText.text = $"모두 떨어질 확률: {allFailChance / 10000f:F2}%";
    }
    // Start is called before the first frame update
    void Start()
    {
        admissionType = Util.InGameDate(GameData.time) < UniversityManager.earlyApplicationEnd ? AdmissionType.EARLY_ADMISSION : AdmissionType.REGULAR_ADMISSION;
        // 대학교 목록 생성
        for (int i = 0; i < UniversityManager.universities.Length; i++)
        {
            University university = UniversityManager.universities[i];
            UniversityListItem item = Instantiate(universityListItemPrefab, universityList).GetComponent<UniversityListItem>();
            item.universityName.text = university.name + "대학교";
            int score = UniversityManager.GetAdmissionScore(AdmissionType.EARLY_ADMISSION, i);
            item.scoreEarly.text = score.ToString();
            int chance = UniversityManager.GetChance(i, score);
            item.chanceEarly.text = chance.ToString() + "%";
            item.chanceEarly.color = GetChanceColor(chance);
            if (!UniversityManager.IsAdmissionScoreConfirmed(AdmissionType.EARLY_ADMISSION))
            {
                item.scoreEarly.fontStyle = FontStyles.Italic;
                item.chanceEarly.fontStyle = FontStyles.Italic;
            }
            score = UniversityManager.GetAdmissionScore(AdmissionType.REGULAR_ADMISSION, i);
            item.scoreRegular.text = score.ToString();
            chance = UniversityManager.GetChance(i, score);
            item.chanceRegular.text = chance.ToString() + "%";
            item.chanceRegular.color = GetChanceColor(chance);
            if (!UniversityManager.IsAdmissionScoreConfirmed(AdmissionType.REGULAR_ADMISSION))
            {
                item.scoreRegular.fontStyle = TMPro.FontStyles.Italic;
                item.chanceRegular.fontStyle = TMPro.FontStyles.Italic;
            }
            item.admissionCount.text = UniversityManager.universityCount[i].ToString();
            int i2 = i;
            item.button.onClick.AddListener(() =>
            {
                UniversityAdmissionInfoUIManager.universityName = UniversityManager.universities[i2].name;
                UniversityAdmissionInfoUIManager.registerFee = UniversityManager.universities[i2].registerFee;
                UniversityAdmissionInfoUIManager.info = UniversityManager.universityAdmissionInfo[i2];
                SceneManager.LoadScene("UniversityAdmissionInfo");
            });
        }
        applicationTitleText.text = $"{GameData.startYear + 3}학년도 {(admissionType == AdmissionType.EARLY_ADMISSION ? "수시" : "정시")} 지원 <size=50>전체 지원 기간: {UniversityManager.GetApplicationDate(admissionType, 0):yyyy년 M월 d일} ~ {UniversityManager.GetApplicationDate(admissionType, 2):M월 d일}</size>";
        if (admissionType == AdmissionType.REGULAR_ADMISSION)
        {
            // 정시 모집의 경우 수시 모집의 수능 최저 자리에 모집군을 표시
            applicationInfoSuneungMinOrGroupText.text = "모집군";
            if (UniversityManager.admissionType != AdmissionType.EARLY_ADMISSION) // 이미 수시를 지원한 경우는 제외하고
            {
                // 수시 모집의 지원 목록 1, 2, 3 자리에 가, 나, 다 표시
                applicationedGroupTexts[0].text = "가";
                applicationedGroupTexts[1].text = "나";
                applicationedGroupTexts[2].text = "다";
            }
        }
        for (int i = 0; i < UniversityManager.universities.Length; i++)
        {
            UniversityApplicationListItem item = Instantiate(applicationListPrefab, applicationList).GetComponent<UniversityApplicationListItem>();
            item.universityName.text = UniversityManager.universities[i].name + "대학교";
            UniversityAdmissionInfo info = UniversityManager.universityAdmissionInfo[i];
            if (admissionType == AdmissionType.EARLY_ADMISSION)
            {
                item.applicaitonDate.text = $"{info.earlyApplicationStartDay:M\\/d}~{info.earlyApplicationEndDay2:M\\/d}";
            }
            else
            {
                item.applicaitonDate.text = $"{UniversityManager.GetApplicationDate(AdmissionType.REGULAR_ADMISSION, 0):M\\/d}~{UniversityManager.GetApplicationDate(AdmissionType.REGULAR_ADMISSION, 2):M\\/d}";
            }
            int score = UniversityManager.GetAdmissionScore(admissionType, i);
            item.score.text = score.ToString();
            int chance = UniversityManager.GetChance(i, score);
            item.chance.text = chance.ToString() + "%";
            item.chance.color = GetChanceColor(chance);
            if (!UniversityManager.IsAdmissionScoreConfirmed(admissionType))
            {
                item.score.fontStyle = FontStyles.Italic;
                item.chance.fontStyle = FontStyles.Italic;
            }
            if (admissionType == AdmissionType.EARLY_ADMISSION)
            {
                if (info.suneungMinSubjectCount == 0)
                {
                    item.suneungMinOrGroup.text = "-";
                }
                else
                {
                    item.suneungMinOrGroup.text = $"{info.suneungMinSubjectCount}합 {info.suneungMinGradeSum}";
                }
            }
            else
            {
                item.suneungMinOrGroup.text = UniversityManager.regularGroupNames[info.regularGroup];
            }
            int i2 = i;
            item.apply.onClick.AddListener(() => ClickApplication(i2));
        }
        if (UniversityManager.admissionType == AdmissionType.EARLY_ADMISSION && admissionType == AdmissionType.REGULAR_ADMISSION) // 수시를 이미 지원했고 지금 정시 기간이면 (수시 납치를 당했다면)
        {
            cannotApply.SetActive(true);
            cannotApplyReasonText.text = "수시모집 지원자는\n정시모집 지원 불가";
        }
        else if (GameData.time < UniversityManager.GetApplicationDate(admissionType, 0)) // 지원 시작 전이라면
        {
            cannotApply.SetActive(true);
            cannotApplyReasonText.text = $"{GameData.startYear + 3}학년도 {(admissionType == AdmissionType.EARLY_ADMISSION ? "수시" : "정시")} 지원 기간\n{UniversityManager.GetApplicationDate(admissionType, 0):yyyy년 M월 d일} ~ {UniversityManager.GetApplicationDate(admissionType, 2):M월 d일}";
        }
        else if (GameData.time >= UniversityManager.regularApplicationEnd) // 정시 지원이 끝났다면
        {
            cannotApply.SetActive(true);
            cannotApplyReasonText.text = $"{GameData.startYear + 3}학년도 정시 지원 기간\n{UniversityManager.regularApplicationStart:yyyy년 M월 d일} ~ {UniversityManager.regularApplicationEnd2:M월 d일}";
            // 참고: 수시 지원만 끝났고 정시 지원이 안 끝났다면 정시 지원 창이 나옴
        }
        UpdateAdmissionList();
    }
    public void ClickApplication(int university)
    {
        void Fail(string message)
        {
            applicationFailedText.text = message;
            applicationConfirm.SetActive(false);
            applicationFailed.SetActive(true);
        }
        if (UniversityManager.appliedUniversityCount >= 3)
        {
            Fail("이미 3개를 지원했습니다.");
            return;
        }
        if (UniversityManager.appliedUniversities.Contains(university))
        {
            Fail("이미 지원한 대학입니다.");
            return;
        }
        int group = UniversityManager.universityAdmissionInfo[university].regularGroup;
        int groupUniv = UniversityManager.appliedUniversities[group];
        if (admissionType == AdmissionType.REGULAR_ADMISSION && groupUniv != -1)
        {
            Fail($"{UniversityManager.regularGroupNames[group]}군에 이미 {UniversityManager.universities[groupUniv].name}대학교를 지원했습니다.");
            return;
        }
        if (admissionType == AdmissionType.EARLY_ADMISSION && (GameData.time < UniversityManager.universityAdmissionInfo[university].earlyApplicationStartDay || GameData.time >= UniversityManager.universityAdmissionInfo[university].earlyApplicationEndDay))
        {
            Fail("해당 대학교의 수시 지원 기간이 아닙니다.");
            return;
        }
        if (GameData.money < 30000)
        {
            Fail("전형료(30,000원)가 부족합니다.");
            return;
        }
        int score = UniversityManager.GetAdmissionScore(admissionType, university);
        int chance = UniversityManager.GetChance(university, score);
        selectedUniv = university;
        applicationConfirmText.text = $"\"{UniversityManager.universities[university].name}대학교\"에 지원하겠습니까?\n\n전형 점수: {score}\n합격 확률: <color=#{ColorUtility.ToHtmlStringRGB(GetChanceColor(chance))}>{chance}%</color>";
        applicationFailed.SetActive(false);
        applicationConfirm.SetActive(true);
    }
    public void ClickApplicationOK()
    {
        UniversityManager.admissionType = admissionType;
        if (admissionType == AdmissionType.EARLY_ADMISSION)
        {
            UniversityManager.appliedUniversities[UniversityManager.appliedUniversityCount] = selectedUniv;
        }
        else
        {
            UniversityManager.appliedUniversities[UniversityManager.universityAdmissionInfo[selectedUniv].regularGroup] = selectedUniv;
        }
        GameData.money -= 30000;
        UniversityManager.appliedUniversityCount++;
        applicationConfirm.SetActive(false);
        UpdateAdmissionList();
    }
}
