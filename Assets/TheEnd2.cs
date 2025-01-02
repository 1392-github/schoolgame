using System.Linq;
using UnityEngine;

public class TheEnd2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        long[] studyExp = GameObject.Find("EndDatePass").GetComponent<EndDatePass>().studyExp;
        TestScore score = GameObject.Find("EndDatePass").GetComponent<EndDatePass>().score;
        Destroy(GameObject.Find("EndDatePass"));
        GetComponent<UnityEngine.UI.Text>().text = $@"���� �ɷ�ġ
���� {studyExp[0]} / ���� {studyExp[1]} / ��ȸ {studyExp[2]} / ���� {studyExp[3]} / ���� {studyExp[4]}
���� <size=100>{studyExp.Sum()}</size>
���� ����
���� {score.grade[0]}({score.rank[0]}) / ���� {score.grade[1]}({score.rank[1]}) / ��ȸ {score.grade[2]}({score.rank[2]}) / ���� {score.grade[3]}({score.rank[3]}) / ���� {score.grade[4]}({score.rank[4]})
��� <size=100>{score.grade.Average()}</size>
[Enter]�� ���� ȭ������ �̵�";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
