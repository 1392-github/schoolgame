//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ExpDisplay : MonoBehaviour
//{
//    Player player;
//    Data data;
//    Text text;
//    // Start is called before the first frame update
//    void Start()
//    {
//        player = GameObject.Find("Player").GetComponent<Player>();
//        data = GameObject.Find("Data").GetComponent<Data>();
//        text = GetComponent<Text>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (player.lv == player.maxLevel)
//        {
//            text.text = $"{player.exp} (Max Level)";
//        }
//        else
//        {
//            text.text = $"{player.exp} / {player.needExpForLvUP} ({(float)player.exp / player.needExpForLvUP * 100:F2}%)";
//        }
//    }
//}
