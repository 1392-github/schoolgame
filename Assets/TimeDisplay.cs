using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameData.time.ToString($"yyyy-MM-dd(ddd)\nHH:mm:ss\n{(int)(GameData.time - GameData.firstDay).TotalDays + 1}ÀÏÂ÷");
    }
}
