using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class IntroController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public InputField input;
    public AudioSource source;

    string schoolName;
    string name;
    int birth;
    string saveName;
    int nowYear;
    bool inputCompleted;
    readonly char[] bannedChar = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
        nowYear = DateTime.Now.Year;
    }
    public void CompleteInput()
    {
        if (!string.IsNullOrWhiteSpace(input.text))
        {
            inputCompleted = true;
            input.gameObject.SetActive(false);
        }
    }
    IEnumerator TypeText(string text, float delay = 0.1f)
    {
        yield return StartCoroutine(Util.TypeText(text, this.text, source, delay));
    }
    // 호출 후 yield return new WaitUntil(() => inputCompleted); 을 호출할 것
    void InputText(string placeholder, string def = "")
    {
        inputCompleted = false;
        ((Text)input.placeholder).text = placeholder;
        input.text = def;
        input.gameObject.SetActive(true);
    }
    IEnumerator HiddenLevelModeCheck()
    {
        while (true)
        {
            if (inputCompleted)
            {
                break;
            }
            if (input.text == "1392chounhyuk")
            {
                GameData.hiddenLevelMode = true;
            }
            yield return null;
        }
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
        yield return StartCoroutine(TypeText($"[{schoolName} 교장] {birth}년생이로군!\n그럼 학생부 명칭은 무엇으로 하지?\n"));
        while (true)
        {
            saveName = name;
            int i = 1;
            while (File.Exists(Path.Join(Application.persistentDataPath, "saves", saveName)))
            {
                i++;
                saveName = name + i.ToString();
            }
            InputText("학생부 명칭(저장 파일 명칭)은?", saveName);
            yield return StartCoroutine(HiddenLevelModeCheck());
            saveName = input.text.Trim();
            if (saveName.IndexOfAny(bannedChar) >= 0)
            {
                yield return StartCoroutine(TypeText($"[{schoolName} 교장] 학생부 이름에는 \\, /, :, *, ?, \", <, >, |는 못 쓴다고!\n"));
                continue;
            }
            if (File.Exists(Path.Join(Application.persistentDataPath, "saves", saveName)))
            {
                yield return StartCoroutine(TypeText($"[{schoolName} 교장] 학생부 이름이 겹치는데?\n"));
                continue;
            }
            break;
        }
        yield return StartCoroutine(TypeText("이제 입학을 할까?\n그럼 3년간 잘 다녀보도록!", 0.2f));
        GameData.name = name;
        GameData.school = schoolName;
        GameData.birth = birth;
        GameData.saveName = saveName;
        GameData.nextDayOnHome = true;
        GameData.clas = Random.Range(0, 10);
        ExamManager.type1Exam = new ExamScore[GameData.type1Exams.Length];
        ExamManager.type2Exam = new ExamScore[GameData.curriculum.type2Exam.Length];
        GameData.Load2();
        SceneManager.LoadScene("HomeScene");
    }
}
