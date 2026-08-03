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
        foreach (string file in Directory.GetFiles(Path.Combine(Application.persistentDataPath, "saves")))
        {
            GameObject save = Instantiate(this.save);
            save.transform.Find("Name").GetComponent<Text>().text = Path.GetFileName(file);
            save.transform.SetParent(transform, false);
        }
    }
}
