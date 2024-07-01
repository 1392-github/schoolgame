//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ExpBarDisplay : MonoBehaviour
//{
//    Player player;
//    RectTransform transform;
//    // Start is called before the first frame update
//    void Start()
//    {
//        player = GameObject.Find("Player").GetComponent<Player>();
//        transform = GetComponent<RectTransform>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (player.lv == player.maxLevel)
//        {
//            transform.anchorMax = new Vector2(1, 1);
//        }
//        else
//        {
//            transform.anchorMax = new Vector2((float)player.exp / player.needExpForLvUP, 1);
//        }
//    }
//}
