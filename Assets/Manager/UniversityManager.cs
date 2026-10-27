using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 수시/정시 모집 구분
/// </summary>
public enum AdmissionType
{
    /// <summary>
    /// 지원 안 함 / 지원 전
    /// </summary>
    NONE,
    /// <summary>
    /// 수시모집
    /// </summary>
    EARLY_ADMISSION,
    /// <summary>
    /// 정시모집
    /// </summary>
    REGULAR_ADMISSION
}
[Serializable]
public class UniversityAdmissionInfo
{
    /// <summary>
    /// 과목별 가중치
    /// </summary>
    public int[] weight;
    /// <summary>
    /// 수능 최저의 a합 b에서 a값 (범위에 포함, 수능 최저 없으면 0)
    /// </summary>
    public int suneungMinSubjectCount;
    /// <summary>
    /// 수능 최저의 a합 b에서 b값 (범위에서 제외, 수능 최저 없으면 0)
    /// </summary>
    public int suneungMinGradeSum;
    /// <summary>
    /// 수시 지원의 시작일 (범위에 포함, 전체 시작일로부터의 일수, 첫날이 0)
    /// </summary>
    public int earlyApplicationStart;
    /// <summary>
    /// 수시 지원의 마감일 (범위에서 제외, 전체 시작일로부터의 일수, 첫날이 0)
    /// </summary>
    public int earlyApplicationEnd;
    /// <summary>
    /// earlyApplicationStart를 DateTime으로 변환
    /// </summary>
    public DateTime earlyApplicationStartDay => UniversityManager.earlyApplicationStart.AddDays(earlyApplicationStart);
    /// <summary>
    /// earlyApplicationEnd를 DateTime으로 변환 (범위에서 제외)
    /// </summary>
    public DateTime earlyApplicationEndDay => UniversityManager.earlyApplicationStart.AddDays(earlyApplicationEnd);
    /// <summary>
    /// earlyApplicationEnd를 DateTime으로 변환 (범위에 포함)
    /// </summary>
    public DateTime earlyApplicationEndDay2 => UniversityManager.earlyApplicationStart.AddDays(earlyApplicationEnd - 1);
    /// <summary>
    /// 정시 지원의 군 (0: 가군, 1: 나군, 2: 다군)
    /// </summary>
    public int regularGroup;
    /// <summary>
    /// 전형점수별 확률 (전형점수가 s라고 할 때 s / 50이 인덱스이며 1000일 경우 항상 100임, 배열 길이는 20임)
    /// </summary>
    public int[] chance;
}
public static class UniversityManager
{
    /// <summary>
    /// 대학교들
    /// </summary>
    public static University[] universities;
    /// <summary>
    /// 대학교 입학 정보
    /// </summary>
    public static UniversityAdmissionInfo[] universityAdmissionInfo;
    /// <summary>
    /// 대학교별 입학한 횟수
    /// </summary>
    public static int[] universityCount;
    /// <summary>
    /// 지원한 전형의 종류 (지원하기 전에는 NONE)
    /// </summary>
    public static AdmissionType admissionType;
    /// <summary>
    /// 지원한 대학의 목록 (수시 모집일 경우 지원한 순서대로 [0]부터 시작해서 [2]까지 채우고, 정시 모집일 경우 [0]은 가군, [1]은 나군, [2]는 다군)
    /// </summary>
    public static int[] appliedUniversities;
    /// <summary>
    /// 지원한 대학의 수 (지원 전에는 0)
    /// </summary>
    public static int appliedUniversityCount;
    /// <summary>
    /// 지원한 대학의 합격 여부 (true: 합격, false: 불합격, 지원 전에는 전부 false임)
    /// </summary>
    public static bool[] applicationPassed;
    /// <summary>
    /// 정시의 군 이름들 (뒤의 "군" 글자 제외)
    /// </summary>
    public static readonly string[] regularGroupNames = {"가", "나", "다"};
    /// <summary>
    /// 수시 지원의 전체 시작일
    /// </summary>
    public static DateTime earlyApplicationStart;
    /// <summary>
    /// 수시 지원의 전체 종료일(범위에 제외)
    /// </summary>
    public static DateTime earlyApplicationEnd;
    /// <summary>
    /// <summary>
    /// 수시 지원의 전체 종료일(범위에서 포함)
    /// </summary>
    public static DateTime earlyApplicationEnd2;
    /// <summary>
    /// 정시 지원의 전체 시작일
    /// </summary>
    public static DateTime regularApplicationStart;
    /// <summary>
    /// 정시 지원의 전체 종료일(범위에 제외)
    /// </summary>
    public static DateTime regularApplicationEnd;
    /// <summary>
    /// 정시 지원의 전체 종료일(범위에서 포함)
    /// </summary>
    public static DateTime regularApplicationEnd2;
    /// <summary>
    /// 합격자 발표일
    /// </summary>
    public static DateTime announcementDate;
    /// <summary>
    /// 합격자를 이미 봤는지
    /// </summary>
    public static bool announcementOpened;
    /// <summary>
    /// 최종 등록한 대학 (-1이면 없음)
    /// </summary>
    public static int finalUniversity = -1;
    /// <summary>
    /// 모집요강이 해금되었는지
    /// </summary>
    public static bool admissionInfoUnlocked => GameData.grade == 3;
    static readonly int[] nearMonday = {1, 0, 6, 5, 4, 3, 2};
    public static void Init()
    {
        Array.Resize(ref universityCount, universities.Length);
        if (appliedUniversities.Length == 0) appliedUniversities = new int[] {-1, -1, -1};
        if (applicationPassed.Length == 0) applicationPassed = new bool[3];
        earlyApplicationStart = new DateTime(GameData.startYear + 2, 9, 1);
        earlyApplicationStart = earlyApplicationStart.AddDays(nearMonday[(int)earlyApplicationStart.DayOfWeek]);
        earlyApplicationEnd = earlyApplicationStart.AddDays(5);
        earlyApplicationEnd2 = earlyApplicationStart.AddDays(4);
        regularApplicationStart = new DateTime(GameData.startYear + 2, 12, 8);
        regularApplicationStart = regularApplicationStart.AddDays(nearMonday[(int)regularApplicationStart.DayOfWeek]);
        regularApplicationEnd = regularApplicationStart.AddDays(5);
        regularApplicationEnd2 = regularApplicationStart.AddDays(4);
        announcementDate = new DateTime(GameData.startYear + 2, 12, 22);
        announcementDate = announcementDate.AddDays(nearMonday[(int)announcementDate.DayOfWeek]);
    }
    /// <summary>
    /// 입학 정보 생성 1 (한 세이브 파일 내 전체에서)
    /// </summary>
    public static void CreateAdmissionInfo1()
    {
        universityAdmissionInfo = new UniversityAdmissionInfo[universities.Length];
        for (int i = 0; i < universities.Length; i++)
        {
            UniversityAdmissionInfo info = new UniversityAdmissionInfo();
            // 가중치 설정
            info.weight = new int[5];
            for (int j = 0; j < 5; j++)
            {
                info.weight[j] = Random.Range(universities[i].weightMin, 6);
            }
            universityAdmissionInfo[i] = info;
        }
    }
    /// <summary>
    /// 입학 정보 생성 2 (매 회차마다)
    /// </summary>
    public static void CreateAdmissionInfo2()
    {
        for (int i = 0; i < universities.Length; i++)
        {
            University univ = universities[i];
            UniversityAdmissionInfo info = universityAdmissionInfo[i];
            // 수능 최저 설정
            if (Random.Range(0, 100) < univ.hasSuneungMinChance)
            {
                info.suneungMinSubjectCount = Random.Range(univ.suneungMinSubjectCountMin, univ.suneungMinSubjectCountMax);
                info.suneungMinGradeSum = Random.Range(univ.suneungMinGradeSumMin * info.suneungMinSubjectCount / 5, univ.suneungMinGradeSumMax * info.suneungMinSubjectCount / 5);
            }
            // 지원 기간 설정
            do
            {
                info.earlyApplicationStart = Random.Range(0, 3);
                info.earlyApplicationEnd = info.earlyApplicationStart + Random.Range(3, 6);
            } while (info.earlyApplicationEnd > 5); // 범위 벗어나면 다시
            // 군 설정
            if (i < 10) // 조운1대~조운10대는
            {
                int r = Random.Range(0, 7);
                if (r < 3) // 0, 1, 2: 가
                {
                    info.regularGroup = 0;
                }
                else if (r < 6) // 3, 4, 5: 나
                {
                    info.regularGroup = 1;
                }
                else // 6: 다
                {
                    info.regularGroup = 2;
                }
            }
            else // 조운11대~조운20대는
            {
                int r = Random.Range(0, 5);
                if (r < 2) // 0, 1: 가
                {
                    info.regularGroup = 0;
                }
                else if (r < 4) // 2, 3: 나
                {
                    info.regularGroup = 1;
                }
                else // 4: 다
                {
                    info.regularGroup = 2;
                }
            }
            // 확률 값 설정
            int startDecrease = Random.Range(univ.chanceDecreaseStartMin, univ.chanceDecreaseStartMax);
            info.chance = new int[20];
            for (int j = 19; j >= 0; j--)
            {
                if (j > startDecrease) // 감소 시작 전에는
                {
                    info.chance[j] = 100; // 항상 100
                }
                else
                {
                    // 0 이하로는 안 떨어지게, j == 19일 경우 chance[20]이 없으니(기록없이 100 고정이니)
                    info.chance[j] = Mathf.Max((j == 19 ? 100 : info.chance[j + 1]) - Random.Range(univ.chanceDecreaseAmountMin, univ.chanceDecreaseAmountMax), 0);
                }
            }
        }
    }
    /// <summary>
    /// 전형 점수 계산
    /// </summary>
    /// <param name="admissionType">지원하려는 전형의 유형</param>
    /// <param name="university">지원하려는 대학의 번호</param>
    /// <returns>전형 점수 계산 결과</returns>
    /// <exception cref="ArgumentException">admissionType이 AdmissionType.NONE이거나 유효하지 않은 값일 때</exception>
    public static int GetAdmissionScore(AdmissionType admissionType, int university)
    {
        DateTime date = Util.InGameDate(GameData.time);
        if (admissionType == AdmissionType.EARLY_ADMISSION)
        {
            int total = 0;
            int divider = 0;
            for (int i = 0; i < 10; i++) // 10개의 시험(3-1 기말까지) 반영
            {
                if (date < GameData.type1ExamDate[i].AddDays(7)) // 성적 통지일 전인 시험이면 break
                {
                    break;
                }
                for (int j = 0; j < 5; j++)
                {
                    total += (9 - ExamManager.type1Exam[i].grade[j]) * 125 * universityAdmissionInfo[university].weight[j];
                    divider += universityAdmissionInfo[university].weight[j];
                }
            }
            if (divider == 0) // 1-1 중간 성적통지 전에는 divider가 0으로 오류가 나니
            {
                return 0;
            }
            return total / divider;
        }
        else if (admissionType == AdmissionType.REGULAR_ADMISSION)
        {
            for (int i = ExamManager.type2Exam.Length - 1; i >= 0; i--) // 오래된 것이 아닌 최근 것부터 찾아야 하니
            {
                if (date >= GameData.type2ExamDate[i].AddDays(14)) // 성적통지된 가장 최근의 모의고사/수능 시험을 찾음
                {
                    int total = 0;
                    int divider = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        total += (9 - ExamManager.type2Exam[i].grade[j]) * 125 * universityAdmissionInfo[university].weight[j];
                        divider += universityAdmissionInfo[university].weight[j];
                    }
                    return total / divider;
                }
            }
            return 0; // 1학년 3월 학평 전에는 0으로 처리
        }
        else
        {
            throw new ArgumentException("admissionType은 AdmissionType.EARLY_ADMISSION 또는 AdmissionType.REGULAR_ADMISSION이어야 합니다.");
        }
    }
    public static bool IsAdmissionScoreConfirmed(AdmissionType admissionType)
    {
        if (admissionType == AdmissionType.EARLY_ADMISSION)
        {
            return Util.InGameDate(GameData.time) >= GameData.type1ExamDate[9].AddDays(7);
        }
        else if (admissionType == AdmissionType.REGULAR_ADMISSION)
        {
            return Util.InGameDate(GameData.time) >= GameData.type2ExamDate[^1].AddDays(14);
        }
        else
        {
            throw new ArgumentException("admissionType은 AdmissionType.EARLY_ADMISSION 또는 AdmissionType.REGULAR_ADMISSION이어야 합니다.");
        }
    }
    public static int GetChance(int university, int score)
    {
        if (score >= 1000)
        {
            return 100;
        }
        else
        {
            return universityAdmissionInfo[university].chance[score / 50];
        }
    }
    /// <summary>
    /// 원서 지원의 기간 날짜 반환
    /// </summary>
    /// <param name="admissionType">입학전형의 유형</param>
    /// <param name="dateType">가져오려는 날짜의 종류 (0: 시작일, 1: 종료일(제외), 2: 종료일(포함))</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">admissionType이 AdmissionType.NONE이거나 유효하지 않은 값이거나 dateType이 0, 1, 2가 아닐 때</exception>
    public static DateTime GetApplicationDate(AdmissionType admissionType, int dateType)
    {
        if (admissionType == AdmissionType.EARLY_ADMISSION)
        {
            switch (dateType)
            {
                case 0:
                    return earlyApplicationStart;
                case 1:
                    return earlyApplicationEnd;
                case 2:
                    return earlyApplicationEnd2;
                default:
                    throw new ArgumentException("dateType은 0, 1, 2 중 하나여야 합니다.");
            }
        }
        else if (admissionType == AdmissionType.REGULAR_ADMISSION)
        {
            switch (dateType)
            {
                case 0:
                    return regularApplicationStart;
                case 1:
                    return regularApplicationEnd;
                case 2:
                    return regularApplicationEnd2;
                default:
                    throw new ArgumentException("dateType은 0, 1, 2 중 하나여야 합니다.");
            }
        }
        else
        {
            throw new ArgumentException("admissionType은 AdmissionType.EARLY_ADMISSION 또는 AdmissionType.REGULAR_ADMISSION이어야 합니다.");
        }
    }
    /// <summary>
    /// 합격, 불합격을 최종 판정
    /// </summary>
    public static void CheckPassed()
    {
        int[] suneungOrdered = null;
        if (admissionType == AdmissionType.EARLY_ADMISSION)
        {
            suneungOrdered = ExamManager.type2Exam[^1].grade.OrderBy(x => x).ToArray();
        }
        for (int i = 0; i < 3; i++)
        {
            int univ = appliedUniversities[i];
            if (univ == -1)
            {
                continue;
            }
            UniversityAdmissionInfo info = universityAdmissionInfo[univ];
            // 수능 최저 확인
            if (admissionType == AdmissionType.EARLY_ADMISSION && suneungOrdered.Take(info.suneungMinSubjectCount).Sum() > info.suneungMinGradeSum)
            {
                continue; // 수능 최저 미달이면 무조건 불합격
            }
            if (Random.Range(0, 100) < GetChance(univ, GetAdmissionScore(admissionType, univ)))
            {
                applicationPassed[i] = true;
            }
        }
    }
}
