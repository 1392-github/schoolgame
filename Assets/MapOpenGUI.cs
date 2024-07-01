using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpenGUI : MonoBehaviour
{
    public string name;
    GameObject target;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Canvas").transform.Find(name).gameObject;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Click()
    {
        target.SetActive(true);
    }
}
