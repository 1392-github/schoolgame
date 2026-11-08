using System;
public static class ItemScripts
{
    public static HomeUIManager uiManager;
    public static object[] Item1Desc(int id)
    {
        int l = id % 10 + 1;
        float bonus = GameData.studyLvBonus * UniversityManager.GetStudyBonusSubject(id / 10);
        return new object[] { (int)(l * bonus), (int)(l * 2 * bonus) };
    }
    public static bool UseItem1(int id)
    {
        if (GameData.weekendOrVacation)
        {
            if (GameData.time.TimeOfDay >= new TimeSpan(7, 0, 0) && GameData.time.TimeOfDay <= new TimeSpan(8, 0, 0))
            {
                if (uiManager != null) uiManager.OpenDialog("이 아이템은 7시 ~ 8시까지 사용할 수 없습니다");
                return false;
            }
        }
        else
        {
            if (GameData.time.TimeOfDay >= new TimeSpan(7, 0, 0) && GameData.time.TimeOfDay <= new TimeSpan(14, 50, 0))
            {
                if (uiManager != null) uiManager.OpenDialog("이 아이템은 7시 ~ 14시 50분까지 사용할 수 없습니다");
                return false;
            }
        }
        int l = id % 10 + 1;
        GameData.giveStudyExp(id / 10, l, l * 2);
        GameData.time += UniversityManager.problemTime;
        if (uiManager != null) uiManager.UpdateTimeUI();
        return true;
    }
}
