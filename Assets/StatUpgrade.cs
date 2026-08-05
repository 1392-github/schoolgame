using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour
{
    public int id;
    public Text text;
    public InputField xpInput;
    public InputField chanceInput;
    public Button button;
    public long xp;
    public float chance;
    Player player;
    bool during;
    System.Reflection.PropertyInfo prop;
    public void Start2()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        prop = typeof(Player).GetProperty(GameData.statTypes[id].prop);
        UpdateText();
    }
    long GetCost() => (long)Mathf.Max(GameData.statTypes[id].reqBase * Mathf.Pow(GameData.statTypes[id].reqExp, GameData.stat[id]), 1);
    public void XpInputChange(string sxp)
    {
        if (during)
        {
            return;
        }
        during = true;
        if (sxp.Length > 0 && sxp[0] == '-')
        {
            sxp = sxp.Remove(0, 1);
            xpInput.text = sxp;
        }
        if (long.TryParse(sxp, out xp))
        {
            if (xp > GetCost())
            {
                xp = GetCost();
                xpInput.text = xp.ToString();
            }
            chance = (float)xp / GetCost();
            chanceInput.text = (chance * 100).ToString("0.##########");
        }
        during = false;
    }
    public void ChanceInputChance(string sch)
    {
        if (during)
        {
            return;
        }
        during = true;
        if (sch.Length > 0 && sch[0] == '-')
        {
            sch = sch.Remove(0, 1);
            chanceInput.text = sch;
        }
        if (float.TryParse(sch, out chance))
        {
            chance /= 100;
            if (chance > 1)
            {
                chance = 1;
                chanceInput.text = "100";
            }
            xp = Mathf.CeilToInt(GetCost() * chance);
            xpInput.text = xp.ToString();
            chance = (float)xp / GetCost();
        }
        during = false;
    }
    public void ChanceInputEnd()
    {
        during = true;
        chanceInput.text = (chance * 100).ToString("0.##########");
        during = false;
    }
    public void Upgrade()
    {
        if (xp < 0)
        {
            return;
        }
        if (GameData.end)
        {
            player.OpenDialog("이미 종료된 게임입니다");
            return;
        }
        if (GameData.exp < xp)
        {
            player.OpenDialog("XP가 부족합니다");
            return;
        }
        GameData.exp -= xp;
        object before = prop.GetValue(player);
        if (Random.Range(0f, 1f) <= chance)
        {
            GameData.stat[id]++;
            player.SendMessage($"{GameData.statTypes[id].name} 업그레이드에 성공했습니다 (Lv {GameData.stat[id] - 1} ({GameData.statTypes[id].prefix}{before}{GameData.statTypes[id].suffix}) → Lv {GameData.stat[id]} ({GameData.statTypes[id].prefix}{prop.GetValue(player)}{GameData.statTypes[id].suffix}))");
        }
        else
        {
            GameData.stat[id]--;
            player.SendMessage($"{GameData.statTypes[id].name} 업그레이드에 실패했습니다 (Lv {GameData.stat[id] + 1} ({GameData.statTypes[id].prefix}{before}{GameData.statTypes[id].suffix}) → Lv {GameData.stat[id]} ({GameData.statTypes[id].prefix}{prop.GetValue(player)}{GameData.statTypes[id].suffix}))");
        }
        GameData.statTypes[id].onUpgrade?.Invoke();
        UpdateText();
        ChanceInputChance(chanceInput.text);
        ChanceInputEnd();
    }
    public void UpdateText()
    {
        text.text = $"{GameData.statTypes[id].name} Lv {GameData.stat[id]} ({GameData.statTypes[id].prefix}{prop.GetValue(player)}{GameData.statTypes[id].suffix})";
        if (GameData.statTypes[id].max != 0 && GameData.stat[id] == GameData.statTypes[id].max)
        {
            button.interactable = false;
        }
    }
}
