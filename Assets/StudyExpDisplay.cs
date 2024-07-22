using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyExpDisplay : MonoBehaviour
{
    Player player;
    Text text;
    public int index;
    [SerializeField]
    float time;
    [SerializeField]
    int oldValue;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        text = GetComponent<Text>();
        time = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (time == -1)
        {
            if (oldValue != player.studyExp[index])
            {
                time = 0;
            }
        }
        else
        {
            text.text = ((int)(oldValue + (player.studyExp[index] - oldValue) * time)).ToString();
            time += Time.deltaTime;
            if (time >= 1)
            {
                time = -1;
                oldValue = player.studyExp[index];
                text.text = player.studyExp[index].ToString();
            }
        }
    }
}
