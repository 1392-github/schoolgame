using System;
using System.Linq;
using UnityEngine;

public static class GlobalEventManager
{
    public static Delegate[] events;
    public static void Register()
    {
        events = new Delegate[73];
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
        events[60] = (Func<(object[], int)>)(() => (new object[] {GameData.school, GameData.name}, 0));
        // 61: 대화의 반 번호
        events[61] = (Func<(object[], int)>)(() => (new object[] {GameData.clas+1}, 0));
        // 62: 대화의 학교명
        events[62] = (Func<(object[], int)>)(() => (new object[] {GameData.school}, 0));
        // 63: 이 세상의 교과서 1, 2, 3 잠금해제
        events[63] = (Action)(() =>
        {
            GameData.worldBookUnlocked[0] = true;
            GameData.worldBookUnlocked[1] = true;
            GameData.worldBookUnlocked[2] = true;
        });
        // 64: "너 왜 등록 안 해?"에서 합격한 대학 목록
        events[64] = (Func<(object[], int)>)(() => (new object[] {string.Join(", ", UniversityManager.appliedUniversities.Where((x, i) => x != -1 && UniversityManager.applicationPassed[i]).Select(x => UniversityManager.universities[x].name))}, 0));
        // 65: 이 세상의 교과서 4 잠금해제
        events[65] = (Action)(() => GameData.worldBookUnlocked[3] = true);
        // 66 : 그럼 지금부터 제{0}회 {1} 졸업식을 시작하겠습니다.
        events[66] = (Func<(object[], int)>)(() => (new object[] {GameData.playthrough, GameData.school}, 0));
        // 67 : {0} {1} 학생, 박문원 학생, 이외 298명의 졸업을 허가합니다.
        events[67] = (Func<(object[], int)>)(() => (new object[] {GameData.school, GameData.name}, UniversityManager.finalUniversity == -1 ? 5 : 3));
        // 68: 그나저나 너 {0}대학교 합격했다며? 진짜 대박이다! / 그럼 지금부터 {0}대학교 입학식을 시작합니다!
        events[68] = (Func<(object[], int)>)(() => (new object[] {UniversityManager.universities[UniversityManager.finalUniversity].name}, 0));
        // 69: 졸업식 시작
        events[69] = (Action)(() => GameData.player.StartGraduation());
        // 70: 졸업식 "{0}년 2월 1일"
        events[70] = (Func<(object[], int)>)(() => (new object[] {GameData.startYear + 3}, 0));
        // 71: 졸업식 "안녕, {0}?"
        events[71] = (Func<(object[], int)>)(() => (new object[] {Util.Josa(GameData.name, "아", "야")}, 0));
        // 72: 졸업식 "나는 아쉽게 {0}대로 갔거든..."
        events[72] = (Func<(object[], int)>)(() => (new object[] {UniversityManager.universities[Mathf.Min(UniversityManager.finalUniversity + 2, UniversityManager.universities.Length - 1)].name}, 0));
    }
}
