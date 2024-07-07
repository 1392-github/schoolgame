using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    width,
    height
}
[ExecuteAlways]
public class UseChildSize2 : MonoBehaviour
{
    public Direction direction;
    RectTransform self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float r = 0;
        foreach (Transform item in transform)
        {
            RectTransform rt = item.GetComponent<RectTransform>();
            r += direction == Direction.width ? rt.sizeDelta.x : rt.sizeDelta.y;
        }
        self.sizeDelta = direction == Direction.width ? new Vector2(r, self.sizeDelta.y) : new Vector2(self.sizeDelta.x, r);
    }
}
