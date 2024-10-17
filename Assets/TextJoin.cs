using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class TextJoin : MonoBehaviour
{
    public RectTransform target;
    public Direction direction;
    public bool reverse;
    RectTransform self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        self.position = new Vector3(direction == Direction.width ? (reverse ? target.position.x - target.sizeDelta.x : target.position.x + target.sizeDelta.x) : target.position.x, direction == Direction.height ? (reverse ? target.position.y - target.sizeDelta.y : target.position.y + target.sizeDelta.y) : target.position.y);
    }
}
