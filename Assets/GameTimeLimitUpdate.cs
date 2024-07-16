using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeLimitUpdate : MonoBehaviour
{
    Text text;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    public void OnChange(string value)
    {
        if (value == "")
        {
            text.text = "";
            return;
        }
        if (value == "-")
        {
            input.text = "";
            return;
        }
        if (int.Parse(value) < 1)
        {
            input.text = "1";
            return;
        }
        text.text = $"(~{new DateTime(2024, 3, 4) + new TimeSpan(int.Parse(value) * 7, 0, 0, 0):yyyy-MM-dd})";
    }
}
