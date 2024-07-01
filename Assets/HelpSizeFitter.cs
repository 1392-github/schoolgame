using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSizeFitter : MonoBehaviour
{
    RectTransform obj;
    RectTransform self;
    bool enabled;
    // Start is called before the first frame update
    void Start()
    {
        obj = transform.Find("Desc").GetComponent<RectTransform>();
        self = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        self.sizeDelta = new Vector2(self.sizeDelta.x, enabled ? 100 + obj.sizeDelta.y : 100);
    }
    public void Click()
    {
        enabled = !enabled;
        obj.gameObject.SetActive(enabled);
    }
}
