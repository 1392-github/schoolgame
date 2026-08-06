using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    readonly char[] choTable = {'¤¡', '¤¢', '¤¤', '¤§', '¤¨', '¤©', '¤±', '¤²', '¤³', '¤µ', '¤¶', '¤·', '¤¸', '¤¹', '¤º', '¤»', '¤¼', '¤½', '¤¾'};

    public Text text;
    public InputField input;
    public AudioClip typeSound;
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
        foreach (char c in text)
        {
            if (c >= '°¡' && c <= 'ÆR')
            {
                string before = this.text.text;
                int code = c - '°¡';
                int cho = code / 588;
                this.text.text = before + choTable[cho];
                source.PlayOneShot(typeSound);
                yield return new WaitForSeconds(delay);
                if (code % 28 != 0)
                {
                    this.text.text = before + (char)(0xac00 + code / 28 * 28);
                    source.PlayOneShot(typeSound);
                    yield return new WaitForSeconds(delay);
                }
                this.text.text = before + c;
            }
            else
            {
                this.text.text += c;
            }
            source.PlayOneShot(typeSound);
            yield return new WaitForSeconds(delay);
        }
    }
    // È£Ãâ ÈÄ yield return new WaitUntil(() => inputCompleted); À» È£ÃâÇÒ °Í
    void InputText(string placeholder)
    {
        inputCompleted = false;
        ((Text)input.placeholder).text = placeholder;
        input.text = "";
        input.gameObject.SetActive(true);
    }
    IEnumerator Intro()
    {
        yield return StartCoroutine(TypeText("[00°íµîÇÐ±³ ±³Àå] ¿©±ä ¾î´À ÇÐ±³ÀÌÁö?\n"));
        InputText("ÇÐ±³ÀÇ ÀÌ¸§Àº?");
        yield return new WaitUntil(() => inputCompleted);
        schoolName = input.text;
        yield return StartCoroutine(TypeText($"¾Æ, {schoolName}ÀÌ±º\n±×·³ ´ç½ÅÀÇ ÀÌ¸§Àº ´©±¸ÀÌÁö?\n"));
        InputText("´ç½ÅÀÇ ÀÌ¸§Àº?");
        yield return new WaitUntil(() => inputCompleted);
        name = input.text;
        yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] ¾Æ, {name}ÀÌ±º\n±×·³ ´ç½ÅÀº ¸î ³âµµ¿¡ ÅÂ¾î³µÁö?\n"));
        while (true)
        {
            InputText("´ç½ÅÀÇ »ý³âÀº? (¿¬µµ¸¸ ÀÔ·ÂÇÒ °Í)");
            yield return new WaitUntil(() => inputCompleted);
            if (int.TryParse(input.text, out birth))
            {
                if (birth < 1938)
                {
                    yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] ¹¹, {birth}³â? ±×¶§´Â Á¦1Â÷ ±³À°°úÁ¤µµ ¾ø¾ú´Âµ¥?\n(1938 ~ {nowYear} ¹üÀ§¿¡¼­ ÀÔ·ÂÇØÁÖ¼¼¿ä)\n"));
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
                        yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] ¾Æ, Àá¸¸, ¿ø·¡ ±³À°°úÁ¤ ¹Ù²î´Â °Í ³ÖÀ¸·Á°í ÇÏ´Âµ¥, ¾ÆÁ÷ ¾È ¸¸µé¾ú³×?\n(2003 ~ 2008 ¹üÀ§¿¡¼­ ÀÔ·ÂÇØÁÖ¼¼¿ä)\n"));
                    }
                }
                else
                {
                    yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] Áö±ÝÀº {nowYear}³âÀÎµ¥ ¾î¶»°Ô {birth}³â»ýÀÎÁö?\n(1938 ~ {nowYear} ¹üÀ§¿¡¼­ ÀÔ·ÂÇØÁÖ¼¼¿ä)\n"));
                }
            }
            else
            {
                yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] Àú°Ç ¼ýÀÚ°¡ ¾Æ´ÏÀÝ¾Æ!\n"));
            }
        }
        yield return StartCoroutine(TypeText($"[{schoolName} ±³Àå] {birth}³â»ýÀÌ·Î±º!\n"));
        yield return StartCoroutine(TypeText("ÀÌÁ¦ ÀÔÇÐÀ» ÇÒ±î?\n±×·³ 3³â°£ Àß ´Ù³àº¸µµ·Ï!", 0.2f));
        GameData.name = name;
        GameData.school = schoolName;
        GameData.birth = birth;
        GameData.nextDayOnHome = true;
        GameData.Load2();
        SceneManager.LoadScene("HomeScene");
    }
}
