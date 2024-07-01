using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewSave : MonoBehaviour
{
    public void OnSubmit(string name)
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{name}", JsonUtility.ToJson(GameObject.Find("SaveData").GetComponent<SaveBuffer>().save));
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
