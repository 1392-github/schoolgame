using UnityEngine;

/// <summary>
/// 대학교의 정보
/// </summary>
[CreateAssetMenu(fileName = "University", menuName = "ScriptableObject/University")]
public class University : ScriptableObject
{
    /// <summary>
    /// 대학교의 이름 (뒤의 "대학교"는 제외)
    /// </summary>
    public string name;
    /// <summary>
    /// 대학교 효과의 설명
    /// </summary>
    public string effect;
}
