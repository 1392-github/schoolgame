using UnityEngine;

public class TheEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.DateTime end = GameObject.Find("EndDatePass").GetComponent<EndDatePass>().endDate;
        System.TimeSpan end2 = end - new System.DateTime(2024, 3, 4);
        Destroy(GameObject.Find("EndDatePass"));
        GetComponent<UnityEngine.UI.Text>().text = $@"축하갑니다! 당신은 이 게임을 클리어 했습니다
{end:yyyy년 M월 d일 H시 m분 s초}
({end2.Days}일 {end2.Hours}시간 {end2.Minutes}분 {end2.Seconds}초 소모)
[Enter]로 메인 화면으로 이동
(시간은 게임 시간입니다)";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectSaveScene");
        }
    }
}
