using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
public class QuestPreview : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePreview();
        GameData.questPreview = this;
    }
    public void UpdatePreview()
    {
        StringBuilder stringBuilder = new StringBuilder($"Äù½ºÆ® {GameData.quest.Count} / {GameData.maxQuest}\n\n");
        foreach (Quest quest in GameData.quest)
        {
            stringBuilder.AppendLine($"+{quest.reward} XP");
            for (int i = 0; i < 5; i++)
            {
                if (quest.req[i] > 0)
                {
                    stringBuilder.AppendLine($"<color={(GameData.studyExp[i] >= quest.req[i] ? "#008000" : "red")}>{Util.subjectName[i]} {quest.req[i]} ÀÌ»ó</color>");
                }
            }
            stringBuilder.Append("\n");
        }
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        text.text = stringBuilder.ToString();
    }
}
