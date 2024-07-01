using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPBlock : MonoBehaviour
{
    Player player;
    GameObject ok;
    GameObject err;
    Tab tab;
    Text errText;
    public void Run()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            ok = transform.Find("GetClass").gameObject;
            err = transform.Find("GetClassError").gameObject;
            errText = err.GetComponent<Text>();
            tab = GetComponent<Tab>();
        }
        ok.SetActive(!player.duringClassPlacement);
        err.SetActive(player.duringClassPlacement);
        errText.text = $"지금은 반 배정을 조회할 수 없습니다\n{player.endClassPlacement.ToString("yyyy-MM-dd")} 오전 8시부터 조회 가능합니다";
        tab.tabs[0] = player.duringClassPlacement ? err : ok;
    }
}
