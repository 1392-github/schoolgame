using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    DeleteButton2 deleteButton2;
    Text deleteText;
    GameObject deleteCheck;
    void Start()
    {
        deleteCheck = GameObject.Find("Canvas").transform.Find("DeleteCheck").gameObject;
        deleteButton2 = deleteCheck.transform.Find("Button (Legacy)").GetComponent<DeleteButton2>();
        deleteText = deleteCheck.transform.Find("Text (Legacy)").GetComponent<Text>();
    }
    public void Click()
    {
        string name = transform.parent.Find("Name").GetComponent<Text>().text;
        deleteButton2.saveName = name;
        deleteText.text = $"정말로 \"{name}\" 파일을 삭제하시겠습니까?";
        deleteCheck.SetActive(true);
    }
}
