using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatOnUpgradeScripts
{
    public static InventoryManager inventoryManager;
    public static void OnStudyUpgrade()
    {
        if (inventoryManager != null) inventoryManager.updateInventory();
    }
    //public static void OnQuestMaxUpgrade()
    //{
    //    GameData.player.UpdateQuestCount();
    //}
    //public static void OnQuestTimeUpgrade()
    //{
    //    GameData.player.UpdateNewQuest();
    //}
}
