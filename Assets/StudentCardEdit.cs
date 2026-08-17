using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SFB;
public class StudentCardEdit : MonoBehaviour
{
    [SerializeField] StudentCard studentCard;
    [SerializeField] TMP_InputField patternColor;
    [SerializeField] TextMeshProUGUI addPhotoText;
    [SerializeField] Button removePhoto;
    void Start()
    {
        patternColor.text = StudentCard.patternColor;
        addPhotoText.text = StudentCard.photoTexture == null ? "추가" : "변경";
        removePhoto.interactable = StudentCard.photoTexture != null;
    }
    public void ChangePatternColor(string text)
    {
        StudentCard.patternColor = text;
        studentCard.UpdateStudentCard();
    }
    public void AddOrChangePhoto()
    {
        string[] file = StandaloneFileBrowser.OpenFilePanel("학생증 사진 선택", "", new ExtensionFilter[] { new ExtensionFilter("PNG, JPG 사진", "png", "jpg") }, false);
        if (file.Length == 0)
        {
            return;
        }
        File.Copy(file[0], Path.Combine(Application.persistentDataPath, "studentCardPhoto", GameData.saveName), true);
        StudentCard.LoadPhoto();
        studentCard.UpdateStudentCard();
        addPhotoText.text = "변경";
        removePhoto.interactable = true;
    }
    public void RemovePhoto()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "studentCardPhoto", GameData.saveName));
        StudentCard.photoTexture = null;
        studentCard.UpdateStudentCard();
        addPhotoText.text = "추가";
        removePhoto.interactable = false;
    }
    public void CloseCompleted()
    {
        gameObject.SetActive(false);
    }
}
