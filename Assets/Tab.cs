using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public List<GameObject> tabs;
    public void TabButton(int id)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetActive(i == id);
        }
    }
}
