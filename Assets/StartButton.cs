using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public List<NameAndVal<Experimental>> exp;
    public GameObject expCheck;
    public Transform expScroll;
    public InputField name;
    Dictionary<Experimental, Toggle> expSelect;
    public SaveFile1 defaultSave;
    // Start is called before the first frame update
    void Start()
    {
        expSelect = new Dictionary<Experimental, Toggle>();
        foreach (NameAndVal<Experimental> item in exp)
        {
            GameObject g = Instantiate(expCheck);
            g.transform.SetParent(expScroll, false);
            g.transform.Find("Text (Legacy)").GetComponent<Text>().text = item.name;
            expSelect.Add(item.value, g.transform.Find("Toggle").GetComponent<Toggle>());
        }
    }
    public void Click()
    {
        foreach (KeyValuePair<Experimental, Toggle> item in expSelect)
        {
            if (item.Value.isOn)
            {
                defaultSave.experimental.Add(item.Key);
            }
        }
        if (defaultSave.experimental.Contains(Experimental.FRIEND_SYSTEM))
        {
            defaultSave.clas = Enumerable.Repeat(-1, 1000).ToArray();
        }
        else
        {
            defaultSave.clas = new int[] {-1};
        }
        defaultSave.startTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{name.text}", JsonUtility.ToJson(defaultSave));
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
}
