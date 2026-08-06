using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatOnUpgradeScripts
{
    public static void OnStudyUpgrade()
    {
        GameData.player.updateInventory();
        GameData.player.updateShop();
    }
    public static void OnQuestMaxUpgrade()
    {
        GameData.player.UpdateQuestCount();
    }
    public static void OnQuestTimeUpgrade()
    {
        GameData.player.UpdateNewQuest();
    }
}
