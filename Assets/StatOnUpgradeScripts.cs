using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatOnUpgradeScripts
{
    public static Player player;
    public static void OnStudyUpgrade()
    {
        player.updateInventory();
        player.updateShop();
    }
    public static void OnQuestMaxUpgrade()
    {
        player.UpdateQuestCount();
    }
    public static void OnQuestTimeUpgrade()
    {
        player.UpdateNewQuest();
    }
}
