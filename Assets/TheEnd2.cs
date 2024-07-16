using System.Linq;
using UnityEngine;

public class TheEnd2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int[] studyExp = GameObject.Find("EndDatePass").GetComponent<EndDatePass>().studyExp;
        TestScore score = GameObject.Find("EndDatePass").GetComponent<EndDatePass>().score;
        Destroy(GameObject.Find("EndDatePass"));
        GetComponent<UnityEngine.UI.Text>().text = $@"최종 능력치
국어 {studyExp[0]} / 수학 {studyExp[1]} / 사회 {studyExp[2]} / 과학 {studyExp[3]} / 영어 {studyExp[4]}
총합 <size=100>{studyExp.Sum()}</size>
최종 성적
국어 {score.grade[0]}({score.rank[0]}) / 수학 {score.grade[1]}({score.rank[1]}) / 사회 {score.grade[2]}({score.rank[2]}) / 과학 {score.grade[3]}({score.rank[3]}) / 영어 {score.grade[4]}({score.rank[4]})
평균 <size=100>{score.grade.Average()}</size>
[Enter]로 메인 화면으로 이동";
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
