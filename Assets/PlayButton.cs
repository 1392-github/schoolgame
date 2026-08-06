using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    T GetSaveFile<T>() where T : SaveFile0
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "saves", GameData.saveName)));
    }
    public void Play()
    {
        GameData.saveName = transform.parent.Find("Name").GetComponent<UnityEngine.UI.Text>().text;
        int version = GetSaveFile<SaveFile0>().version;
        SaveFile0 save;
        if (version < 8)
        {
            GameObject.Find("Canvas").transform.Find("UnsupportedError").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("UnsupportedError").Find("Text (Legacy)").GetComponent<UnityEngine.UI.Text>().text = "우리들의 학교생활 1.0~20 - 21~ 간의 호환은 지원되지 않습니다. 해당 파일을 플레이하려면 1.0~20 버전을 이용해 주세요.";
            return;
        }
        else if (version == 8)
        {
            save = GetSaveFile<SaveFile8>();
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("UnsupportedError").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("UnsupportedError").Find("Text (Legacy)").GetComponent<UnityEngine.UI.Text>().text = $"호환되지 않는 버전입니다\n{GetSaveFile<SaveFile0>().versionName} 이상의 버전으로 플레이해 주세요";
            return;
        }
        SaveFile8 save2 = (SaveFile8)save;
        GameData.Load(save2);
        string scene;
        if (save2.introCompleted)
        {
            if (GameData.currentScene == "Home")
            {
                scene = "HomeScene";
            }
            else
            {
                scene = "GlobalScene";
            }
        }
        else
        {
            scene = "IntroScene";
        }
        SceneManager.LoadScene(scene);
    }
}
