using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    #region 저장 데이터
    public int exp;
    public int lv;
    public int save;
    public int money;
    public DateTime time;
    public TimeSpan timeSpeed;
    public bool inClass;
    public bool inSchool;
    public float moveSpeed;
    public string currentScene;
    public int mapArgs;
    public int[] studyExp;
    public List<TestScore> scores;
    public bool enableCheat;
    public int schedule;
    public bool[] achCompleted;
    public int[] clas;
    public bool duringClassPlacement;
    public DateTime startClassPlacement;
    public DateTime endClassPlacement;
    public List<TimeSpan> busStopTime;
    public DateTime nextBusStopTimeChange;
    public List<int> inventory;
    public int speed;
    public int repeatAll1;
    public int weeklyGoalSubject;
    public int weeklyGoalValue;
    public int weeklyGoalReward;
    public DateTime weeklyGoalTime;
    public bool weeklyGoalCompleted;
    public int[] stat;
    public List<Experimental> experimental;
    public DateTime startTime;
    public TimeSpan totalPlayTime;
    #endregion
    #region 저장 데이터가 아닌 변수들
    public int sbindex;
    public string saveName;
    //public int needExpForLvUP;
    //public int maxLevel;
    public GameObject gradeDoor;
    public bool tutorial;
    public TimeSpan blockTime;
    public int blockTimeSpace;
    bool left;
    bool right;
    bool up;
    bool down;
    Data data;
    Rigidbody2D rb;
    GameObject dialog;
    Text dialogText;
    ScrollRect msgScroll;
    Text msg;
    int msgScrollDown;
    GameObject classPlaceInput;
    Text classPlaceDDay;
    Text busStopTimeDisplay;
    Dropdown busStopDropdown;
    Dropdown busDirectionDropdown;
    public bool busDirection;
    TextMeshPro busLocD;
    Door busDoor;
    GameObject bus1;
    GameObject bus2;
    TimeSpan[] busTime1;
    TimeSpan[] busTime2;
    TimeSpan busBaseTime;
    TimeSpan busBaseTime1;
    TimeSpan busBaseTime2;
    bool mapInited;
    bool control;
    KeyCode[] moveKeys =
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow
    };
    float moneyFloat;
    Text lvInfo;
    GameObject problem;
    Text problemText;
    Image problemImage;
    string problemAnswer;
    Text speedDisplay;
    Text weeklyGoalDisplay;
    public GameObject itemContent;
    Transform inventoryDisplay;
    GameObject inventoryDisplay2;
    Transform canvas;
    object[] descExt;
    int cntProblemItem;
    bool itemRemove;
    TimeSpan problemTimer;
    Text problemTimerDisplay;
    InputField problemAnswerInput;
    public GameObject buyItemContent;
    Transform buyItemDisplay;
    GameObject buyItemDisplay2;
    float moneyFloat2;
    float xpFloat2;
    public int[,] sudoku;
    public Transform sudokuGrid;
    public GameObject sudokuText;
    public GameObject sudokuInput;
    public GameObject endingDisplay;
    public Text endingDisplayText;
    public Button endButton;
    public Text sudokuTimer;
    TextMeshPro busStopText1;
    TextMeshPro busStopText2;
    public Text xpDisplay;
    public Text xpDisplay2;
    public Transform upgradeScroll;
    public GameObject upgradeButton;
    public GameObject menu;
    public AchGenerator achGen;
    public int currentChat;
    public int currentChatElement;
    public int nextChatElement;
    public GameObject chatDisplay;
    public Text chatTitleText;
    public Text chatContentText;
    public GameObject optionButton;
    public Transform chatOption;
    bool enableNext;
    string actualSceneName;
    string doorName;
    Direction2 doorDirection;
    #endregion
    #region 스탯 정보 속성
    public float studyLvBonus
    {
        get
        {
            if (stat[0] <= 18)
            {
                return Mathf.Pow(1.25f, stat[0]);
            }
            else
            {
                return 55 * Mathf.Pow(1.02f, stat[0] - 18);
            }
        }
    }
    public int LvIncome => (int)(9860 * Mathf.Pow(1.02f, stat[1]));
    public float nextDayStudyExp => 1 - 0.05f * Mathf.Pow(0.98f, stat[2]);
    public int classPlacementChance => Mathf.Clamp(10 + stat[3] * 3, 10, 100);
    public float problemTime => 60 * Mathf.Pow(0.99f, stat[4]);
    #endregion
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        data = GameObject.Find("Data").GetComponent<Data>();
        saveName = GameObject.Find("SaveData").GetComponent<SaveBuffer>().name;
        #region 저장 데이터 불러오기
        SaveFile1 save = GameObject.Find("SaveData").GetComponent<SaveBuffer>().save;
        time = DateTime.ParseExact(save.time, "yyyy-MM-dd HH:mm:ss", null);
        timeSpeed = TimeSpan.Parse(save.timeSpeed);
        exp = save.exp;
        lv = save.level;
        money = save.money;
        studyExp = save.studyExp;
        scores = save.scores;
        schedule = save.schindex;
        inClass = save.inclass;
        inSchool = save.inschool;
        clas = save.clas;
        inventory = save.inventory;
        tutorial = GameObject.Find("SaveData").GetComponent<SaveBuffer>().tutorial;
        achCompleted = save.achCompleted;
        if (achCompleted.Length < data.achievement.Count)
        {
            achCompleted = achCompleted.Concat(new bool[data.achievement.Count - achCompleted.Length]).ToArray();
        }
        duringClassPlacement = save.duringClassPlacement;
        startClassPlacement = DateTime.ParseExact(save.startClassPlacement, "yyyy-MM-dd", null);
        endClassPlacement = DateTime.ParseExact(save.endClassPlacement, "yyyy-MM-dd", null);
        busStopTime = save.busStopTime.Select(c => TimeSpan.ParseExact(c, "hh\\:mm", null)).ToList();
        nextBusStopTimeChange = DateTime.ParseExact(save.nextBusStopTimeChange, "yyyy-MM-dd", null);
        speed = save.speed;
        repeatAll1 = save.repeatAll1;
        experimental = save.experimental;
        Move(save.map, save.mapextra, new Vector3(save.x, save.y, 0));
        weeklyGoalSubject = save.weeklyGoalSubject;
        weeklyGoalValue = save.weeklyGoalValue;
        weeklyGoalReward = save.weeklyGoalReward;
        weeklyGoalTime = DateTime.ParseExact(save.weeklyGoalTime, "yyyy-MM-dd", null);
        weeklyGoalCompleted = save.weeklyGoalCompleted;
        stat = save.stat;
        if (stat.Length < data.stat.Count)
        {
            stat = stat.Concat(new int[data.stat.Count - stat.Length]).ToArray();
        }
        startTime = DateTime.ParseExact(save.startTime, "yyyy-MM-dd HH:mm:ss", null);
        totalPlayTime = TimeSpan.ParseExact(save.totalPlayTime, "d\\:hh\\:mm\\:ss", null);
        #endregion
        Destroy(GameObject.Find("SaveData"));
        SaveBuffer.generated = false;
        canvas = GameObject.Find("Canvas").transform;
        dialog = canvas.Find("Dialog").gameObject;
        dialogText = dialog.transform.Find("DialogText").Find("Viewport").Find("Content").GetComponent<Text>();
        msgScroll = canvas.Find("MsgScroll").GetComponent<ScrollRect>();
        msg = msgScroll.transform.Find("Viewport").Find("Msg").GetComponent<Text>();
        achGen.Start2();
        if (clas[0] == -1)
        {
            if (ExperimentalCheck(Experimental.FRIEND_SYSTEM))
            {
                for (int i = 0; i < 330; i++)
                {
                    do
                    {
                        clas[i] = Random.Range(0, 10);
                    } while (clas.Count(c => c == clas[i]) > 33);
                }
                for (int i = 330; i < 660; i++)
                {
                    do
                    {
                        clas[i] = Random.Range(10, 20);
                    } while (clas.Count(c => c == clas[i]) > 33);
                }
                for (int i = 660; i < 1000; i++)
                {
                    do
                    {
                        clas[i] = Random.Range(20, 30);
                    } while (clas.Count(c => c == clas[i]) > 34);
                }
            }
            else
            {
                clas[0] = Random.Range(0, 10);
            }
        }
        classPlaceInput = canvas.Find("ClassPlaceInput").gameObject;
        classPlaceDDay = canvas.Find("Menu").Find("GetClass").Find("ChangeDDay").GetComponent<Text>();
        busStopTimeDisplay = canvas.Find("BusStopTime").Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>();
        busStopDropdown = canvas.Find("BusStopTime").Find("Dropdown (Legacy)").GetComponent<Dropdown>();
        busDirectionDropdown = canvas.Find("BusStopTime").Find("Dropdown (Legacy) (1)").GetComponent<Dropdown>();
        UpdateDDay();
        int r;
        if (scores.Count == 0)
        {
            r = 9;
        }
        else
        {
            r = (int)Math.Ceiling(scores[^1].grade.Average());
        }
        if (r <= 3)
        {
            blockTime = TimeSpan.Zero;
        }
        else
        {
            blockTime = new TimeSpan(1, 0, 0, 0) - new TimeSpan(0, blockTimeSpace, 0) * (r - 4);
        }
        if (busStopTime.Count == 0)
        {
            ChangeBusStopTime();
        }
        busStopDropdown.options = data.busStop.Select(c => new Dropdown.OptionData(c.name)).ToList();
        busDirectionDropdown.options[0].text = data.busStop[^1].name + " 방향";
        busDirectionDropdown.options[1].text = data.busStop[0].name + " 방향";
        lvInfo = canvas.Find("LvInfo").Find("Lv").GetComponent<Text>();
        inventoryDisplay2 = canvas.Find("Inventory").gameObject;
        inventoryDisplay = inventoryDisplay2.transform.Find("Scroll View").Find("Viewport").Find("Content");
        descExt = new object[0];
        //UpdateLv();
        problem = canvas.Find("Problem").gameObject;
        problemText = problem.transform.Find("Text").Find("Viewport").Find("Content").GetComponent<Text>();
        problemImage = problem.transform.Find("Image").GetComponent<Image>();
        problemTimerDisplay = problem.transform.Find("Timer").GetComponent<Text>();
        problemAnswerInput = problem.transform.Find("Answer").GetComponent<InputField>();
        speedDisplay = GameObject.Find("SpeedDisplay").GetComponent<Text>();
        weeklyGoalDisplay = GameObject.Find("WeeklyGoalText").GetComponent<Text>();
        if (weeklyGoalSubject == -1)
        {
            weeklyGoalSubject = Random.Range(0, 5);
            weeklyGoalValue = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 20, int.MaxValue) * Random.Range(1.2f, 1.5f));
            weeklyGoalReward = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 500, int.MaxValue) * Random.Range(0.3f, 0.5f));
        }
        updateWeeklyGoalDisplay();
        cntProblemItem = -1;
        buyItemDisplay2 = canvas.Find("Shop").gameObject;
        buyItemDisplay = buyItemDisplay2.transform.Find("Scroll View").Find("Viewport").Find("Content");
        updateShop();
        sudoku = new int[6, 6];
        for (int i = 0; i < data.stat.Count; i++)
        {
            if (data.stat[i].experimental != Experimental.NONE && !ExperimentalCheck(data.stat[i].experimental))
            {
                continue;
            }
            GameObject g = Instantiate(upgradeButton);
            g.transform.SetParent(upgradeScroll, false);
            StatUpgrade u = g.GetComponent<StatUpgrade>();
            u.id = i;
            u.Start2();
            u.UpdateText();
        }
        updateInventory();
    }
    void Update()
    {
        time += timeSpeed * Time.deltaTime * speed;
        totalPlayTime += new TimeSpan(0, 0, 1) * Time.deltaTime;
        if (enableCheat)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                canvas.Find("Cheat").gameObject.SetActive(true);
            }
        }
        if (control)
        {
            float x = Input.GetAxis("Horizontal") * moveSpeed;
            float y = Input.GetAxis("Vertical") * moveSpeed;
            if (left)
            {
                x = -moveSpeed;
            }
            if (right)
            {
                x = moveSpeed;
            }
            if (up)
            {
                y = moveSpeed;
            }
            if (down)
            {
                y = -moveSpeed;
            }
            rb.velocity = new Vector3(x * speed, y * speed, 0);
        }
        else
        {
            control = true;
            foreach (KeyCode k in moveKeys)
            {
                if (Input.GetKey(k))
                {
                    control = false;
                    break;
                }
            }
            if (control)
            {
                Input.ResetInputAxes();
            }
            rb.velocity = Vector3.zero;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 11f);
            if (hit)
            {
                hit.transform.GetComponent<Clickable>()?.onClick?.Invoke();
            }
        }
        if (tutorial)
        {
            if (TimeSpan.Parse(data.tutorialSchedule[schedule].time) < time.TimeOfDay)
            {
                if (TimeSpan.Parse(data.tutorialSchedule[schedule].time) > new TimeSpan(8, 0, 0) || time.TimeOfDay < new TimeSpan(14, 0, 0))
                {
                    data.tutorialSchedule[schedule].work.Invoke();
                    schedule++;
                }
            }
        }
        else
        {
            if (TimeSpan.Parse(data.schedule[schedule].time) < time.TimeOfDay)
            {
                if (TimeSpan.Parse(data.schedule[schedule].time) > new TimeSpan(8, 0, 0) || time.TimeOfDay < new TimeSpan(14, 0, 0))
                {
                    data.schedule[schedule].work.Invoke();
                    schedule++;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            msgScroll.gameObject.SetActive(!msgScroll.gameObject.activeSelf);
        }
        if (mapInited)
        {
            if (currentScene == "BusStop")
            {
                TimeSpan t = time.TimeOfDay;
                bool b1 = true;
                bool b2 = true;
                for (int i = 0; i < busTime1.Length; i++)
                {
                    TimeSpan bt1 = busTime1[i];
                    TimeSpan bt2 = busTime2[i];
                    if (b1)
                    {
                        if (t >= bt1 && t <= bt1 + new TimeSpan(0, 5, 0))
                        {
                            bus1.SetActive(true);
                            busBaseTime1 = bt1;
                            b1 = false;
                        }
                        else
                        {
                            bus1.SetActive(false);
                            //busStopText1.text = "";
                        }
                    }
                    if (b2)
                    {
                        if (t >= bt2 && t <= bt2 + new TimeSpan(0, 5, 0))
                        {
                            bus2.SetActive(true);
                            busBaseTime2 = bt2;
                            b2 = false;
                        }
                        else
                        {
                            bus2.SetActive(false);
                        }
                    }
                }
            }
            if (currentScene == "Bus")
            {
                if (mapArgs == (busDirection ? 0 : data.busStop.Count - 1))
                {
                    busDoor.gameObject.SetActive(true);
                    return;
                }
                TimeSpan t = time.TimeOfDay;
                if (t >= busBaseTime && t <= busBaseTime + new TimeSpan(0, 5, 0))
                {
                    busDoor.gameObject.SetActive(true);
                }
                if (t >= busBaseTime + new TimeSpan(0, 5, 0) && t <= busBaseTime + new TimeSpan(0, 20, 0))
                {
                    busDoor.gameObject.SetActive(false);
                }
                if (t >= busBaseTime + new TimeSpan(0, 20, 0))
                {
                    mapArgs += busDirection ? -1 : 1;
                    busBaseTime += new TimeSpan(0, 20, 0);
                    money -= 100;
                }
                busDoor.args = mapArgs;
                busLocD.text = $"{data.busStop[mapArgs].name}\n({data.busStop[busDirection ? 0 : ^1].name} →)";
            }
            if (currentScene == "Unnamed3")
            {
                moneyFloat += (float)(speed * timeSpeed).TotalHours * LvIncome * Time.deltaTime;
                int i = Mathf.FloorToInt(moneyFloat);
                moneyFloat -= i;
                money += i;
            }
            if (currentScene == "DormitoryRoom" && Input.GetKeyDown(KeyCode.N) && !inSchool)
            {
                timeSpeed = new TimeSpan(1, 0, 0);
            }
        }
        speedDisplay.text = $"Speed = {speed}";
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (speed == 0)
                {
                    speed = 1;
                }
                else
                {
                    speed *= 2;
                }
            }
            else
            {
                speed += 1;
            }
            if (speed > 100)
            {
                speed = 100;
            }
            Time.fixedDeltaTime = 0.02f / speed;
        }
        if (Input.GetKeyDown(KeyCode.Minus) && speed > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed /= 2;
            }
            else
            {
                speed -= 1;
            }
            if (speed == 0)
            {
                Time.fixedDeltaTime = 0.02f;
            }
            else
            {
                Time.fixedDeltaTime = 0.02f / speed;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryDisplay2.SetActive(true);
        }
        if (cntProblemItem != -1)
        {
            problemTimer -= timeSpeed * Time.deltaTime * speed;
            TimeSpan realTime = problemTimer / timeSpeed.TotalSeconds;
            problemTimerDisplay.text = $"{Math.Floor(realTime.TotalMinutes)}:{realTime.Seconds} ({Math.Floor(problemTimer.TotalMinutes)}:{problemTimer.Seconds})";
            if (problemTimer <= TimeSpan.Zero)
            {
                if (problemAnswerInput.text == problemAnswer)
                {
                    int l = cntProblemItem % 10 + 1;
                    giveStudyExp(cntProblemItem / 10, l, l * 2);
                }
                else
                {
                    giveStudyExp(cntProblemItem / 10, -20, -10);
                }
                problem.SetActive(false);
                cntProblemItem = -1;
                timeSpeed = new TimeSpan(0, 0, 30);
            }
        }
        xpDisplay.text = $"{exp} XP";
        xpDisplay2.text = $"{exp} XP";
        //if (blockTime != TimeSpan.Zero && currentScene != "DormitoryRoom" && (time.TimeOfDay < new TimeSpan(8, 0, 0) || time.TimeOfDay > blockTime))
        //{
        //    xpFloat2 += (float)needExpForLvUP / 2 * (float)(speed * timeSpeed).TotalHours * Time.deltaTime;
        //    int a = (int)xpFloat2;
        //    xpFloat2 %= 1;
        //    exp -= a;
        //    if (lv == maxLevel)
        //    {
        //        lvInfo.text = $"Lv {lv + 1} ({exp})\n공부 효율 {studyLvBonus}\n수입 {LvIncome}원/1시간\n반배정 성공확률 100%\n수업 1번 들을 시 능력치 1~{(int)(10 * studyLvBonus)} 상승";
        //    }
        //    else
        //    {
        //        lvInfo.text = $"Lv {lv + 1} ({exp}/{needExpForLvUP})\n공부 효율 {studyLvBonus}\n수입 {LvIncome}원/1시간\n반배정 성공확률 {Mathf.Clamp(lv + 10, 10, 100)}%\n수업 1번 들을 시 능력치 1~{(int)(10 * studyLvBonus)} 상승";
        //    }
        //    moneyFloat2 += LvIncome * 2 * (float)(speed * timeSpeed).TotalHours * Time.deltaTime;
        //    a = (int)moneyFloat2;
        //    moneyFloat2 %= 1;
        //    money -= a;
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            endingDisplay.SetActive(true);
            int ach = 0;
            for (int i = 0; i < data.achievement.Count; i++)
            {
                if (!data.achievement[i].excludeEnding && achCompleted[i])
                {
                    ach++;
                }
            }
            int achSum = data.achievement.Count(c => !c.excludeEnding);
            int studyExpSum = studyExp.Sum();
            endingDisplayText.text = $@"엔딩 조건
