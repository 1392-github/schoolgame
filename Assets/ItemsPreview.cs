using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
public class ItemsPreview : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePreview();
    }
    public void UpdatePreview()
    {
        StringBuilder stringBuilder = new StringBuilder();
        int[] items = new int[GameData.items.Count];
        foreach (int item in GameData.inventory)
        {
            items[item]++;
        }
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] > 0)
            {
                stringBuilder.AppendLine($"{GameData.items[i].name} x{items[i]}");
            }
        }
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }
        text.text = stringBuilder.ToString();
    }
}
