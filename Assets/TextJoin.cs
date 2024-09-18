using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class TextJoin : MonoBehaviour
{
    public RectTransform target;
    RectTransform self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        self.position = new Vector3(target.position.x + target.sizeDelta.x, self.position.y);
    }
}
