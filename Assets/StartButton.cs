using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public List<ExperData> exp;
    public List<int> diff;
    public GameObject expCheck;
    public GameObject timeSelect;
    public Transform expScroll;
    public InputField name;
    public InputField length;
    Dictionary<Experimental, Toggle> expSelect;
    public Dropdown mulDropdown;
    public SaveFile4 defaultSave;

    // Start is called before the first frame update
    void Start()
    {
        expSelect = new Dictionary<Experimental, Toggle>();
        foreach (ExperData item in exp)
        {
            GameObject g = Instantiate(expCheck);
            g.transform.SetParent(expScroll, false);
            g.transform.Find("Text (Legacy)").GetComponent<Text>().text = item.name;
            g.transform.Find("Text (Legacy) (1)").GetComponent<Text>().text = item.desc;
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
        if (length.text != "")
        {
            defaultSave.length = int.Parse(length.text);
        }
        defaultSave.costWeightStatus = Random.Range(0, 2) == 0;
        for (int i = 0; i < 5; i++)
        {
            defaultSave.stockStatus[i] = Random.Range(0, 2) == 0;
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{name.text}", JsonUtility.ToJson(defaultSave));
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
    }
    public void ChangeMul(int i)
    {
        defaultSave.difficulty = diff[i];
    }
}
