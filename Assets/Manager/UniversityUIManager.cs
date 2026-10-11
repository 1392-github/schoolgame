using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversityUIManager : MonoBehaviour
{
    public Transform universityList;
    public GameObject universityListItemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // 대학교 목록 생성
        for (int i = 0; i < UniversityManager.universities.Length; i++)
        {
            University university = UniversityManager.universities[i];
            UniversityListItem item = Instantiate(universityListItemPrefab, universityList).GetComponent<UniversityListItem>();
            item.universityName.text = university.name + "대학교";
            item.scoreEarly.text = Random.Range(0, 1000).ToString(); // 임시
            item.chanceEarly.text = $"{Random.Range(0, 100)}%"; // 임시
            item.scoreRegular.text = Random.Range(0, 1000).ToString(); // 임시
            item.chanceRegular.text = $"{Random.Range(0, 100)}%"; // 임시
            item.admissionCount.text = UniversityManager.universityCount[i].ToString();
            int i2 = i;
            item.button.onClick.AddListener(() =>
            {
                UniversityAdmissionInfoUIManager.universityName = UniversityManager.universities[i2].name;
                UniversityAdmissionInfoUIManager.registerFee = UniversityManager.universities[i2].registerFee;
                UniversityAdmissionInfoUIManager.info = UniversityManager.universityAdmissionInfo[i2];
                SceneManager.LoadScene("UniversityAdmissionInfo");
            });
        }
    }
}
