using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public void CloseCompleted()
    {
        gameObject.SetActive(false);
    }
}
