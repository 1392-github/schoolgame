using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraduationManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] Chat graduation2Chat;
    [SerializeField] Chat graduation3Chat;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image theEndImage;
    [SerializeField] TextMeshProUGUI theEndText;
    [SerializeField] Image startEffect;
    AudioSource audioSource;
    IEnumerator GraduationMain()
    {
        yield return StartCoroutine(Util.TypeText($"그렇게 나의 3년간의 고등학교 생활은 끝이 나게 되었다.\n\n그리고...\n\n나는 {(UniversityManager.finalUniversity == -1 ? "아무 대학에도 입학하지 못했다." : $"{UniversityManager.universities[UniversityManager.finalUniversity].name}대학교에 입학했다.")}\n\n" +
            $"최종 능력치\n\n국어 {GameData.studyExp[0]}, 수학 {GameData.studyExp[1]}, " +
            $"사회 {GameData.studyExp[2]}, 과학 {GameData.studyExp[3]}, 영어 {GameData.studyExp[4]}\n\n", introText, audioSource));
        yield return StartCoroutine(Util.TypeText($"([Enter] 클릭이나 화면 클릭/터치)", introText, audioSource, 0.05f));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0));
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            introText.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        introText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        ChatManager.OpenChat(graduation2Chat);
        yield return new WaitUntil(() => ChatManager.instance.currentChat == null);
        alpha = 0;
        backgroundImage.gameObject.SetActive(true);
        while (alpha < 1)
        {
            alpha += Time.deltaTime / 3f;
            backgroundImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        backgroundImage.color = Color.white;
        ChatManager.OpenChat(graduation3Chat);
        yield return new WaitUntil(() => ChatManager.instance.currentChat == null);
        alpha = 0;
        theEndImage.gameObject.SetActive(true);
        while (alpha < 1)
        {
            alpha += Time.deltaTime / 3f;
            theEndImage.color = new Color(1, 1, 1, alpha);
            theEndText.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0));
        alpha = 0;
        startEffect.gameObject.SetActive(true);
        while (alpha < 1)
        {
            alpha += Time.deltaTime / 5f;
            startEffect.color = new Color(1, 1, 1, alpha);
            audioSource.volume = 1 - alpha;
            yield return null;
        }
        GameData.NewGamePlus();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GraduationMain());
    }
}
