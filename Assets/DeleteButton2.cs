using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton2 : MonoBehaviour
{
    public string saveName;
    public void Click()
    {
        System.IO.File.Delete(Application.persistentDataPath + $"/{saveName}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
