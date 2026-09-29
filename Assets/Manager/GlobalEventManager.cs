using System;

public static class GlobalEventManager
{
    public static Delegate[] events;
    public static void Register()
    {
        events = new Delegate[64];
        for (int i = 0; i < 50; i++)
        {
            int i2 = i;
            events[i] = (Func<bool>)(() => ItemScripts.UseItem1(i2));
        }
        for (int i = 0; i < 10; i++)
        {
            int i2 = i;
            events[i+50] = (Func<object[]>)(() => ItemScripts.Item1Desc(i2+1));
        }
        events[60] = (Func<object[]>)(() => new object[] {GameData.school, GameData.name});
        events[61] = (Func<object[]>)(() => new object[] {GameData.clas+1});
        events[62] = (Func<object[]>)(() => new object[] {GameData.school});
        events[63] = (Action)(() =>
        {
            GameData.worldBookUnlocked[0] = true;
            GameData.worldBookUnlocked[1] = true;
            GameData.worldBookUnlocked[2] = true;
        });
    }
}
