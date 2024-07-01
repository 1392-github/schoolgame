using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public void Click()
    {
        System.IO.File.Delete(Application.persistentDataPath + $"/{transform.parent.Find("Name").GetComponent<UnityEngine.UI.Text>().text}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
