using UnityEngine;
using TMPro;

public class WorldBookUIManager : MonoBehaviour
{
    public static WorldBook book;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI leftPage;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI rightPage;
    [SerializeField] AudioClip pageTurnClip;
    AudioSource audioSource;
    int page;
    int totalPage;
    void Start()
    {
        totalPage = book.content.Length;
        audioSource = GetComponent<AudioSource>();
        UpdatePage();
    }
    void UpdatePage()
    {
        leftPage.text = (page + 1).ToString();
        leftText.text = book.content[page];
        if (page + 1 < totalPage)
        {
            rightPage.text = (page + 2).ToString();
            rightText.text = book.content[page+1];
        }
        else
        {
            rightPage.text = "";
            rightText.text = "";
        }
        audioSource.PlayOneShot(pageTurnClip);
    }
    public void ClickLeft()
    {
        if (page > 0)
        {
            page -= 2;
            UpdatePage();
        }
    }
    public void ClickRight()
    {
        if (page + 1 < totalPage)
        {
            page += 2;
            UpdatePage();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ClickLeft();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ClickRight();
        }
    }
}
