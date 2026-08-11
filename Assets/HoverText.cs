using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Multiline] string text;
    bool activedSelf;
    public void OnPointerEnter(PointerEventData eventData)
    {
        activedSelf = true;
        HoverCanvas.instance.Open(text);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        activedSelf = false;
        HoverCanvas.instance.Close();
    }
    void OnDestroy()
    {
        if (activedSelf)
        {
            HoverCanvas.instance.Close();
        }
    }
}
