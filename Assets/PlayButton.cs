using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        SaveBuffer buf = GameObject.Find("SaveData").GetComponent<SaveBuffer>();
        buf.name = transform.parent.Find("Name").GetComponent<UnityEngine.UI.Text>().text;
        buf.save = JsonUtility.FromJson<SaveFile1>(System.IO.File.ReadAllText(Application.persistentDataPath + $"/{buf.name}"));
        SceneManager.LoadScene("GlobalScene");
    }
}
