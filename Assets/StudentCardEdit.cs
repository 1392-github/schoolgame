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
#if UNITY_ANDROID
    bool loadPhoto = false;
#endif
    void Start()
    {
        patternColor.text = StudentCard.patternColor;
        addPhotoText.text = StudentCard.photoTexture == null ? "추가" : "변경";
        removePhoto.interactable = StudentCard.photoTexture != null;
    }
#if UNITY_ANDROID
    void Update()
    {
        if (loadPhoto)
        {
            StudentCard.LoadPhoto();
            studentCard.UpdateStudentCard();
            addPhotoText.text = "변경";
            removePhoto.interactable = true;
            loadPhoto = false;
        }
    }
#endif
    public void ChangePatternColor(string text)
    {
        StudentCard.patternColor = text;
        studentCard.UpdateStudentCard();
    }
    public void AddOrChangePhoto()
    {
#if UNITY_STANDALONE_WIN
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
#endif
#if UNITY_ANDROID
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                File.Copy(path, Path.Combine(Application.persistentDataPath, "studentCardPhoto", GameData.saveName), true);
                loadPhoto = true;
            }
        }, "학생증 이미지 선택");
#endif
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
