using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    DeleteButton2 deleteButton2;
    GameObject deleteCheck;
    void Start()
    {
        deleteCheck = GameObject.Find("Canvas").transform.Find("DeleteCheck").gameObject;
        deleteButton2 = deleteCheck.transform.Find("Button (Legacy)").GetComponent<DeleteButton2>();
    }
    public void Click()
    {
        deleteButton2.saveName = transform.parent.Find("Name").GetComponent<UnityEngine.UI.Text>().text;
        deleteCheck.SetActive(true);
    }
}
