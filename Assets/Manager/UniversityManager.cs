using System;
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
    public static void Init()
    {
        Array.Resize(ref universityCount, universities.Length);
        if (appliedUniversities.Length == 0) appliedUniversities = new int[] {-1, -1, -1};
        if (applicationPassed.Length == 0) applicationPassed = new bool[3];
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
}
