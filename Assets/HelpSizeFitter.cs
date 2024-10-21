using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class HelpSizeFitter : MonoBehaviour
{
    public RectTransform obj;
    public RectTransform obj2;
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
        self.sizeDelta = new Vector2(self.sizeDelta.x, enabled ? obj2.sizeDelta.y + obj.sizeDelta.y : obj2.sizeDelta.y);
    }
    public void Click()
    {
        enabled = !enabled;
        obj.gameObject.SetActive(enabled);
    }
}
