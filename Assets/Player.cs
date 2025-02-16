using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    #region 저장 데이터
    public long exp;
    public int money;
    public DateTime time;
    public TimeSpan timeSpeed;
    public bool inClass;
    public bool inSchool;
    public float moveSpeed;
    public string currentScene;
    public int mapArgs;
    public long[] studyExp;
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
    public int[] stat;
    public List<Experimental> experimental;
    public DateTime startTime;
    public TimeSpan totalPlayTime;
    public int length;
    public bool end;
    public int difficulty;
    public int[] repeatGradeMax;
    public int[] stockCost;
    public int[] stockAmount;
    public bool[] stockStatus;
    public int[] stockCostChanged;
    public List<Quest> quest;
    public Quest1[] pendingQuest;
    public bool tutorial;
    public bool hiddenLevelMode;
    #endregion
    #region 저장 데이터가 아닌 변수들
    public int sbindex;
    public string saveName;
    //public int needExpForLvUP;
    public long needExpForLvUP => (long)(30 * Mathf.Pow(1.07f, stat[0]));
    //public int maxLevel;
    public GameObject gradeDoor;
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
    GameObject classPlaceInput;
    Text classPlaceDDay;
    public Text busStopTimeDisplay;
    public Dropdown busStopDropdown;
    public Dropdown busDirectionDropdown;
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
    Text goalDisplay;
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
    Direction2 doorDirection;
    public DateTime endTime;
    public Image endEffect;
    float endEffectAlpha;
    bool endEffectDuring;
    long[] oldStudyExp;
    public GameObject studyExpIncreaseEffect;
    public RectTransform studyExpPanel;
    public bool pause;
    public GameObject tutorialArrow;
    Image currentTutorialImage;
    public List<KeyCode> currentPressingButton;
    public Toggle fastSpeedToggle;
    List<int> alreadyTutorial;
    public object[] chatExtra;
    public GameObject mobileOnlyUI;
    bool alreadyPenalty;
    public string[] firstGrade;
    public int[] repeatGrade;
    public GameObject stockItem;
    public Transform stockList;
    public Transform[] stockItems;
    InputField[] stockInput;
    public GameObject alertItem;
    public Transform alertList;
    public Text questAmount;
    public GameObject questDialog;
    public GameObject questItem;
    public Transform questList;
    int currentQuestLevel;
    public Text newQuestText;
    public GameObject newQuestDialog;
    public GameObject shopDialog;
    int doorID = -1;
    public GameObject gradeDoor2;
    public RectTransform weeklyPaneltra;
    PropertyInfo[] statProp;
    public GameObject oldExpPanel;
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
                return 55 * Mathf.Pow(1.03f, stat[0] - 18);
            }
        }
    }
    public int LvIncome => (int)(10000 * Mathf.Pow(1.025f, stat[1]));
    public int classPlacementChance => Mathf.Clamp(10 + stat[2] * 2, 10, 100);
    public float problemTime => 60 * Mathf.Pow(0.99f, stat[3]);
    public int maxQuest => stat[4] <= 0 ? 1 : (hiddenLevelMode ? stat[4] / 10 : stat[4]) + 1;
    public int questTime => stat[5] <= 0 ? 1 : (hiddenLevelMode ? stat[5] / 10 : stat[5]) + 1;
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
        alreadyTutorial = new List<int>();
        chatExtra = new object[0];
        #region 저장 데이터 불러오기
        SaveFile6 save = (SaveFile6)GameObject.Find("SaveData").GetComponent<SaveBuffer>().save;
        time = DateTime.ParseExact(save.time, "yyyy-MM-dd HH:mm:ss", null);
        timeSpeed = TimeSpan.Parse(save.timeSpeed);
        exp = save.exp;
        money = save.money;
        studyExp = save.studyExp;
        scores = save.scores;
        schedule = save.schindex;
        inClass = save.inclass;
        inSchool = save.inschool;
        clas = save.clas;
        inventory = save.inventory;
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
        experimental = save.experimental;
        mapInited = true;
        Move(save.map, save.mapextra, new Vector3(save.x, save.y, 0));
        stat = save.stat;
        if (stat.Length < data.stat.Count)
        {
            stat = stat.Concat(new int[data.stat.Count - stat.Length]).ToArray();
        }
        startTime = DateTime.ParseExact(save.startTime, "yyyy-MM-dd HH:mm:ss", null);
        totalPlayTime = TimeSpan.ParseExact(save.totalPlayTime, "d\\:hh\\:mm\\:ss", null);
        length = save.length;
        end = save.end;
        difficulty = save.difficulty;
        repeatGradeMax = save.repeatGradeMax;
        stockAmount = save.stockAmount;
        stockCost = save.stockCost;
        stockCostChanged = save.stockCostChanged;
        stockStatus = save.stockStatus;
        quest = save.quest;
        pendingQuest = save.pendingQuest;
        tutorial = save.tutorial;
        hiddenLevelMode = save.hiddenLevelMode;
        #endregion
        Destroy(GameObject.Find("SaveData"));
        SaveBuffer.generated = false;
        canvas = GameObject.Find("Canvas").transform;
        dialog = canvas.Find("Dialog").gameObject;
        dialogText = dialog.transform.Find("DialogText").Find("Viewport").Find("Content").GetComponent<Text>();
        firstGrade = new string[8];
        repeatGrade = new int[8];
        CalcuateRankStat();
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
        //busStopTimeDisplay = canvas.Find("BusStopTime").Find("Scroll View").Find("Viewport").Find("Content").GetComponent<Text>();
        //busStopDropdown = canvas.Find("BusStopTime").Find("Dropdown (Legacy)").GetComponent<Dropdown>();
        //busDirectionDropdown = canvas.Find("BusStopTime").Find("Dropdown (Legacy) (1)").GetComponent<Dropdown>();
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
        problem = canvas.Find("Problem").gameObject;
        problemText = problem.transform.Find("Text").Find("Viewport").Find("Content").GetComponent<Text>();
        problemImage = problem.transform.Find("Image").GetComponent<Image>();
        problemTimerDisplay = problem.transform.Find("Timer").GetComponent<Text>();
        problemAnswerInput = problem.transform.Find("Answer").GetComponent<InputField>();
        speedDisplay = GameObject.Find("SpeedDisplay").GetComponent<Text>();
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
        endTime = new DateTime(2024, 3, 4) + new TimeSpan(length * 7, 0, 0, 0);
        oldStudyExp = new long[5];
        for (int i = 0; i < 5; i++)
        {
            oldStudyExp[i] = studyExp[i];
        }
        data.NeedExpForGrade = data.NeedExpForGrade.Select(c => difficulty > 0 ? c * difficulty : c * -difficulty / 100).ToList();
        if (Application.platform == RuntimePlatform.Android)
        {
            mobileOnlyUI.SetActive(true);
            speedDisplay.GetComponent<RectTransform>().offsetMin = new Vector2(300, 0);
        }
        stockInput = new InputField[5];
        stockItems = new Transform[5];
        StockUIUpdate();
        if (pendingQuest[0].reward == 0)
        {
            UpdatePendingQuest();
        }
        else
        {
            UpdateNewQuest();
        }
        UpdateQuestList();
        if (ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
        if (hiddenLevelMode)
        {
            studyExpPanel.offsetMin = new Vector2(0, -300);
            studyExpPanel.offsetMax = new Vector2(0, -150);
            weeklyPaneltra.anchoredPosition = new Vector2(0, -335);
            statProp = new PropertyInfo[data.stat.Count];
            for (int i = 0; i < statProp.Length; i++)
            {
                statProp[i] = typeof(Player).GetProperty(data.stat[i].prop);
            }
            xpDisplay.transform.parent.GetComponent<Button>().enabled = false;
            oldExpPanel.SetActive(true);
            UpdateLv();
        }
    }
    void Update()
    {
        if (!pause)
        {
            time += timeSpeed * Time.deltaTime * speed;
        }
        totalPlayTime += new TimeSpan(0, 0, 1) * Time.deltaTime;
        if (enableCheat)
        {
            if (GetKeyDown(KeyCode.Slash))
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
        //if (tutorial)
        //{
        //    if (TimeSpan.Parse(data.tutorialSchedule[schedule].time) < time.TimeOfDay)
        //    {
        //        if (TimeSpan.Parse(data.tutorialSchedule[schedule].time) > new TimeSpan(8, 0, 0) || time.TimeOfDay < new TimeSpan(14, 0, 0))
        //        {
        //            data.tutorialSchedule[schedule].work.Invoke();
        //            schedule++;
        //        }
        //    }
        //}
        //else
        //{
            if (TimeSpan.Parse(data.schedule[schedule].time) < time.TimeOfDay)
            {
                if (TimeSpan.Parse(data.schedule[schedule].time) > new TimeSpan(8, 0, 0) || time.TimeOfDay < new TimeSpan(14, 0, 0))
                {
                    data.schedule[schedule].work.Invoke();
                    schedule++;
                }
            }
        //}
        if (mapInited)
        {
            if (currentScene == "BusStop")
            {
                TimeSpan t = time.TimeOfDay;
                bool b1 = mapArgs != data.busStop.Count - 1;
                bool b2 = mapArgs != 0;
                for (int i = 0; i < busTime1.Length; i++)
                {
                    TimeSpan bt1 = busTime1[i];
                    TimeSpan bt2 = busTime2[i];
                    if (b1)
                    {
                        //if (t >= bt1 && t <= bt1 + new TimeSpan(0, 5, 0))
                        if (DateTimeCalc2.Between(t, bt1, DateTimeCalc2.Add(bt1, new TimeSpan(0, 5, 0))))
                        {
                            bus1.SetActive(true);
                            busBaseTime1 = bt1;
                            b1 = false;
                        }
                        else
                        {
                            bus1.SetActive(false);
                        }
                        if (b1 && DateTimeCalc2.Sub(bt1, t) <= new TimeSpan(1, 0, 0))
                        {
                            b1 = false;
                            busStopText2.text = $"{data.busStop[^1].name} 방향 ({DateTimeCalc2.Sub(bt1, t):m\\분\\ ss\\초})";
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
                        if (b2 && DateTimeCalc2.Sub(bt2, t) <= new TimeSpan(1, 0, 0))
                        {
                            b2 = false;
                            busStopText1.text = $"{data.busStop[0].name} 방향 ({DateTimeCalc2.Sub(bt2, t):m\\분\\ ss\\초})";
                        }
                    }
                }
            }
            if (currentScene == "Bus")
            {
                if (mapArgs == (busDirection ? 0 : data.busStop.Count - 1))
                {
                    busLocD.text = $"{data.busStop[mapArgs].name}\n({data.busStop[busDirection ? 0 : ^1].name} →)\n(0분 00초 남음)";
                    busDoor.gameObject.SetActive(true);
                }
                else
                {
                    TimeSpan t = time.TimeOfDay;
                    //if (t >= busBaseTime && t <= busBaseTime + new TimeSpan(0, 5, 0))
                    if (DateTimeCalc2.Between(t, busBaseTime, DateTimeCalc2.Add(busBaseTime, new TimeSpan(0, 5, 0))))
                    {
                        busDoor.gameObject.SetActive(true);
                    }
                    //if (t >= busBaseTime + new TimeSpan(0, 5, 0) && t <= busBaseTime + new TimeSpan(0, 20, 0))
                    if (DateTimeCalc2.Between(t, DateTimeCalc2.Add(busBaseTime, new TimeSpan(0, 5, 0)), DateTimeCalc2.Add(busBaseTime, new TimeSpan(0, 20, 0))))
                    {
                        busDoor.gameObject.SetActive(false);
                    }
                    bool overflow;
                    if (t >= DateTimeCalc2.Add2(busBaseTime, new TimeSpan(0, 20, 0), out overflow) && (!overflow || t <= new TimeSpan(1, 0, 0)))
                    {
                        mapArgs += busDirection ? -1 : 1;
                        busBaseTime = DateTimeCalc2.Add(busBaseTime, new TimeSpan(0, 20, 0));
                    }
                    busDoor.args = mapArgs;
                    TimeSpan tm = DateTimeCalc2.Sub(busBaseTime + new TimeSpan(0, 20, 0), time.TimeOfDay);
                    busLocD.text = $"{data.busStop[mapArgs].name}\n({data.busStop[busDirection ? 0 : ^1].name} →)\n({tm:m\\분\\ ss}초 남음)";
                }
            }
            if (currentScene == "Unnamed3")
            {
                moneyFloat += (float)(speed * timeSpeed).TotalHours * LvIncome * Time.deltaTime;
                int i = Mathf.FloorToInt(moneyFloat);
                moneyFloat -= i;
                money += i;
            }
            if (GetKeyDown(KeyCode.N) && currentScene == "DormitoryRoom" && !inSchool)
            {
                timeSpeed = new TimeSpan(1, 0, 0);
            }
        }
        speedDisplay.text = $"Speed = {speed}";
        if (GetKeyDown(KeyCode.Equals))
        {
            if (Input.GetKey(KeyCode.LeftShift) || fastSpeedToggle.isOn)
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
        }
        if (GetKeyDown(KeyCode.Minus) && speed > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || fastSpeedToggle.isOn)
            {
                speed /= 2;
            }
            else
            {
                speed -= 1;
            }
        }
        if (GetKeyDown(KeyCode.I))
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
                timeSpeed = new TimeSpan(0, 1, 0);
            }
        }
        if (!hiddenLevelMode)
        {
            xpDisplay.text = $"{exp} XP";
            xpDisplay2.text = $"{exp} XP";
        }
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
        if (GetKeyDown(KeyCode.E))
        {
            if (length == 0)
            {
                endingDisplay.SetActive(true);
                int ach = 0;
                for (int i = 0; i < data.achievement.Count; i++)
                {
                    if (!data.achievement[i].name.EndsWith("(E)") && achCompleted[i])
                    {
                        ach++;
                    }
                }
                int achSum = data.achievement.Count(c => !c.name.EndsWith("(E)"));
                long studyExpSum = studyExp.Sum();
                endingDisplayText.text = $@"엔딩 조건
<color={(ach >= achSum ? "green" : "red")}>1 - 모든 업적 달성 (이름 끝에 (E)가 있는 업적 제외) ({ach}/{achSum})</color>
<color={(stat[0] >= 150 ? "green" : "red")}>2 - 공부 효율 레벨 150 이상 달성 ({stat[0]}/150)</color>
<color={(money >= 10000000 ? "green" : "red")}>3 - 돈 10000000 달성 ({money}/10000000)</color>
<color={(studyExpSum >= 10000000 ? "green" : "red")}>4 - 모든 과목 능력치 합계 10000000 이상 ({studyExpSum}/10000000)</color>
(엔딩을 볼 시 자동으로 저장되며, 엔딩을 본 이후에도 해당 파일을 계속 플레이할 수 있습니다)";
                endButton.interactable = ach >= achSum && stat[0] >= 150 && money >= 10000000 && studyExpSum >= 10000000;
            }
        }
        GameObject selectedGameobject = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
        if (selectedGameobject == null || selectedGameobject.GetComponent<InputField>() == null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (GetKeyDown(KeyCode.Alpha0 + i))
                {
                    speed = i;
                }
            }
        }
        if (GetKeyDown(KeyCode.Escape))
        {
            if (currentChat == 1 && currentChatElement == 1)
            {
                NextChat2();
            }
            menu.SetActive(true);
        }
        if (speed == 0 && cntProblemItem != -1)
        {
            speed = 1;
        }
        if (endEffectDuring)
        {
            endEffectAlpha += Time.deltaTime / 5;
            endEffect.color = new Color(0, 0, 0, endEffectAlpha);
            if (endEffectAlpha >= 1)
            {
                Save();
                GameObject end = GameObject.Find("EndDatePass");
                DontDestroyOnLoad(end);
                end.GetComponent<EndDatePass>().studyExp = studyExp;
                end.GetComponent<EndDatePass>().score = scores[^1];
                SceneManager.LoadScene("EndScene2");
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (oldStudyExp[i] != studyExp[i])
            {
                GameObject g = Instantiate(studyExpIncreaseEffect, studyExpPanel);
                g.GetComponent<Text>().text = (studyExp[i] - oldStudyExp[i]).ToString("+0;-#");
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2((i + 1) / 6f, 0.33f);
                rt.anchorMax = rt.anchorMin;
                oldStudyExp[i] = studyExp[i];
            }
        }
        if (GetKeyDown(KeyCode.Q))
        {
            questDialog.SetActive(true);
        }
    }
    void Save()
    {
        if (tutorial)
        {
            return;
        }
        SaveFile6 save = new SaveFile6();
        save.version = 6;
        save.versionName = "18";
        save.time = time.ToString("yyyy-MM-dd HH:mm:ss");
        save.timeSpeed = timeSpeed.ToString();
        save.exp = exp;
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
        save.stat = stat;
        save.experimental = experimental;
        save.startTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
        save.totalPlayTime = totalPlayTime.ToString("d\\:hh\\:mm\\:ss");
        save.length = length;
        save.end = end;
        save.difficulty = difficulty;
        save.repeatGradeMax = repeatGradeMax;
        save.stockAmount = stockAmount;
        save.stockCost = stockCost;
        save.stockCostChanged = stockCostChanged;
        save.stockStatus = stockStatus;
        save.tutorial = tutorial;
        save.quest = quest;
        save.pendingQuest = pendingQuest;
        save.hiddenLevelMode = hiddenLevelMode;
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{saveName}", JsonUtility.ToJson(save));
    }
    public void StartDay()
    {
        schedule = -1;
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
        if (tutorial)
        {
            if (time.Date == new DateTime(2024, 3, 4))
            {
                OpenChat(0);
            }
            if (time.Date == new DateTime(2024, 3, 5))
            {
                OpenChat(8);
            }
            if (time.Date == new DateTime(2024, 3, 9))
            {
                OpenChat(10);
            }
            if (time.Date == new DateTime(2024, 3, 11))
            {
                OpenChat(11);
            }
        }
        alreadyPenalty = false;
        if (time.Date != new DateTime(2024, 3, 4))
        {
            updateShop();
            for (int i = 0; i < 5; i++)
            {
                if (stockStatus[i])
                {
                    stockCostChanged[i] = Random.Range(1, 101);
                    stockCost[i] += stockCostChanged[i];
                }
                else
                {
                    int prevcost = stockCost[i];
                    stockCostChanged[i] = Random.Range(-100, 0);
                    stockCost[i] += stockCostChanged[i];
                    if (stockCost[i] < 1)
                    {
                        stockCost[i] = 1;
                        stockCostChanged[i] = stockCost[i] - prevcost;
                    }
                }
                if (Random.Range(0, 10) == 0)
                {
                    stockStatus[i] = !stockStatus[i];
                }
            }
            StockUIUpdate();
        }
        for (int i = quest.Count - 1; i >= 0; i--)
        {
            Quest q = quest[i];
            if (DateTime.ParseExact(q.timeLimit, "yyyy-MM-dd", null) > time.Date)
            {
                continue;
            }
            bool fail = false;
            for (int j = 0; j < 5; j++)
            {
                if (studyExp[j] < q.req[j])
                {
                    fail = true;
                    break;
                }
            }
            if (fail)
            {
                SendMessage($"퀘스트를 실패하여 {q.reward} XP를 잃었습니다");
                GiveExp(-q.reward, false);
                quest.RemoveAt(i);
            }
        }
        UpdateQuestList();
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
        CalcuateRankStat();
        achGen.AchRegen();
        if (repeatGrade[0] == 10)
        {
            GiveAch(9);
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
        inClass = true;
        if (currentScene == "Classroom" && mapArgs == clas[0])
        {
            if (tutorial && time.Date == new DateTime(2024, 3, 5) && schedule == 0)
            {
                //studyExp[goalSubject] = goalValue;
                //GiveExp(goalReward);
                TutorialOpenChat(9);
            }
            else
            {
                giveStudyExp(Random.Range(0, 5), 1, 10);
            }
        }
        else
        {
            if (!alreadyPenalty)
            {
                money -= 50000;
                for (int i = 0; i < 5; i++)
                {
                    if (studyExp[i] >= 0)
                    {
                        studyExp[i] /= 2;
                    }
                    else
                    {
                        studyExp[i] *= 2;
                    }
                }
                alreadyPenalty = true;
            }
        }
        TutorialOpenChat(3);
    }
    public void EndClass()
    {
        //timeSpeed = new TimeSpan(0, 0, 5);
        //inClass = false;
    }
    public void EndSchool()
    {
        if (length != 0 && time.Date == endTime)
        {
            if (tutorial)
            {
                timeSpeed = TimeSpan.Zero;
            }
            else
            {
                End2();
            }
        }
        else if (length != 0 && time.Date == endTime)
        {
            timeSpeed = TimeSpan.Zero;
            end = true;
            endEffectDuring = true;
            endEffect.gameObject.SetActive(true);
            endEffect.transform.SetAsLastSibling();
        }
        else
        {
            timeSpeed = new TimeSpan(0, 1, 0);
        }
        if (time.DayOfWeek == DayOfWeek.Monday && time.Date != new DateTime(2024, 3, 4)) 
        {
            TutorialOpenChat(12);
            Test();
        }
        inClass = false;
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
        TutorialOpenChat(4);
    }
    public void GiveExp(long amount, bool msg = true)
    {
        exp += amount;
        if (msg)
        {
            SendMessage($"{amount} 경험치를 획득했습니다");
        }
        if (hiddenLevelMode)
        {
            while (exp >= needExpForLvUP)
            {
                exp -= needExpForLvUP;
                for (int i = 0; i < stat.Length; i++)
                {
                    stat[i]++;
                }
                SendMessage($"레벨 {stat[0] + 1}을 달성했습니다");
                UpdateLv();
            }
        }
    }
    public void Move(string name, int args, Vector3 pos, int destDoorId = -1, Direction2 doorDirection = 0)
    {
        if (!mapInited)
        {
            return;
        }
        if (name == "Main1F")
        {
            GiveAch(0);
        }
        if (name == "BusStop")
        {
            TutorialOpenChat(1);
        }
        if (name == "Main1F")
        {
            TutorialOpenChat(2);
        }
        if (name == "Shop")
        {
            TutorialOpenChat(7);
        }
        shopDialog.SetActive(false);
        if (name != currentScene || args != mapArgs)
        {
            mapInited = false;
            if (!string.IsNullOrEmpty(currentScene))
            {
                SceneManager.UnloadSceneAsync(actualSceneName);
            }
            actualSceneName = ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN) && SceneUtility.GetBuildIndexByScenePath($"Assets/Map/{name}.unity") != -1 ? name : "Old" + name;
            //actualSceneName = name;
            currentScene = name;
            SceneManager.LoadScene(actualSceneName, LoadSceneMode.Additive);
            mapArgs = args;
        }
        transform.position = pos;
        control = false;
        doorID = destDoorId;
        this.doorDirection = doorDirection;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
        {
            return;
        }
        SceneManager.SetActiveScene(scene);
        if (currentScene == "Main1F") // 본관 복도 생성
        {
            if (ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject door = Instantiate(gradeDoor2);
                    door.transform.position = new Vector3(i * 4 - 17.5f, 3.5f, 0);
                    door.transform.Find("Text").GetComponent<TextMeshPro>().text = $"{mapArgs + 1}-{i + 1}";
                    Door door2 = door.GetComponent<Door>();
                    door2.args = mapArgs * 10 + i;
                    door2.doorID = door2.args;
                    if (mapArgs == 0 && i == clas[0])
                    {
                        door.transform.Find("Text").GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                    }
                    door = Instantiate(gradeDoor2);
                    door.transform.position = new Vector3(i * 4 - 15.5f, 3.5f, 0);
                    door.transform.Find("Text").GetComponent<TextMeshPro>().text = $"{mapArgs + 1}-{i + 1}";
                    door2 = door.GetComponent<Door>();
                    door2.args = mapArgs * 10 + i;
                    door2.doorID = door2.args + 100;
                    if (mapArgs == 0 && i == clas[0])
                    {
                        door.transform.Find("Text").GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject door = Instantiate(gradeDoor);
                    door.transform.position = new Vector3(i * 3 + 1, 1.4f, 0);
                    door.transform.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"{mapArgs + 1}-{i + 1}";
                    Door door2 = door.transform.Find("Door (2)").GetComponent<Door>();
                    door2.args = mapArgs * 10 + i;
                    door2.doorID = door2.args;
                    if (mapArgs == 0 && i == clas[0])
                    {
                        door.transform.Find("Text (TMP)").GetComponent<TextMeshPro>().fontStyle = FontStyles.Bold;
                    }
                }
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
        if (currentScene == "Classroom") // 교실 생성
        {
            //GameObject.Find("Square (3)").GetComponent<Door>().args = mapArgs / 10;
            //GameObject.Find("Square (3)").GetComponent<Door>().pos = new Vector3(1 + mapArgs % 10 * 3, 0.8f, 0);
            /*if (mapArgs != 9)
            {
                GameObject.Find("EasterEgg").SetActive(false);
            }*/
            if (mapArgs == clas[0] && inSchool)
            {
                timeSpeed = new TimeSpan(0, 10, 0);
            }
        }
        //if (currentScene == "Dormitory1F")
        //{
            //GameObject.Find("Square (4)").GetComponent<OpenGUIButton>().target = canvas.Find("PC").gameObject;
            //GameObject.Find("Square (4)").GetComponent<CPBlockCall>().c = canvas.Find("PC").GetComponent<CPBlock>();
            #region 통금 시간 (이제 안 씀)
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
        //}
        if (currentScene == "BusStop")
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
            GameObject.Find("Square (6)").GetComponent<Door>().destDoorID = data.busStop[mapArgs].destDoorID;
            GameObject.Find("Square (6)").GetComponent<Door>().direction = data.busStop[mapArgs].direction;
            bus1 = GameObject.Find("Square (7)");
            bus1.GetComponent<Door>().args = mapArgs;
            bus2 = GameObject.Find("Square (8)");
            bus2.GetComponent<Door>().args = mapArgs + 65536;
            busTime1 = GetBusTime(mapArgs, false);
            busTime2 = GetBusTime(mapArgs, true);
            busStopText1 = GameObject.Find("Text (TMP) (1)").GetComponent<TextMeshPro>();
            busStopText2 = GameObject.Find("Text (TMP) (2)").GetComponent<TextMeshPro>();
            busStopText1.text = $"{data.busStop[0].name} 방향";
            busStopText2.text = $"{data.busStop[^1].name} 방향";
            bus1.SetActive(false);
            bus2.SetActive(false);
        }
        if (currentScene == "Bus")
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
        if (tutorial && currentScene == "Unnamed3" && time.Date == new DateTime(2024, 3, 4)) // 튜토리얼 6 출력 + 문 잠그기
        {
            TutorialOpenChat(5);
            GameObject.Find("Door").GetComponent<Door>().enable = false;
        }
        mapInited = true;
        if (doorID != -1 && ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            Vector3 dpos = new Vector3(0, 0, 1);
            foreach (Door d in FindObjectsOfType<Door>())
            {
                if (d.doorID == doorID)
                {
                    dpos = d.transform.position;
                }
            }
            if (dpos == new Vector3(0, 0, 1))
            {
                OpenDialog($"Error : wrong door ID [{doorID}]");
            }
            switch (doorDirection)
            {
                case Direction2.left:
                    transform.position = dpos + Vector3.left;
                    break;
                case Direction2.right:
                    transform.position = dpos + Vector3.right;
                    break;
                case Direction2.up:
                    transform.position = dpos + Vector3.up;
                    break;
                case Direction2.down:
                    transform.position = dpos + Vector3.down;
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
        Instantiate(alertItem, alertList).transform.Find("Text (Legacy)").GetComponent<Text>().text = text;
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
        if (tutorial && currentScene == "Unnamed3")
        {
            GameObject.Find("Door").GetComponent<Door>().enable = true;
            TutorialOpenChat(6);
        }
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
            TimeSpan a = DateTimeCalc2.Add(c, new TimeSpan(0, id * 20, 0));
            return a;
        }).ToArray();
    }
    void UpdateLv()
    {
        if (!hiddenLevelMode)
        {
            return;
        }
        /*needExpForLvUP = (int)(25 * Mathf.Pow(1.08f, lv));
        if (lv <= 18)
        {
            studyLvBonus = Mathf.Pow(1.25f, lv);
        }
        else
        {
            studyLvBonus = 55 * Mathf.Pow(1.02f, lv - 18);
        }
        LvIncome = Mathf.RoundToInt(9860 * Mathf.Pow(1.015f, lv));*/
        //수업 1번 들을 시 능력치 1~{(int)(10 * studyLvBonus)} 상승\n
        string lt = $"Lv {stat[0] + 1} ({exp}/{needExpForLvUP})\n";
        for (int i = 0; i < statProp.Length; i++)
        {
            StatType st = data.stat[i];
            lt += $"{st.name} {st.prefix}{statProp[i].GetValue(this)}{st.suffix}\n";
        }
        lvInfo.text = lt;
        //nextDayStudyExp = 1 - 0.05f * Mathf.Pow(0.99f, lv);
        updateInventory();
    }
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
        int amount = Random.Range((int)(min * studyLvBonus), (int)(max * studyLvBonus) + 1);
        studyExp[sub] += amount;
        SendMessage($"{data.subjectName[sub]} 능력치가 {Mathf.Abs(amount)} {(amount >= 0 ? "증가" : "감소")}했습니다");
        if (studyExp.All(c => c >= 1000000))
        {
            GiveAch(12);
        }
        for (int i = quest.Count - 1; i >= 0; i--)
        {
            Quest q = quest[i];
            bool complete = true;
            for (int j = 0; j < 5; j++)
            {
                if (studyExp[j] < q.req[j])
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                SendMessage($"퀘스트를 성공하여 {q.reward} XP를 획득했습니다");
                GiveExp(q.reward, false);
                quest.RemoveAt(i);
            }
        }
        UpdateQuestList();
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
        if (end)
        {
            OpenDialog("이미 종료된 게임입니다");
            return;
        }
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
        descExt = new object[] { (int)(l * studyLvBonus), (int)(l * 2 * studyLvBonus) };
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
    public void End2()
    {
        timeSpeed = TimeSpan.Zero;
        end = true;
        endEffectDuring = true;
        endEffect.gameObject.SetActive(true);
        endEffect.transform.SetAsLastSibling();
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
            SceneManager.LoadScene("TitleScene");
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
            data.chat[currentChat].endEvent.Invoke();
            currentChat = -1;
            return;
        }
        ChatElement e = data.chat[currentChat].value[currentChatElement];
        e.chatEvent.Invoke();
        if (e.next == -2)
        {
            nextChatElement = 0;
        }
        else if (e.next == 0)
        {
            if (currentChatElement == data.chat[currentChat].value.Count - 1)
            {
                nextChatElement = -1;
            }
            else
            {
                nextChatElement = currentChatElement + 1;
            }
        }
        else
        {
            nextChatElement = e.next;
        }
        chatContentText.text = string.Format(e.value, chatExtra);
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
        if (e.disableNext)
        {
            enableNext = false;
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
    public void NextChat2()
    {
        currentChatElement = nextChatElement;
        updateChat();
    }
    public void ChatOptionSelect(int id)
    {
        currentChatElement = id;
        updateChat();
    }
    public void ChangeTimeSpeed(string speed)
    {
        timeSpeed = TimeSpan.ParseExact(speed, "hh\\:mm\\:ss", null);
    }
    public void Pause(bool pause)
    {
        this.pause = pause;
    }
    public void TutorialUI(int id)
    {
        if (currentTutorialImage != null)
        {
            currentTutorialImage.color = new Color32(255, 255, 255, 100);
        }
        if (id == -1)
        {
            tutorialArrow.SetActive(false);
        }
        else
        {
            currentTutorialImage = data.tutorialUI[id].uiImage;
            if (currentTutorialImage != null)
            {
                currentTutorialImage.color = new Color32(255, 255, 255, 255);
            }
            tutorialArrow.SetActive(true);
            tutorialArrow.transform.localPosition = data.tutorialUI[id].arrowPos;
            tutorialArrow.transform.rotation = Quaternion.Euler(0, 0, data.tutorialUI[id].arrowRot);
        }
    }
    public bool GetKeyDown(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            return true;
        }
        if (currentPressingButton.Contains(key))
        {
            currentPressingButton.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void TutorialOpenChat(int id)
    {
        if (!alreadyTutorial.Contains(id) && tutorial)
        {
            alreadyTutorial.Add(id);
            OpenChat(id);
        }
    }
    public void ChatExtra1()
    {
        chatExtra = new object[] {clas[0] + 1};
    }
    public void TutorialEvent1()
    {
        //goalSubject = Random.Range(0, 5);
        //goalValue = (int)(Mathf.Clamp(studyExp[goalSubject], 20, int.MaxValue) * Random.Range(1.2f, 1.5f));
        //goalReward = (int)(Mathf.Clamp(studyExp[goalSubject], 500, int.MaxValue) * Random.Range(0.3f, 0.5f));
        //updateWeeklyGoalDisplay();
    }
    public void CalcuateRankStat()
    {
        for (int i = 0; i < 8; i++)
        {
            firstGrade[i] = "없음";
        }
        foreach (TestScore score in scores)
        {
            int avg = Mathf.CeilToInt(score.grade.Sum() / 5f) - 1;
            for (int i = 0; i < 8; i++)
            {
                if (avg <= i)
                {
                    if (firstGrade[i] == "없음")
                    {
                        firstGrade[i] = score.date;
                    }
                    repeatGrade[i]++;
                    if (repeatGrade[i] > repeatGradeMax[i])
                    {
                        repeatGradeMax[i] = repeatGrade[i];
                    }
                }
                else
                {
                    repeatGrade[i] = 0;
                }
            }
        }
    }
    public void StockUIUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            Transform st;
            if (stockItems[i] == null)
            {
                st = Instantiate(stockItem, stockList).transform;
                int i2 = i;
                st.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuyStock(i2));
                st.Find("SellButton").GetComponent<Button>().onClick.AddListener(() => SellStock(i2));
                stockInput[i] = st.Find("AmountInput").GetComponent<InputField>();
                stockItems[i] = st;
            }
            else
            {
                st = stockItems[i];
            }
            st.Find("Info").GetComponent<Text>().text = $"{i + 1}번 ({stockCost[i]}원) ({stockAmount[i]}주) <color={(stockCostChanged[i] > 0 ? "red" : "blue")}>{stockCostChanged[i]:▲ 0;▼ 0;\"\"}</color>";
        }
    }
    public void BuyStock(int id)
    {
        int amount = stockInput[id].text == "" ? money / stockCost[id] : int.Parse(stockInput[id].text);
        if (stockCost[id] * amount > money)
        {
            OpenDialog("돈이 부족합니다");
        }
        else
        {
            money -= stockCost[id] * amount;
            stockAmount[id] += amount;
            StockUIUpdate();
        }
    }
    public void SellStock(int id)
    {
        int amount = stockInput[id].text == "" ? stockAmount[id] : int.Parse(stockInput[id].text);
        if (amount > stockAmount[id])
        {
            OpenDialog("주식이 부족합니다");
        }
        else
        {
            money += stockCost[id] * amount;
            stockAmount[id] -= amount;
            StockUIUpdate();
        }
    }
    public void UpdateQuestCount()
    {
        questAmount.text = $"{quest.Count} / {maxQuest}";
        if (quest.Count >= maxQuest)
        {
            questAmount.color = Color.red;
        }
        else
        {
            questAmount.color = new Color32(50, 50, 50, 255);
        }
    }
    public void UpdateNewQuest()
    {
        Quest1 quest = pendingQuest[currentQuestLevel];
        string req = "";
        for (int i = 0; i < 5; i++)
        {
            int r = quest.req[i];
            if (r != 0)
            {
                req += $"{data.subjectName[i]} +{r} ({studyExp[i] + r}) 이상\n";
            }
        }
        newQuestText.text = $"기간 : {questTime}일 (~{GetDate2() + new TimeSpan(questTime, 0, 0, 0):yyyy-MM-dd} 8시까지)\n(기간은 퀘스트를 받은 당일 기준의 \"퀘스트 기간\" 강화에 의해 결정됩니다)\n성공 보상 : {quest.reward} XP\n실패 패널티: -{quest.reward} XP\n{req}(기간 및 요구량의 괄호 안 내용은 지금 받을 때 기준이며, 괄호 밖 부분 및 성공 보상 / 실패 패널티는 받는 시간과 무관합니다)";
    }
    public void ChangeQuestLevel(int lv)
    {
        currentQuestLevel = lv;
        UpdateNewQuest();
    }
    public void UpdatePendingQuest()
    {
        pendingQuest[0].req = new int[5];
        int sub = Random.Range(0, 5);
        pendingQuest[0].req[sub] = Mathf.Max((int)(studyExp[sub] * Random.Range(0.1f, 0.3f)), 10);
        pendingQuest[0].reward = (int)(Random.Range(0.3f, 0.5f) * Mathf.Max(pendingQuest[0].req[sub], 500));
        for (int i = 1; i < 5; i++)
        {
            pendingQuest[i].req = (int[])pendingQuest[i-1].req.Clone();
            int sub2 = 0;
            do
            {
                sub2 = Random.Range(0, 5);
            } while (pendingQuest[i].req[sub2] != 0);
            for (int j = 0; j < 5; j++)
            {
                if (pendingQuest[i].req[j] != 0)
                {
                    pendingQuest[i].req[j] = (int)(pendingQuest[i].req[j] * Random.Range(1.5f, 2f));
                }
            }
            pendingQuest[i].req[sub2] = Mathf.Max((int)(studyExp[sub2] * Random.Range(0.1f, 0.3f)), 10);
            pendingQuest[i].reward = (int)(pendingQuest[i - 1].reward * Random.Range(1.5f, 2f));
        }
        UpdateNewQuest();
    }
    public DateTime GetDate2()
    {
        DateTime d = time.Date;
        if (time.TimeOfDay < new TimeSpan(8, 0, 0))
        {
            return d - DateTimeCalc2.day;
        }
        else
        {
            return d;
        }
    }
    public void AcceptQuest()
    {
        if (quest.Count >= maxQuest)
        {
            OpenDialog($"최대 퀘스트 수({maxQuest}개)에 도달했습니다");
            return;
        }
        Quest1 q1 = pendingQuest[currentQuestLevel];
        long[] r = new long[5];
        for (int i = 0; i < 5; i++)
        {
            r[i] = q1.req[i] == 0 ? 0 : studyExp[i] + q1.req[i];
        }
        quest.Add(new Quest() {req = r, reward = q1.reward, timeLimit = (GetDate2() + new TimeSpan(questTime, 0, 0, 0)).ToString("yyyy-MM-dd")});
        newQuestDialog.SetActive(false);
        UpdatePendingQuest();
        UpdateQuestList();
    }
    public void RefreshQuest()
    {
        if (money >= 300000)
        {
            money -= 300000;
            UpdatePendingQuest();
        }
        else
        {
            OpenDialog("돈이 부족합니다");
        }
    }
    public void UpdateQuestList()
    {
        foreach (Transform item in questList)
        {
            Destroy(item.gameObject);
        }
        foreach (Quest q in quest)
        {
            string req = "";
            for (int i = 0; i < 5; i++)
            {
                if (q.req[i] != 0)
                {
                    req += $"\n<color={(q.req[i] <= studyExp[i] ? "green" : "red")}>{data.subjectName[i]} {q.req[i]} 이상 (현재 {studyExp[i]}{(q.req[i] > studyExp[i] ? $", {q.req[i] - studyExp[i]} 남음" : "")})</color>";
                }
            }
            DateTime d = DateTime.ParseExact(q.timeLimit, "yyyy-MM-dd", null);
            Instantiate(questItem, questList).transform.Find("Text").GetComponent<Text>().text = $"기간 : {d.Year}년 {d.Month}월 {d.Day}일 8시까지 (D-{(d - GetDate2()).TotalDays})\n성공 보상 : {q.reward} XP\n실패 패널티 : -{q.reward} XP{req}";
        }
        UpdateQuestCount();
    }
}
