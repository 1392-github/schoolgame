using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpGenerator : MonoBehaviour
{
    public List<Help> helps;
    public GameObject prefab; 
    public Transform content;
    void Start()
    {
        foreach (Help help in helps)
        {
            GameObject g = Instantiate(prefab);
            g.transform.SetParent(content, false);
            g.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = "Q: " + help.name;
            g.transform.Find("Desc").GetComponent<UnityEngine.UI.Text>().text = "A: " + help.description;
        }
    }
}
