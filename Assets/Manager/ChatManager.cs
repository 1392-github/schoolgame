using System;
using System.Collections;
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
    public GameObject skipButton;
    public GameObject optionButton;
    public Transform chatOption;
    public AudioSource audioSource;
    bool enableNext;
    bool enableNext2;
    IEnumerator typeTextCoroutine;
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
        skipButton.SetActive(chat.skipable);
        if (chat.pauseGame)
        {
            GameData.pause = true;
        }
        updateChat();
    }
    void updateChat()
    {
        if (currentChatElement == -1)
        {
            gameObject.SetActive(false);
            if (currentChat.pauseGame)
            {
                GameData.pause = false;
            }
            if (currentChat.endEvent != -1)
            {
                ((Action)GlobalEventManager.events[currentChat.endEvent])();
            }
            currentChat = null;
            return;
        }
        ChatElement e = currentChat.value[currentChatElement];
        object[] chatExtra;
        if (e.chatEvent == -1)
        {
            chatExtra = new object[0];
        }
        else
        {
            chatExtra = ((Func<object[]>)GlobalEventManager.events[e.chatEvent])();
        }
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
        StartCoroutine(Chat((e.character == "" ? "" : $"[{(e.character == "S" ? GameData.name : e.character)}] ") + string.Format(e.value, chatExtra), e));
    }
    IEnumerator Chat(string text, ChatElement e)
    {
        typeTextCoroutine = Util.TypeText(text, chatContentText, audioSource);
        yield return StartCoroutine(typeTextCoroutine);
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
    public void SkipChat()
    {
        StopCoroutine(typeTextCoroutine);
        currentChatElement = -1;
        updateChat();
    }
}
