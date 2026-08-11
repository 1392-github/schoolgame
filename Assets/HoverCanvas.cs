using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform hoverTextTransform;

    public static HoverCanvas instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        Vector3 position = Input.mousePosition + new Vector3(Screen.width / 50, 0);
        if (position.x > Screen.width - hoverTextTransform.rect.width)
        {
            position.x = Input.mousePosition.x - hoverTextTransform.rect.width - Screen.width / 50;
        }
        hoverTextTransform.position = position;
    }
    public void Open(string text)
    {
        this.text.text = text;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
