using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPBlock : MonoBehaviour
{
    bool init;
    GameObject ok;
    GameObject err;
    Tab tab;
    Text errText;
    public void Run()
    {
        if (!init)
        {
            ok = transform.Find("GetClass").gameObject;
            err = transform.Find("GetClassError").gameObject;
            errText = err.GetComponent<Text>();
            tab = GetComponent<Tab>();
            init = true;
        }
        ok.SetActive(!GameData.duringClassPlacement);
        err.SetActive(GameData.duringClassPlacement);
        errText.text = $"지금은 반 배정을 조회할 수 없습니다\n{GameData.endClassPlacement.ToString("yyyy-MM-dd")} 오전 8시부터 조회 가능합니다";
        tab.tabs[0] = GameData.duringClassPlacement ? err : ok;
    }
}
