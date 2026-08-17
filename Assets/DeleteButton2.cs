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
        string studentCardPhoto = Path.Combine(Application.persistentDataPath, "studentCardPhoto", saveName);
        if (File.Exists(studentCardPhoto))
        {
            File.Delete(studentCardPhoto);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
