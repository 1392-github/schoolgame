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
    public int xp;
    public float chance;
    Player player;
    Data data;
    bool during;
    System.Reflection.PropertyInfo prop;
    public void Start2()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        data = GameObject.Find("Data").GetComponent<Data>();
        prop = typeof(Player).GetProperty(data.stat[id].prop);
        UpdateText();
    }
    int GetCost() => (int)(data.stat[id].reqBase * Mathf.Pow(data.stat[id].reqExp, player.stat[id]));
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
        if (int.TryParse(sxp, out xp))
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
        if (player.end)
        {
            player.OpenDialog("이미 종료된 게임입니다");
            return;
        }
        if (player.exp < xp)
        {
            player.OpenDialog("XP가 부족합니다");
            return;
        }
        player.exp -= xp;
        object before = prop.GetValue(player);
        if (Random.Range(0f, 1f) <= chance)
        {
            player.stat[id]++;
            player.SendMessage($"{data.stat[id].name} 업그레이드에 성공했습니다 (Lv {player.stat[id] - 1} ({before}) → Lv {player.stat[id]} ({prop.GetValue(player)}))");
        }
        else
        {
            player.stat[id]--;
            player.SendMessage($"{data.stat[id].name} 업그레이드에 실패했습니다 (Lv {player.stat[id] + 1} ({before}) → Lv {player.stat[id]} ({prop.GetValue(player)}))");
        }
        if (player.stat[id] < -20)
        {
            player.stat[id] = -20;
        }
        UpdateText();
        ChanceInputChance(chanceInput.text);
        ChanceInputEnd();
        player.updateShop();
        player.updateInventory();
    }
    public void UpdateText()
    {
        text.text = $"{data.stat[id].name} Lv {player.stat[id]} ({data.stat[id].prefix}{prop.GetValue(player)}{data.stat[id].suffix})";
        if (data.stat[id].max != 0 && player.stat[id] == data.stat[id].max)
        {
            button.interactable = false;
        }
    }
}
