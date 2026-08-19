using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public static class Util
{
    public static readonly string[] subjectName = { "±¹¾î", "¼öÇÐ", "»çÈ¸", "°úÇÐ", "¿µ¾î" };
    static readonly char[] choTable = { '¤¡', '¤¢', '¤¤', '¤§', '¤¨', '¤©', '¤±', '¤²', '¤³', '¤µ', '¤¶', '¤·', '¤¸', '¤¹', '¤º', '¤»', '¤¼', '¤½', '¤¾' };
    public static AudioClip typeSound;
    public static string DDay(DateTime from, DateTime to)
    {
        if (to > from)
        {
            return $"D-{(to - from).Days}";
        }
        else if (to < from)
        {
            return $"D+{(from - to).Days}";
        }
        else
        {
            return "D-Day";
        }
    }
    public static DateTime InGameDate(DateTime time)
    {
        if (time.Hour < 8)
        {
            return time.Date.AddDays(-1);
        }
        else
        {
            return time.Date;
        }
    }
    public static float GetNormalCDF(float z)
    {
        float a1 = 0.254829592f;
        float a2 = -0.284496736f;
        float a3 = 1.421413741f;
        float a4 = -1.453152027f;
        float a5 = 1.061405429f;
        float p = 0.3275911f;
        int sign = 1;
        if (z < 0)
            sign = -1;
        z = Mathf.Abs(z) / Mathf.Sqrt(2);

        float t = 1 / (1 + p * z);
        float y = 1 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-z * z);

        return 0.5f * (1 + sign * y);
    }
    public static IEnumerator TypeText(string text, TextMeshProUGUI textMesh, AudioSource source, float delay = 0.1f)
    {
        foreach (char c in text)
        {
            if (c >= '°¡' && c <= 'ÆR')
            {
                string before = textMesh.text;
                int code = c - '°¡';
                int cho = code / 588;
                textMesh.text = before + choTable[cho];
                source.PlayOneShot(typeSound);
                yield return new WaitForSeconds(delay);
                if (code % 28 != 0)
                {
                    textMesh.text = before + (char)(0xac00 + code / 28 * 28);
                    source.PlayOneShot(typeSound);
                    yield return new WaitForSeconds(delay);
                }
                textMesh.text = before + c;
            }
            else
            {
                textMesh.text += c;
            }
            source.PlayOneShot(typeSound);
            yield return new WaitForSeconds(delay);
        }
    }
}
