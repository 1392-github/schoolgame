using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingEffect : MonoBehaviour
{
    public Vector3 speed;
    public float duration;
    Graphic graphic;
    float baseAlpha;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<Graphic>();
        baseAlpha = graphic.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, (duration - time) * baseAlpha);
        transform.Translate(speed * Time.deltaTime);
        if (time >= duration)
        {
            Destroy(gameObject);
        }
    }
}