<color={(ach == achSum ? "green" : "red")}>1 - 모든 업적 달성 (THE END, 이름없는업적15 제외) ({ach}/14)</color>
<color={(lv == 99 ? "green" : "red")}>2 - 레벨 100 달성 ({lv + 1}/100)</color>
<color={(money >= 10000000 ? "green" : "red")}>3 - 돈 10000000 달성 ({money}/10000000)</color>
<color={(studyExpSum >= 7500000 ? "green" : "red")}>4 - 모든 과목 능력치 합계 7500000 이상 ({studyExpSum}/7500000)</color>
(엔딩을 볼 시 자동으로 저장되며, 엔딩을 본 이후에도 해당 파일을 계속 플레이할 수 있습니다)";
            endButton.interactable = ach == achSum && lv == 99 && money >= 10000000 && studyExpSum >= 7500000;
        }
        GameObject selectedGameobject = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
        if (selectedGameobject == null || selectedGameobject.GetComponent<InputField>() == null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    speed = i;
                    if (speed == 0)
                    {
                        Time.fixedDeltaTime = 0.02f;
                    }
                    else
                    {
                        Time.fixedDeltaTime = 0.02f / speed;
                    }

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
        }
        if (speed == 0 && cntProblemItem != -1)
        {
            speed = 1;
        }
    }
    void LateUpdate()
    {
        if (msgScrollDown > 0)
        {
            msgScrollDown--;
            if (msgScrollDown == 0)
            {
                msgScroll.verticalNormalizedPosition = 0;
            }
        }
    }
    void Save()
    {
        SaveFile1 save = new SaveFile1();
        save.version = 1;
        save.time = time.ToString("yyyy-MM-dd HH:mm:ss");
        save.timeSpeed = timeSpeed.ToString();
        save.exp = exp;
        save.level = lv;
        save.money = money;
        save.studyExp = studyExp;
        save.map = currentScene;
        save.mapextra = mapArgs;
        save.scores = scores;
        save.x = transform.position.x;
        save.y = transform.position.y;
        save.schindex = schedule;
        save.inclass = inClass;
        save.inschool = inSchool;
        save.achCompleted = achCompleted;
        save.clas = clas;
        save.duringClassPlacement = duringClassPlacement;
        save.startClassPlacement = startClassPlacement.ToString("yyyy-MM-dd");
        save.endClassPlacement = endClassPlacement.ToString("yyyy-MM-dd");
        save.busStopTime = busStopTime.Select(c => c.ToString("hh\\:mm")).ToList();
        save.nextBusStopTimeChange = nextBusStopTimeChange.ToString("yyyy-MM-dd");
        save.inventory = inventory;
        save.speed = speed;
        save.repeatAll1 = repeatAll1;
        save.weeklyGoalSubject = weeklyGoalSubject;
        save.weeklyGoalValue = weeklyGoalValue;
        save.weeklyGoalReward = weeklyGoalReward;
        save.weeklyGoalTime = weeklyGoalTime.ToString("yyyy-MM-dd");
        save.weeklyGoalCompleted = weeklyGoalCompleted;
        save.stat = stat;
        save.experimental = experimental;
        save.startTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
        save.totalPlayTime = totalPlayTime.ToString("d\\:hh\\:mm\\:ss");
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{saveName}", JsonUtility.ToJson(save));
    }
    public void StartDay()
    {
        //Move("DormitoryRoom", 0, Vector3.zero);
        if (tutorial)
        {
            if (time.Date == new DateTime(2024, 3, 4))
            {
                OpenDialog("학교의 1학년 1반 교실에 들어가세요, 8시 50분까지 들어가지 않으면 100XP와 10000원이 깍입니다");
            }
            else
            {
                GiveExp(150);
                OpenDialog("XP를 모으면 레벨이 오르며, 레벨은 공부 효율, 수입, 반배정 확률에 영향을 미칩니다\n자세한 내용은 '1392year.pythonanywhere.com/w/123456/레벨'을 참고하세요");
            }
        }
        /*if (!tutorial && time.Date != new DateTime(2024, 3, 4))
        {
            GiveExp(Random.Range(50, 151));
        }*/
        schedule = -1;
        studyExp = studyExp.Select(c => c > 0 ? (int)(c * nextDayStudyExp) : c).ToArray();
        inSchool = true;
        timeSpeed = new TimeSpan(0, 1, 0);
        if (time.Date == endClassPlacement)
        {
            duringClassPlacement = false;
        }
        if (time.Date == nextBusStopTimeChange)
        {
            nextBusStopTimeChange = new DateTime(time.Date.Month == 12 ? time.Date.Year + 1 : time.Date.Year, time.Date.Month == 12 ? 1 : time.Month + 1, 1);
            ChangeBusStopTime();
        }
        //if (time.Date == weeklyGoalTime)
        //{
        //    weeklyGoalSubject = Random.Range(0, 5);
        //    weeklyGoalValue = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 20, int.MaxValue) * Random.Range(1.2f, 1.5f));
        //    weeklyGoalReward = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 100, int.MaxValue) * Random.Range(0.3f, 0.5f));
        //    updateWeeklyGoalDisplay();
        //}
    }
    public void Test()
    {
        TestScore rslt = new TestScore();
        rslt.date = time.ToString("yyyy-MM-dd");
        for (int i = 0; i < 5; i++)
        {
            rslt.grade[i] = 14;
            for (int j = 0; j < 13; j++)
            {
                if (studyExp[i] > data.NeedExpForGrade[j+1])
                {
                    if (Random.Range(data.NeedExpForGrade[j+1], data.NeedExpForGrade[j]+1) < studyExp[i])
                    {
                        rslt.grade[i] = j + 1;
                    }
                    else
                    {
                        rslt.grade[i] = j + 2;
                    }
                    break;
                }
            }
            rslt.rank[i] = Random.Range(data.gradeRank[rslt.grade[i]-1], data.gradeRank[rslt.grade[i]]);
            rslt.grade[i] = Mathf.Clamp(rslt.grade[i] - 5, 1, 9);
        }
        double avgRank = rslt.grade.Average();
        for (int j = 1; j < 9; j++)
        {
            if (avgRank <= 9 - j)
            {
                GiveAch(j);
            }
        }
        if (avgRank == 1)
        {
            repeatAll1++;
            achGen.AchRegen();
            if (repeatAll1 == 10)
            {
                GiveAch(9);
            }
        }
        else
        {
            repeatAll1 = 0;
            achGen.AchRegen();
        }
        scores.Add(rslt);
        int r = (int)Math.Ceiling(avgRank);
        if (r <= 3)
        {
            blockTime = TimeSpan.Zero;
        }
        else
        {
            blockTime = new TimeSpan(1, 0, 0, 0) - new TimeSpan(0, blockTimeSpace, 0) * (r - 4);
        }
        if (rslt.rank.All(c => c == 1))
        {
            GiveAch(13);
        }
        if (rslt.rank.Any(c => c == 500))
        {
            GiveAch(14);
        }
    }
    public void StartClass()
    {
        if (tutorial)
        {
            if (time.Date == new DateTime(2024, 3, 4))
            {
                OpenDialog("수업이 시작하면 국어,수학,사회,과학,영어 중 랜덤으로 1과목이 1~500(레벨1 기준) 사이에서 오릅니다");
            }
            else
            {
                OpenDialog("매주 월요일 (2024년 3월 4일은 제외, 튜토리얼에서는 3월 5일)에 시험을 봅니다");
            }
        }
        inClass = true;
        timeSpeed = new TimeSpan(0, 10, 0);
        if (currentScene == "Classroom" && mapArgs == clas[0])
        {
            giveStudyExp(Random.Range(0, 5), 1, 10);
        }
        else
        {
            GiveExp(-3000);
            money -= LvIncome * 8;
        }
    }
    public void EndClass()
    {
        timeSpeed = new TimeSpan(0, 0, 5);
        inClass = false;
    }
    public void EndSchool()
    {
        if (tutorial && time.Date == new DateTime(2024, 3, 5))
        {
            timeSpeed = TimeSpan.Zero;
        }
        else
        {
            timeSpeed = new TimeSpan(0, 0, 30);
            //timeSpeed = new TimeSpan(3, 0, 0);
        }
        if (time.DayOfWeek == DayOfWeek.Monday && time.Date != new DateTime(2024, 3, 4) || (tutorial && time.Date == new DateTime(2024, 3, 5))) 
        {
            Test();
        }
        if (tutorial)
        {
            if (time.Date == new DateTime(2024, 3, 4))
            {
                OpenDialog("시간표는 다음과 같습니다 (튜토리얼에는 1교시만 있음)\n1교시 08:50~09:35\n2교시 09:45~10:30\n3교시 10:40~11:25\n4교시 11:35~12:20\n5교시 13:10~13:55\n6교시 14:05~14:50");
            }
            else
            {
                OpenDialog("시험 등급은 능력치에 따라 결정되며, 자세한 내용은 '1392year.pythonanywhere.com/w/123456/시험 등급'을 참고하세요\n능력치가 표시되는 부분을 클릭하면 성적표를 볼 수 있습니다");
            }
        }
        inSchool = false;
        if (time.Date == startClassPlacement)
        {
            duringClassPlacement = true;
            endClassPlacement = startClassPlacement + new TimeSpan(3, 0, 0, 0);
            int y = time.Month == 12 ? time.Year + 1 : time.Year;
            int m = time.Month == 12 ? 1 : time.Month + 1;
            startClassPlacement = new DateTime(y, m, DateTime.DaysInMonth(y, m));
            while (startClassPlacement.DayOfWeek != DayOfWeek.Friday)
            {
                startClassPlacement -= new TimeSpan(1, 0, 0, 0);
            }
            if (ExperimentalCheck(Experimental.FRIEND_SYSTEM))
            {
                classPlaceInput.SetActive(true);
                timeSpeed = TimeSpan.Zero;
            }
            else
            {
                clas[0] = Random.Range(0, 10);
            }
        }
    }
    public void GiveExp(int amount)
    {
        exp += amount;
        SendMessage($"{amount} 경험치를 획득했습니다");
        /*while (lv < maxLevel && exp >= needExpForLvUP)
        {
            exp -= needExpForLvUP;
            lv++;
            SendMessage($"레벨 {lv+1}을 달성했습니다");
            UpdateLv();
        }*/
    }
    public void Move(string name, int args, Vector3 pos, string door = "", Direction2 doorDirection = 0)
    {
        if (name == "Main1F")
        {
            GiveAch(0);
        }
        if (name == "Bus")
        {
            money -= 1000;
        }
        if (name != currentScene || args != mapArgs)
        {
            mapInited = false;
            if (!string.IsNullOrEmpty(currentScene))
            {
                SceneManager.UnloadSceneAsync(actualSceneName);
            }
            actualSceneName = ExperimentalCheck(Experimental.NEW_MAP) && SceneUtility.GetBuildIndexByScenePath($"Assets/Map/{name}.unity") != -1 ? name : "Old" + name;
            SceneManager.LoadScene(actualSceneName, LoadSceneMode.Additive);
            mapArgs = args;
        }
        currentScene = name;
        transform.position = pos;
        control = false;
        doorName = door;
        this.doorDirection = doorDirection;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
        {
            return;
        }
        SceneManager.SetActiveScene(scene);
        if (scene.name == "Main1F") // 본관 복도 생성
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject door = Instantiate(gradeDoor);
                door.transform.position = new Vector3(i * 3 + 1, 1.4f, 0);
                door.transform.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"{mapArgs + 1}-{i + 1}";
                door.transform.Find("Door (2)").GetComponent<Door>().args = mapArgs * 10 + i;
            }
            if (mapArgs == 2)
            {
                Destroy(GameObject.Find("Door (3)"));
                Destroy(GameObject.Find("Text (TMP) (1)"));
            }
            else
            {
                GameObject.Find("Door (3)").GetComponent<Door>().args = mapArgs + 1;
                GameObject.Find("Text (TMP) (1)").GetComponent<TextMeshPro>().text = $"↑ {mapArgs + 2}층";
            }
            if (mapArgs == 0)
            {
                GameObject.Find("Door (1)").GetComponent<Door>().map = "BusStop";
                GameObject.Find("Door (1)").GetComponent<Door>().args = 1;
                GameObject.Find("Door (1)").GetComponent<Door>().pos = new Vector3(0.6f, 1, 0);
                Destroy(GameObject.Find("Text (TMP)"));
            }
            else
            {
                GameObject.Find("Door (1)").GetComponent<Door>().args = mapArgs - 1;
                GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"↓ {mapArgs}층";
            }
        }
        if (scene.name == "Classroom") // 교실 생성
        {
            GameObject.Find("Square (3)").GetComponent<Door>().args = mapArgs / 10;
            GameObject.Find("Square (3)").GetComponent<Door>().pos = new Vector3(1 + mapArgs % 10 * 3, 0.8f, 0);
        }
        if (scene.name == "Dormitory1F")
        {
            //GameObject.Find("Square (4)").GetComponent<OpenGUIButton>().target = canvas.Find("PC").gameObject;
            //GameObject.Find("Square (4)").GetComponent<CPBlockCall>().c = canvas.Find("PC").GetComponent<CPBlock>();
            #region 통금 시간 (임시 비활성화)
            //int r;
            //if (scores.Count == 0)
            //{
            //    r = 9;
            //}
            //else
            //{
            //    r = (int)Math.Ceiling(scores[^1].grade.Average()); 
            //}
            //string msg;
            //if (r <= 3)
            //{
            //    msg = "외출 금지 시간\n<b>1~3등급 없음</b>";
            //}
            //else
            //{
            //    msg = "외출 금지 시간\n1~3등급 없음";
            //}
            //TimeSpan t = new TimeSpan(1, 0, 0, 0);
            //for (int i = 4; i <= 9; i++)
            //{
            //    if (i > 4)
            //    {
            //        t -= new TimeSpan(0, blockTimeSpace, 0);
            //    }
            //    string display = i == 4 ? "24:00" : t.ToString("hh\\:mm");
            //    if (r == i)
            //    {
            //        msg += $"\n<b>{i}등급 {display}~08:00</b>";
            //    }
            //    else
            //    {
            //        msg += $"\n{i}등급 {display}~08:00";
            //    }
            //}
            //GameObject.Find("Square (5)").GetComponent<MapSendMsg>().msg = msg;
            //if (r <= 3)
            //{
            //    GameObject.Find("Text (TMP) (3)").GetComponent<TextMeshPro>().text = "외출 금지 시간\n--------";
            //}
            //else if (r == 4)
            //{
            //    GameObject.Find("Text (TMP) (3)").GetComponent<TextMeshPro>().text = "외출 금지 시간\n24:00~08:00";
            //}
            //else
            //{
            //    GameObject.Find("Text (TMP) (3)").GetComponent<TextMeshPro>().text = $"외출 금지 시간\n{new TimeSpan(1, 0, 0, 0) - new TimeSpan(0, blockTimeSpace, 0) * (r - 4):hh\\:mm}~08:00";
            //}
            #endregion
        }
        if (scene.name == "BusStop")
        {
            if (mapArgs == 0)
            {
                GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"<size=5><b>{data.busStop[0].name}</b></size> → {data.busStop[1].name}";
            }
            else if (mapArgs == data.busStop.Count - 1)
            {
                GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"{data.busStop[^2].name} ← <size=5><b>{data.busStop[^1].name}</b></size>";
            }
            else
            {
                GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"{data.busStop[mapArgs - 1].name} ← <size=5><b>{data.busStop[mapArgs].name}</b></size> → {data.busStop[mapArgs + 1].name}";
            }
            GameObject.Find("Square (6)").GetComponent<Door>().map = data.busStop[mapArgs].map;
            GameObject.Find("Square (6)").GetComponent<Door>().args = data.busStop[mapArgs].extra;
            GameObject.Find("Square (6)").GetComponent<Door>().pos = data.busStop[mapArgs].loc;
            bus1 = GameObject.Find("Square (7)");
            bus1.GetComponent<Door>().args = mapArgs;
            bus2 = GameObject.Find("Square (8)");
            bus2.GetComponent<Door>().args = mapArgs + 65536;
            busTime1 = GetBusTime(mapArgs, false);
            busTime2 = GetBusTime(mapArgs, true);
            GameObject.Find("Text (TMP) (1)").GetComponent<TextMeshPro>().text = $"{data.busStop[0].name} 방향";
            GameObject.Find("Text (TMP) (2)").GetComponent<TextMeshPro>().text = $"{data.busStop[^1].name} 방향";
        }
        if (scene.name == "Bus")
        {
            if (mapArgs >= 65536)
            {
                mapArgs -= 65536;
                busDirection = true;
                busBaseTime = busBaseTime2;
            }
            else
            {
                busDirection = false;
                busBaseTime = busBaseTime1;
            }
            busDoor = GameObject.Find("Square (5)").GetComponent<Door>();
            busLocD = GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>();
        }
        mapInited = true;
        if (doorName != "")
        {
            GameObject d = GameObject.Find(doorName);
            switch (doorDirection)
            {
                case Direction2.left:
                    transform.position = d.transform.position + Vector3.left;
                    break;
                case Direction2.right:
                    transform.position = d.transform.position + Vector3.right;
                    break;
                case Direction2.up:
                    transform.position = d.transform.position + Vector3.up;
                    break;
                case Direction2.down:
                    transform.position = d.transform.position + Vector3.down;
                    break;
            }
        }
    }
    public void PrevScore()
    {
        if (sbindex == 0)
        {
            sbindex = scores.Count - 1;
        }
        else
        {
            sbindex--;
        }
    }
    public void NextScore()
    {
        if (sbindex == scores.Count - 1)
        {
            sbindex = 0;
        }
        else
        {
            sbindex++;
        }
    }
    public void Left(bool hold)
    {
        left = hold;
    }
    public void Right(bool hold)
    {
        right = hold;
    }
    public void Up(bool hold)
    {
        up = hold;
    }
    public void Down(bool hold)
    {
        down = hold;
    }
    public void OpenDialog(string text)
    {
        dialog.SetActive(true);
        dialog.transform.SetAsLastSibling();
        dialogText.text = text;
    }
    public void SendMessage(string text)
    {
        if (msgScroll.verticalNormalizedPosition <= 0.001f)
        {
            msgScrollDown = 2;
        }
        msg.text += "\n" + text;
    }
    public void GiveAch(int id)
    {
        if (achCompleted[id])
        {
            return;
        }
        SendMessage($"업적 {data.achievement[id].name}을 달성했습니다");
        GiveExp(data.achievement[id].exp);
        achCompleted[id] = true;
        achGen.AchRegen();
    }
    public void UpdateDDay()
    {
        classPlaceDDay.text = $"다음 반배정 : {startClassPlacement:yyyy-MM-dd} (D-{(startClassPlacement - time.Date).TotalDays})";
        LoadBusTime();
    }
    void ChangeBusStopTime()
    {
        busStopTime.Clear();
        busStopTime.Add(new TimeSpan(8, 10, 0));
        bool nextDay = false;
        while (true)
        {
            TimeSpan next = busStopTime[^1] + new TimeSpan(0, Random.Range(30, 60), 0);
            if (next >= new TimeSpan(1, 0, 0, 0))
            {
                nextDay = true;
                next -= new TimeSpan(1, 0, 0, 0);
            }
            if (nextDay && next >= new TimeSpan(8, 10, 0))
            {
                break;
            }
            busStopTime.Add(next);
        }
        LoadBusTime();
    }
    public void LoadBusTime()
    {
        busStopTimeDisplay.text = $"{string.Join('\n', GetBusTime(busStopDropdown.value, busDirectionDropdown.value == 1).Select(c => c.ToString("hh\\:mm")))}\n다음 변경 : {nextBusStopTimeChange:yyyy-MM-dd} (D-{(nextBusStopTimeChange - time.Date).TotalDays})";
    }
    public TimeSpan[] GetBusTime(int id, bool direction)
    {
        if (direction)
        {
            id = (data.busStop.Count - 1) * 2 - id;
        }
        return busStopTime.Select(c =>
        {
            TimeSpan a = c + new TimeSpan(0, id * 20, 0);
            if (a >= new TimeSpan(1, 0, 0, 0))
            {
                a -= new TimeSpan(1, 0, 0, 0);
            }
            return a;
        }).ToArray();
    }
    /*void UpdateLv()
    {
        needExpForLvUP = (int)(25 * Mathf.Pow(1.08f, lv));
        if (lv <= 18)
        {
            studyLvBonus = Mathf.Pow(1.25f, lv);
        }
        else
        {
            studyLvBonus = 55 * Mathf.Pow(1.02f, lv - 18);
        }
        LvIncome = Mathf.RoundToInt(9860 * Mathf.Pow(1.015f, lv));
        if (lv == maxLevel)
        {
            lvInfo.text = $"Lv {lv + 1} ({exp})\n공부 효율 {studyLvBonus}\n수입 {LvIncome}원/1시간\n반배정 성공확률 100%\n수업 1번 들을 시 능력치 1~{(int)(10 * studyLvBonus)} 상승";
        }
        else
        {
            lvInfo.text = $"Lv {lv + 1} ({exp}/{needExpForLvUP})\n공부 효율 {studyLvBonus}\n수입 {LvIncome}원/1시간\n반배정 성공확률 {Mathf.Clamp(lv + 10, 10, 100)}%\n수업 1번 들을 시 능력치 1~{(int)(10 * studyLvBonus)} 상승";
        }
        nextDayStudyExp = 1 - 0.05f * Mathf.Pow(0.99f, lv);
        updateInventory();
    }*/
    public bool isFriendable(int id)
    {
        if (id < 330)
        {
            return id <= data.friendableStudent[0];
        }
        else if (id < 660)
        {
            return id <= data.friendableStudent[1];
        }
        else
        {
            return id <= data.friendableStudent[2];
        }
    }
    public void UseItem1(int id)
    {
        if (cntProblemItem != -1)
        {
            itemRemove = false;
            return;
        }
        if (currentScene != "DormitoryRoom")
        {
            OpenDialog("이 장소에서는 이 아이템을 사용할 수 없습니다");
            itemRemove = false;
            return;
        }
        if (time.TimeOfDay >= new TimeSpan(7, 0, 0) && time.TimeOfDay <= new TimeSpan(14, 50, 0))
        {
            OpenDialog("이 아이템은 7시 ~ 14시 50분까지 사용할 수 없습니다");
            itemRemove = false;
            return;
        }
        /*
        임시로 문제 푸는 기능 없앰
        cntProblemItem = id;
        Problem p = data.problem[id].value[Random.Range(0, data.problem[id].value.Count)];
        problem.SetActive(true);
        problem.transform.SetAsLastSibling();
        if (p.imgContent == null)
        {
            problemText.gameObject.SetActive(true);
            problemImage.gameObject.SetActive(false);
            problemText.text = p.content;
        }
        else
        {
            problemText.gameObject.SetActive(false);
            problemImage.gameObject.SetActive(true);
            problemImage.sprite = p.imgContent;
        }
        problemAnswer = p.answer;
        problemTimer = new TimeSpan(0, 1, 0) * problemTime;
        timeSpeed = new TimeSpan(0, 0, 30) * (problemTime / 60);*/
        // v1.0에서는 문제 풀 필요 없음
        int l = id % 10 + 1;
        giveStudyExp(id / 10, l, l * 2);
        time += new TimeSpan(0, 1, 0) * problemTime;
    }
    public void giveStudyExp(int sub, int min, int max)
    {
        int amount = Random.Range(min == 1 ? 1 : (int)(min * studyLvBonus), (int)(max * studyLvBonus) + 1);
        studyExp[sub] += amount;
        SendMessage($"{data.subjectName[sub]} 능력치가 {Mathf.Abs(amount)} {(amount >= 0 ? "증가" : "감소")}했습니다");
        if (studyExp.All(c => c >= 1000000))
        {
            GiveAch(12);
        }
        if (studyExp[weeklyGoalSubject] >= weeklyGoalValue && !weeklyGoalCompleted)
        {
            //weeklyGoalCompleted = true;
            GiveExp(weeklyGoalReward);
            weeklyGoalSubject = Random.Range(0, 5);
            weeklyGoalValue = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 20, int.MaxValue) * Random.Range(1.2f, 1.5f));
            weeklyGoalReward = (int)(Mathf.Clamp(studyExp[weeklyGoalSubject], 500, int.MaxValue) * Random.Range(0.3f, 0.5f));
            updateWeeklyGoalDisplay();
        }
    }
    void updateWeeklyGoalDisplay()
    {
        weeklyGoalDisplay.text = $"{data.subjectName[weeklyGoalSubject]} {weeklyGoalValue} 이상 → {weeklyGoalReward} XP";
    }
    public void updateInventory()
    {
        foreach (Transform item in inventoryDisplay)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            Transform b = Instantiate(itemContent).transform;
            b.SetParent(inventoryDisplay, false);
            Item d = data.item[inventory[i]];
            b.Find("Name").GetComponent<Text>().text = d.name;
            d.descExt.Invoke();
            b.Find("Desc").GetComponent<Text>().text = String.Format(d.desc, descExt);
            int i2 = i;
            b.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => UseItem(i2));
        }
    }
    public void UseItem(int id)
    {
        itemRemove = true;
        data.item[inventory[id]].use.Invoke();
        if (itemRemove)
        {
            inventory.RemoveAt(id);
        }
        updateInventory();
    }
    public void Item1Desc(int l)
    {
        descExt = new object[] { l == 1 ? 1 : (int)(l * studyLvBonus), (int)(l * 2 * studyLvBonus) };
    }
    public void ProblemOK()
    {
        time += problemTimer;
        problemTimer = TimeSpan.Zero;
    }
    public void updateShop()
    {
        foreach (Transform item in buyItemDisplay)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < data.item.Count; i++)
        {
            Item d = data.item[i];
            if (d.cost == 0)
            {
                continue;
            }
            Transform b = Instantiate(buyItemContent).transform;
            b.SetParent(buyItemDisplay, false);
            b.Find("Name").GetComponent<Text>().text = $"{d.name} ({d.cost}원)";
            d.descExt.Invoke();
            b.Find("Desc").GetComponent<Text>().text = string.Format(d.desc, descExt);
            int i2 = i;
            b.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuyItem(i2));
        }
    }
    public void BuyItem(int id)
    {
        if (money < data.item[id].cost)
        {
            OpenDialog("돈이 부족합니다");
            return;
        }
        money -= data.item[id].cost;
        inventory.Add(id);
        updateInventory();
    }
    public void generateSudoku()
    {
        int inf = 10000;
        while (true)
        {
            generateSudoku_1:
            inf--;
            sudoku = new int[6, 6];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    List<int> available = Enumerable.Range(0, 6).ToList();
                    int[] a = new int[6];
                    for (int k = 1; k <= 6; k++)
                    {
                        int ind = Random.Range(0, available.Count);
                        a[available[ind]] = k;
                        available.RemoveAt(ind);
                    }
                    bool fail = true;
                    foreach (int n in a)
                    {
                        bool flag = false;
                        for (int i2 = 0; i2 < 6; i2++)
                        {
                            if (sudoku[i2, j] == n)
                            {
                                flag = true;
                                goto generateSudoku_3;
                            }
                        }
                        for (int i2 = 0; i2 < 6; i2++)
                        {
                            if (sudoku[i, i2] == n)
                            {
                                flag = true;
                                goto generateSudoku_3;
                            }
                        }
                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            for (int j2 = 0; j2 < 3; j2++)
                            {
                                int i3 = i / 2 * 2 + i2;
                                int j3 = j / 3 * 3 + j2;
                                if (sudoku[i3,j3] == n)
                                {
                                    flag = true;
                                    goto generateSudoku_3;
                                }
                            }
                        }
                        generateSudoku_3:
                        if (flag)
                        {
                            continue;
                        }
                        sudoku[i, j] = n;
                        fail = false;
                        break;
                    }
                    if (fail)
                    {
                        goto generateSudoku_1;
                    }
                }
            }
            break;
        }
        string s = "";
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                s += sudoku[i, j];
                s += " ";
            }
            s += "\n";
        }
        foreach (Transform t in sudokuGrid)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            sudoku[Random.Range(0, 6), Random.Range(0, 6)] = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                GameObject g;
                if (sudoku[i, j] == 0)
                {
                    g = Instantiate(sudokuInput);
                }
                else
                {
                    g = Instantiate(sudokuText);
                    g.GetComponent<Text>().text = sudoku[i, j].ToString();
                }
                g.transform.SetParent(sudokuGrid, false);
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(j / 6f, (5 - i) / 6f);
                rt.anchorMax = new Vector2((j + 1) / 6f, (6 - i) / 6f);
            }
        }
    }
    public void End()
    {
        GiveAch(15);
        Save();
        GameObject end = GameObject.Find("EndDatePass");
        DontDestroyOnLoad(end);
        end.GetComponent<EndDatePass>().endDate = time;
        SceneManager.LoadScene("EndScene");
    }
    public bool ExperimentalCheck(Experimental e)
    {
        return experimental.Contains(e);
    }
    public void Exit()
    {
        if (currentScene == "Bus")
        {
            OpenDialog("여기에서는 게임 종료를 할 수 없습니다");
        }
        else
        {
            if (!tutorial)
            {
                Save();
            }
            SceneManager.LoadScene("SelectSaveScene");
        }
    }
    public void OpenChat(int id)
    {
        chatDisplay.SetActive(true);
        currentChat = id;
        currentChatElement = 0;
        chatTitleText.text = data.chat[id].name;
        updateChat();
    }
    void updateChat()
    {
        if (currentChatElement == -1)
        {
            chatDisplay.SetActive(false);
            return;
        }
        ChatElement e = data.chat[currentChat].value[currentChatElement];
        e.chatEvent.Invoke();
        nextChatElement = e.next;
        chatContentText.text = e.value;
        foreach (Transform item2 in chatOption)
        {
            Destroy(item2.gameObject);
        }
        if (e.option.Count == 0)
        {
            enableNext = true;
        }
        else
        {
            enableNext = false;
            foreach (NameAndVal<int> item in e.option)
            {
                int n = item.value;
                GameObject button = Instantiate(optionButton, chatOption);
                button.GetComponent<Button>().onClick.AddListener(() => ChatOptionSelect(n));
                button.transform.Find("Text (Legacy)").GetComponent<Text>().text = item.name;
            }
        }
    }
    public void NextChat()
    {
        if (enableNext)
        {
            currentChatElement = nextChatElement;
            updateChat();
        }
    }
    public void ChatOptionSelect(int id)
    {
        currentChatElement = id;
        updateChat();
    }
}
