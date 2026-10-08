using UnityEngine;

/// <summary>
/// 대학교의 정보
/// </summary>
[CreateAssetMenu(fileName = "University", menuName = "ScriptableObject/University")]
public class University : ScriptableObject
{
    // 모든 범위에서 최솟값은 포함하고, 최댓값은 제외함
    /// <summary>
    /// 대학교의 이름 (뒤의 "대학교"는 제외)
    /// </summary>
    public string name;
    /// <summary>
    /// 대학교 효과의 설명
    /// </summary>
    public string effect;
    /// <summary>
    /// 과목 가중치의 범위 (최솟값~5포함)
    /// </summary>
    public int weightMin;
    /// <summary>
    /// 수능 최저를 가질 확률 (%)
    /// </summary>
    public int hasSuneungMinChance;
    /// <summary>
    /// 수능 최저가 있을 때, a합 b에서 a 값의 최솟값
    /// </summary>
    public int suneungMinSubjectCountMin;
    /// <summary>
    /// 수능 최저가 있을 때, a합 b에서 a 값의 최댓값
    /// </summary>
    public int suneungMinSubjectCountMax;
    /// <summary>
    /// 수능 최저가 있을 때, a합 b에서 b 값의 최솟값
    /// </summary>
    public int suneungMinGradeSumMin;
    /// <summary>
    /// 수능 최저가 있을 때, a합 b에서 b 값의 최댓값
    /// </summary>
    public int suneungMinGradeSumMax;
    public int chanceDecreaseStartMin;
    public int chanceDecreaseStartMax;
    public int chanceDecreaseAmountMin;
    public int chanceDecreaseAmountMax;
}
