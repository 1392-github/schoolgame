using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class UseChildSize : MonoBehaviour
{
    RectTransform child;
    RectTransform self;
    public bool width = true;
    public bool height = true;
    public int extra;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).GetComponent<RectTransform>();
        self = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        self.sizeDelta = new Vector2(width ? child.sizeDelta.x + extra : self.sizeDelta.x, height ? child.sizeDelta.y + extra : self.sizeDelta.y);
    }
}
