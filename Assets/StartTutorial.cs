using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    public void Click()
    {
        SaveBuffer buf = GameObject.Find("SaveData").GetComponent<SaveBuffer>();
        buf.save.schindex = 2;
        buf.tutorial = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GlobalScene");
    }
}
