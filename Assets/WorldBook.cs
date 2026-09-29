using UnityEngine;

[CreateAssetMenu(fileName = "WorldBook", menuName = "ScriptableObject/WorldBook")]
public class WorldBook : ScriptableObject
{
    [TextArea(20,50)]
    public string[] content;
}