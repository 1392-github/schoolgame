using System;
using UnityEngine;

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
public static class UniversityManager
{
    /// <summary>
    /// 대학교들
    /// </summary>
    public static University[] universities;
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
    /// 지원한 대학의 합격 여부 (true: 합격, false: 불합격, 지원 전에는 전부 false임)
    /// </summary>
    public static bool[] applicationPassed;
    public static void Init()
    {
        Array.Resize(ref universityCount, universities.Length);
        if (appliedUniversities.Length == 0) appliedUniversities = new int[] {-1, -1, -1};
        if (applicationPassed.Length == 0) applicationPassed = new bool[3];
    }
}
