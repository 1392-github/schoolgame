using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperOnly : MonoBehaviour
{
    public bool negative;
    public Experimental experimental;
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.ExperimentalCheck(experimental))
        {
            gameObject.SetActive(!negative);
        }
        else
        {
            gameObject.SetActive(negative);
        }
    }
}
