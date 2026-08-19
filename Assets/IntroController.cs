using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public InputField input;
    public AudioSource source;

    string schoolName;
    string name;
    int birth;
    int nowYear;
    bool inputCompleted;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
        nowYear = DateTime.Now.Year;
    }
    public void CompleteInput()
    {
        inputCompleted = true;
        input.gameObject.SetActive(false);
    }
    IEnumerator TypeText(string text, float delay = 0.1f)
    {
        yield return StartCoroutine(Util.TypeText(text, this.text, source));
    }
    // 호출 후 yield return new WaitUntil(() => inputCompleted); 을 호출할 것
    void InputText(string placeholder)
    {
        inputCompleted = false;
        ((Text)input.placeholder).text = placeholder;
        input.text = "";
        input.gameObject.SetActive(true);
    }
    IEnumerator Intro()
    {
        yield return StartCoroutine(TypeText("[00고등학교 교장] 여긴 어느 학교이지?\n"));
        InputText("학교의 이름은?");
        yield return new WaitUntil(() => inputCompleted);
        schoolName = input.text.Trim();
        yield return StartCoroutine(TypeText($"아, {schoolName}이군\n그럼 당신의 이름은 누구이지?\n"));
        InputText("당신의 이름은?");
        yield return new WaitUntil(() => inputCompleted);
        name = input.text.Trim();
        yield return StartCoroutine(TypeText($"[{schoolName} 교장] 아, {name}이군\n그럼 당신은 몇 년도에 태어났지?\n"));
        while (true)
        {
            InputText("당신의 생년은? (연도만 입력할 것)");
            yield return new WaitUntil(() => inputCompleted);
            if (int.TryParse(input.text.Trim(), out birth))
            {
                if (birth < 1938)
                {
                    yield return StartCoroutine(TypeText($"[{schoolName} 교장] 뭐, {birth}년? 그때는 제1차 교육과정도 없었는데?\n(1938 ~ {nowYear} 범위에서 입력해주세요)\n"));
                    if (birth == 1392)
                    {

                    }
                }
                else if (birth <= nowYear)
                {
                    if (birth >= 2003 && birth <= 2008)
                    {
                        break;
                    }
                    else
                    {
                        yield return StartCoroutine(TypeText($"[{schoolName} 교장] 아, 잠만, 원래 교육과정 바뀌는 것 넣으려고 하는데, 아직 안 만들었네?\n(2003 ~ 2008 범위에서 입력해주세요)\n"));
                    }
                }
                else
                {
                    yield return StartCoroutine(TypeText($"[{schoolName} 교장] 지금은 {nowYear}년인데 어떻게 {birth}년생인지?\n(1938 ~ {nowYear} 범위에서 입력해주세요)\n"));
                }
            }
            else
            {
                yield return StartCoroutine(TypeText($"[{schoolName} 교장] 저건 숫자가 아니잖아!\n"));
            }
        }
        yield return StartCoroutine(TypeText($"[{schoolName} 교장] {birth}년생이로군!\n"));
        yield return StartCoroutine(TypeText("이제 입학을 할까?\n그럼 3년간 잘 다녀보도록!", 0.2f));
        GameData.name = name;
        GameData.school = schoolName;
        GameData.birth = birth;
        GameData.nextDayOnHome = true;
        ExamManager.type1Exam = new ExamScore[GameData.type1Exams.Length];
        ExamManager.type2Exam = new ExamScore[GameData.curriculum.type2Exam.Length];
        GameData.Load2();
        SceneManager.LoadScene("HomeScene");
    }
}
