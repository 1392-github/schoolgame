using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;

    public Chat currentChat;
    public int currentChatElement;
    public int nextChatElement;
    public TextMeshProUGUI chatTitleText;
    public TextMeshProUGUI chatContentText;
    public GameObject optionButton;
    public Transform chatOption;
    public AudioSource audioSource;
    bool enableNext;
    bool enableNext2;
    public static void OpenChat(Chat chat)
    {
        instance.OpenChat1(chat);
    }
    public void OpenChat1(Chat chat)
    {
        gameObject.SetActive(true);
        currentChat = chat;
        currentChatElement = 0;
        chatTitleText.text = chat.name;
        updateChat();
    }
    void updateChat()
    {
        if (currentChatElement == -1)
        {
            gameObject.SetActive(false);
            currentChat.endEvent?.Invoke();
            currentChat = null;
            return;
        }
        ChatElement e = currentChat.value[currentChatElement];
        object[] chatExtra = e.chatEvent?.Invoke() ?? new object[0];
        if (e.next == -2)
        {
            nextChatElement = 0;
        }
        else if (e.next == 0)
        {
            if (currentChatElement == currentChat.value.Count - 1)
            {
                nextChatElement = -1;
            }
            else
            {
                nextChatElement = currentChatElement + 1;
            }
        }
        else
        {
            nextChatElement = e.next;
        }
        enableNext2 = false;
        foreach (Transform item2 in chatOption)
        {
            Destroy(item2.gameObject);
        }
        chatContentText.text = "";
        StartCoroutine(Chat(string.Format(e.value, chatExtra), e));
    }
    IEnumerator Chat(string text, ChatElement e)
    {
        yield return StartCoroutine(Util.TypeText(text, chatContentText, audioSource));
        if (e.option.Count == 0)
        {
            enableNext = true;
        }
        else
        {
            enableNext = false;
            foreach (NameAndVal<int> item in e.option)
            {
                int n = item.value;
                GameObject button = Instantiate(optionButton, chatOption);
                button.GetComponent<Button>().onClick.AddListener(() => ChatOptionSelect(n));
                yield return StartCoroutine(Util.TypeText(item.name, button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), audioSource));
            }
        }
        if (e.disableNext)
        {
            enableNext = false;
        }
        enableNext2 = true;
    }
    public void NextChat()
    {
        if (enableNext && enableNext2)
        {
            currentChatElement = nextChatElement;
            updateChat();
        }
    }
    public void NextChat2()
    {
        currentChatElement = nextChatElement;
        updateChat();
    }
    public void ChatOptionSelect(int id)
    {
        if (enableNext2)
        {
            currentChatElement = id;
            updateChat();
        }
    }
}
