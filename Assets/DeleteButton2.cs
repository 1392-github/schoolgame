using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteButton2 : MonoBehaviour
{
    public string saveName;
    public void Click()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "saves", saveName));
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
