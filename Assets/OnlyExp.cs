using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyExp : MonoBehaviour
{
    public Experimental e;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.Find("Player").GetComponent<Player>().ExperimentalCheck(e))
        {
            GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }
}
