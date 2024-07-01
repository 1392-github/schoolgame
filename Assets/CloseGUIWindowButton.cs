using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGUIWindowButton : MonoBehaviour
{
    public bool isTutorialEnd;
    public void Click()
    {
        if (GameObject.Find("Player").GetComponent<Player>().tutorial && GameObject.Find("Player").GetComponent<Player>().scores.Count != 0 && isTutorialEnd)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
        }
        transform.parent.gameObject.SetActive(false);
    }
}
