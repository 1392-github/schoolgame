using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudentCard : MonoBehaviour
{
    public static string patternColor;
    public static Texture2D photoTexture;

    // public Sprite[] patterns; 나중에 패턴 변경 기능 생기면 사용

    [SerializeField] Image pattern;
    [SerializeField] RawImage photo;
    [SerializeField] TextMeshProUGUI name;
    public void UpdateStudentCard()
    {
        if (ColorUtility.TryParseHtmlString(patternColor, out Color color))
        {
            pattern.color = color;
        }
        else
        {
            pattern.color = Color.gray;
        }
        photo.texture = photoTexture;
        if (photoTexture == null)
        {
            photo.color = Color.gray;
        }
        else
        {
            photo.color = Color.white;
        }
    }
    public static void LoadPhoto()
    {
        if (photoTexture != null)
        {
            Destroy(photoTexture);
        }
        string path = Path.Combine(Application.persistentDataPath, "studentCardPhoto", GameData.saveName);
        if (File.Exists(path))
        {
            photoTexture = new Texture2D(2, 2);
            photoTexture.LoadImage(File.ReadAllBytes(path), true);
        }
        else
        {
            photoTexture = null;
        }
    }
    void Start()
    {
        UpdateStudentCard();
        string name = GameData.name;
        int length = name.Length;
        if (length == 2)
        {
            this.name.text = $"{name.Substring(0, 1)}      {name.Substring(1, 1)}\n{GameData.school}";
        }
        else if (length == 3)
        {
            this.name.text = $"{name.Substring(0, 1)}  {name.Substring(1, 1)}  {name.Substring(2, 1)}\n{GameData.school}";
        }
        else if (length == 4)
        {
            this.name.text = $"{name.Substring(0, 1)} {name.Substring(1, 1)} {name.Substring(2, 1)} {name.Substring(3, 1)}\n{GameData.school}";
        }
        else
        {
            this.name.text = $"{name}\n{GameData.school}";
        }
    }
}
