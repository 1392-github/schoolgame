using System;
using System.Linq;

public static class GlobalEventManager
{
    public static Delegate[] events;
    public static void Register()
    {
        events = new Delegate[66];
        // 0~49: 문제집 사용
        for (int i = 0; i < 50; i++)
        {
            int i2 = i;
            events[i] = (Func<bool>)(() => ItemScripts.UseItem1(i2));
        }
        // 50~59: 문제집 설명 부가 정보
        for (int i = 0; i < 10; i++)
        {
            int i2 = i;
            events[i+50] = (Func<object[]>)(() => ItemScripts.Item1Desc(i2+1));
        }
        // 60: 대화의 학교명과 이름
        events[60] = (Func<object[]>)(() => new object[] {GameData.school, GameData.name});
        // 61: 대화의 반 번호
        events[61] = (Func<object[]>)(() => new object[] {GameData.clas+1});
        // 62: 대화의 학교명
        events[62] = (Func<object[]>)(() => new object[] {GameData.school});
        // 63: 이 세상의 교과서 1, 2, 3 잠금해제
        events[63] = (Action)(() =>
        {
            GameData.worldBookUnlocked[0] = true;
            GameData.worldBookUnlocked[1] = true;
            GameData.worldBookUnlocked[2] = true;
        });
        // 64: "너 왜 등록 안 해?"에서 합격한 대학 목록
        events[64] = (Func<object[]>)(() => new object[] {string.Join(", ", UniversityManager.appliedUniversities.Where((x, i) => x != -1 && UniversityManager.applicationPassed[i]).Select(x => UniversityManager.universities[x].name))});
        // 65: 이 세상의 교과서 4 잠금해제
        events[65] = (Action)(() => GameData.worldBookUnlocked[3] = true);
    }
}
