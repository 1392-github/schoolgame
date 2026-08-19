using UnityEngine;

public class HoverCanvas : MonoBehaviour
{
    public HoverTextObject hoverText;
    public static HoverCanvas instance = null;
    public ChatManager chatManager;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ChatManager.instance = chatManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
