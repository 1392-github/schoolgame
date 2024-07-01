using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectSaveDisplay : MonoBehaviour
{
    public GameObject save;
    // Start is called before the first frame update
    void Start()
    {
        foreach (string file in Directory.GetFiles(Application.persistentDataPath))
        {
            string file2 = Path.GetFileName(file);
            if (file2 == "Player.log" || file2 == "Player-prev.log")
            {
                continue;
            }
            GameObject save = Instantiate(this.save);
            save.transform.Find("Name").GetComponent<Text>().text = Path.GetFileName(file2);
            save.transform.SetParent(transform, false);
        }
    }
}
