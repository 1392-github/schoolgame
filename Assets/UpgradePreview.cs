using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using TMPro;
public class UpgradePreview : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    PropertyInfo[] upgradePropertys;
    // Start is called before the first frame update
    void Start()
    {
        upgradePropertys = new PropertyInfo[GameData.statTypes.Count];
        for (int i = 0; i < GameData.statTypes.Count; i++)
        {
            upgradePropertys[i] = typeof(GameData).GetProperty(GameData.statTypes[i].prop, BindingFlags.Public | BindingFlags.Static);
        }
        UpdatePreview();
    }
    public void UpdatePreview()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < GameData.statTypes.Count; i++)
        {
            stringBuilder.AppendLine($"{GameData.statTypes[i].name} Lv {GameData.stat[i]} ({GameData.statTypes[i].prefix}{upgradePropertys[i].GetValue(null)}{GameData.statTypes[i].suffix})");
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        text.text = stringBuilder.ToString();
    }
}
