using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyExpDisplay : MonoBehaviour
{
    Text text;
    public int index;
    float time;
    long oldValue;
    bool exactValue;
    public RectTransform background;
    Vector3 anchor;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        time = -1;
        anchor = GetComponent<RectTransform>().anchorMin;
    }

    // Update is called once per frame
    void Update()
    {
        if (time == -1)
        {
            if (oldValue != GameData.studyExp[index])
            {
                time = 0;
            }
        }
        else
        {
            text.text = Convert((long)(oldValue + (GameData.studyExp[index] - oldValue) * time));
            time += Time.deltaTime;
            if (time >= 1)
            {
                time = -1;
                oldValue = GameData.studyExp[index];
                text.text = Convert(GameData.studyExp[index]);
            }
        }
    }
    public void SetExact(bool v)
    {
        exactValue = v;
        if (v)
        {
            text.color = new Color32(50, 50, 50, 255);
            background.anchorMax = anchor;
            background.anchorMin = anchor;
        }
        background.gameObject.SetActive(v);
        background.SetAsLastSibling();
        transform.SetAsLastSibling();
        if (time == -1)
        {
            time = 1;
        }
    }
    public string Convert(long val)
    {
        if (exactValue || val < 1000000)
        {
            return val.ToString();
        }
        else if (val < 1000000000)
        {
            return $"{val / 1000000} M";
        }
        else if (val < 1000000000000)
        {
            return $"{val / 1000000000} B";
        }
        else if (val < 1000000000000000)
        {
            return $"{val / 1000000000000} T";
        }
        else
        {
            return $"{val / 1000000000000000} Qd";
        }
    }
}
