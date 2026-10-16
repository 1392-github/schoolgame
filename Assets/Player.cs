using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    #region 저장 데이터가 아닌 변수들
    public float moveSpeed;
    public int sbindex;
    public List<Schedule> schedule;
    public GameObject gradeDoor;
    [SerializeField] Chat firstClassChat;
    bool left;
    bool right;
    bool up;
    bool down;
    Data data;
    Rigidbody2D rb;
    GameObject dialog;
    Text dialogText;
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
    Text speedDisplay;
    public GameObject itemContent;
    Transform canvas;
    int cntProblemItem;
    public GameObject buyItemContent;
    Transform buyItemDisplay;
    GameObject buyItemDisplay2;
    public int[,] sudoku;
    public Transform sudokuGrid;
    public GameObject sudokuText;
    public GameObject sudokuInput;
    public GameObject endingDisplay;
    public Text endingDisplayText;
    public Button endButton;
    public Text sudokuTimer;
    public Text xpDisplay;
    public Text xpDisplay2;
    public Transform upgradeScroll;
    public GameObject upgradeButton;
    public GameObject menu;
    public AchGenerator achGen;
    string actualSceneName;
    Direction2 doorDirection;
    public Image endEffect;
    long[] oldStudyExp;
    public GameObject studyExpIncreaseEffect;
    public RectTransform studyExpPanel;
    public GameObject tutorialArrow;
    Image currentTutorialImage;
    public List<KeyCode> currentPressingButton;
    public Toggle fastSpeedToggle;
    public object[] chatExtra;
    public GameObject mobileOnlyUI;
    bool alreadyPenalty;
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
    int speed = 1;
    public Chat parkmunwon1;
    public Chat whyDontRegister;
    readonly int[] numberKeySpeed = {0, 1, 2, 3, 5, 10, 20, 30, 50, 100};
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
        chatExtra = new object[0];
        if (GameData.achCompleted.Length < data.achievement.Count)
        {
            GameData.achCompleted = GameData.achCompleted.Concat(new bool[data.achievement.Count - GameData.achCompleted.Length]).ToArray();
        }
        mapInited = true;
        Move(GameData.currentScene, GameData.mapArgs, new Vector3(GameData.x, GameData.y, 0), forceLoad: true);
        canvas = GameObject.Find("Canvas").transform;
        dialog = canvas.Find("Dialog").gameObject;
        dialogText = dialog.transform.Find("DialogText").Find("Viewport").Find("Content").GetComponent<Text>();
        achGen.Start2();
        UpdateDDay();
        lvInfo = canvas.Find("LvInfo").Find("Lv").GetComponent<Text>();
        speedDisplay = GameObject.Find("SpeedDisplay").GetComponent<Text>();
        cntProblemItem = -1;
        buyItemDisplay2 = canvas.Find("Shop").gameObject;
        buyItemDisplay = buyItemDisplay2.transform.Find("Scroll View").Find("Viewport").Find("Content");
        updateShop();
        sudoku = new int[6, 6];
        updateInventory();
        oldStudyExp = new long[5];
        for (int i = 0; i < 5; i++)
        {
            oldStudyExp[i] = GameData.studyExp[i];
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            mobileOnlyUI.SetActive(true);
            speedDisplay.GetComponent<RectTransform>().offsetMin = new Vector2(300, 0);
        }
        if (GameData.pendingQuest[0].reward == 0)
        {
            UpdatePendingQuest();
        }
        else
        {
            UpdateNewQuest();
        }
        UpdateQuestList();
        if (GameData.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
        if (GameData.hiddenLevelMode)
        {
            studyExpPanel.offsetMin = new Vector2(0, -300);
            studyExpPanel.offsetMax = new Vector2(0, -150);
            weeklyPaneltra.anchoredPosition = new Vector2(0, -335);
            statProp = new PropertyInfo[GameData.statTypes.Count];
            for (int i = 0; i < statProp.Length; i++)
            {
                statProp[i] = typeof(Player).GetProperty(GameData.statTypes[i].prop);
            }
            xpDisplay.transform.parent.GetComponent<Button>().enabled = false;
            oldExpPanel.SetActive(true);
            UpdateLv();
        }
        if (GameData.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
        GameData.player = this;
    }
    void Update()
    {
        if (!GameData.pause)
        {
            GameData.time += GameData.timeSpeed * Time.deltaTime * speed;
        }
        GameData.totalPlayTime += new TimeSpan(0, 0, 1) * Time.deltaTime;
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
        if (TimeSpan.Parse(schedule[GameData.schedule].time) < GameData.time.TimeOfDay)
        {
            if (TimeSpan.Parse(schedule[GameData.schedule].time) > new TimeSpan(8, 0, 0) || GameData.time.TimeOfDay < new TimeSpan(14, 0, 0))
            {
                schedule[GameData.schedule].work.Invoke();
                GameData.schedule++;
            }
        }
        //}
        if (mapInited)
        {
            if (GameData.currentScene == "Unnamed3")
            {
                moneyFloat += (float)(speed * GameData.timeSpeed).TotalHours * GameData.LvIncome * Time.deltaTime;
                int i = Mathf.FloorToInt(moneyFloat);
                moneyFloat -= i;
                GameData.money += i;
            }
            if (GetKeyDown(KeyCode.N) && GameData.currentScene == "DormitoryRoom" && !GameData.inSchool)
            {
                GameData.timeSpeed = new TimeSpan(1, 0, 0);
            }
        }
        speedDisplay.text = $"Speed = {speed}";
        if (GetKeyDown(KeyCode.Equals) || GetKeyDown(KeyCode.KeypadPlus))
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
        if ((GetKeyDown(KeyCode.Minus) || GetKeyDown(KeyCode.KeypadMinus)) && speed > 0)
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
        if (!GameData.hiddenLevelMode)
        {
            xpDisplay.text = $"{GameData.exp} XP";
            xpDisplay2.text = $"{GameData.exp} XP";
        }
        GameObject selectedGameobject = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject;
        if (selectedGameobject == null || selectedGameobject.GetComponent<InputField>() == null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (GetKeyDown(KeyCode.Alpha0 + i) || GetKeyDown(KeyCode.Keypad0 + i))
                {
                    speed = numberKeySpeed[i];
                }
            }
        }
        if (GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
        }
        if (speed == 0 && cntProblemItem != -1)
        {
            speed = 1;
        }
        for (int i = 0; i < 5; i++)
        {
            if (oldStudyExp[i] != GameData.studyExp[i])
            {
                GameObject g = Instantiate(studyExpIncreaseEffect, studyExpPanel);
                g.GetComponent<Text>().text = (GameData.studyExp[i] - oldStudyExp[i]).ToString("+0;-#");
                RectTransform rt = g.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2((i + 1) / 6f, 0.33f);
                rt.anchorMax = rt.anchorMin;
                oldStudyExp[i] = GameData.studyExp[i];
            }
        }
        if (GetKeyDown(KeyCode.Q))
        {
            questDialog.SetActive(true);
        }
    }
    public void StartDay()
    {
        GameData.nextDayOnHome = true;
        Move("Home", 0, Vector3.zero);
    }
    public void StartClass()
    {
        if (GameData.weekendOrVacation) return;
        GameData.inClass = true;
        if (GameData.currentScene == "Classroom" && GameData.mapArgs == GameData.clas)
        {
            GameData.giveStudyExp(Random.Range(0, 5), 5, 10);
        }
        else
        {
            if (!alreadyPenalty)
            {
                GameData.money -= 50000;
                for (int i = 0; i < 5; i++)
                {
                    if (GameData.studyExp[i] >= 0)
                    {
                        GameData.studyExp[i] /= 2;
                    }
                    else
                    {
                        GameData.studyExp[i] *= 2;
                    }
                }
                alreadyPenalty = true;
            }
        }
    }
    public void EndClass() { }
    public void EndSchool()
    {
        if (GameData.weekendOrVacation) return;
        GameData.timeSpeed = new TimeSpan(0, 1, 0);
        GameData.inClass = false;
        GameData.inSchool = false;
        ExamManager.Exam();
    }
    public void GiveExp(long amount, bool msg = true)
    {
        GameData.exp += amount;
        if (msg)
        {
            SendMessage($"{amount} 경험치를 획득했습니다");
        }
        if (GameData.hiddenLevelMode)
        {
            while (GameData.exp >= GameData.needExpForLvUP)
            {
                GameData.exp -= GameData.needExpForLvUP;
                for (int i = 0; i < GameData.stat.Length; i++)
                {
                    GameData.stat[i]++;
                }
                SendMessage($"레벨 {GameData.stat[0] + 1}을 달성했습니다");
                UpdateLv();
            }
        }
    }
    public void Move(string name, int args, Vector3 pos, int destDoorId = -1, Direction2 doorDirection = 0, bool forceLoad = false)
    {
        if (!mapInited)
        {
            return;
        }
        if (name == "Home")
        {
            GameData.currentScene = "Home";
            SceneManager.LoadScene("HomeScene");
            return;
        }
        if (name == "Main1F")
        {
            GiveAch(0);
            if (GameData.time < GameData.firstDay.AddHours(1))
            {
                ChatManager.OpenChat(firstClassChat);
            }
            if (UniversityManager.finalUniversity == -1 && Util.InGameDate(GameData.time) == GameData.graduationDate.AddDays(-1))
            {
                ChatManager.OpenChat(whyDontRegister);
            }
        }
        shopDialog.SetActive(false);
        if (forceLoad || name != GameData.currentScene || args != GameData.mapArgs)
        {
            mapInited = false;
            if (!forceLoad)
            {
                SceneManager.UnloadSceneAsync(actualSceneName);
            }
            actualSceneName = GameData.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN) && SceneUtility.GetBuildIndexByScenePath($"Assets/Map/{name}.unity") != -1 ? name : "Old" + name;
            GameData.currentScene = name;
            SceneManager.LoadScene(actualSceneName, LoadSceneMode.Additive);
            GameData.mapArgs = args;
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
        if (GameData.currentScene == "Main1F") // 본관 복도 생성
        {
            if (GameData.ExperimentalCheck(Experimental.IMPROVEMENT_DESIGN))
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject door = Instantiate(gradeDoor2);
                    door.transform.position = new Vector3(i * 4 - 17.5f, 3.5f, 0);
                    door.transform.Find("Text").GetComponent<TextMeshPro>().text = $"{GameData.mapArgs + 1}-{i + 1}";
                    Door door2 = door.GetComponent<Door>();
                    door2.args = GameData.mapArgs * 10 + i;
                    door2.doorID = door2.args;
                    if (GameData.mapArgs == 0 && i == GameData.clas)
                    {
                        door.transform.Find("Text").GetComponent<TextMeshPro>().fontStyle = FontStyles.Underline;
                    }
                    door = Instantiate(gradeDoor2);
                    door.transform.position = new Vector3(i * 4 - 15.5f, 3.5f, 0);
                    door.transform.Find("Text").GetComponent<TextMeshPro>().text = $"{GameData.mapArgs + 1}-{i + 1}";
                    door2 = door.GetComponent<Door>();
                    door2.args = GameData.mapArgs * 10 + i;
                    door2.doorID = door2.args + 100;
                    if (GameData.mapArgs == 0 && i == GameData.clas)
                    {
                        door.transform.Find("Text").GetComponent<TextMeshPro>().fontStyle = FontStyles.Underline;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    GameObject door = Instantiate(gradeDoor);
                    door.transform.position = new Vector3(i * 3 + 1, 1.4f, 0);
                    door.transform.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"{GameData.mapArgs + 1}-{i + 1}";
                    Door door2 = door.transform.Find("Door (2)").GetComponent<Door>();
                    door2.args = GameData.mapArgs * 10 + i;
                    door2.doorID = door2.args;
                    if (GameData.mapArgs == 0 && i == GameData.clas)
                    {
                        door.transform.Find("Text (TMP)").GetComponent<TextMeshPro>().fontStyle = FontStyles.Underline;
                    }
                }
            }
            if (GameData.mapArgs == 2)
            {
                Destroy(GameObject.Find("Door (3)"));
                Destroy(GameObject.Find("Text (TMP) (1)"));
            }
            else
            {
                GameObject.Find("Door (3)").GetComponent<Door>().args = GameData.mapArgs + 1;
                GameObject.Find("Text (TMP) (1)").GetComponent<TextMeshPro>().text = $"↑ {GameData.mapArgs + 2}층";
            }
            if (GameData.mapArgs == 0)
            {
                GameObject.Find("Door (1)").GetComponent<Door>().map = "Hub";
                GameObject.Find("Door (1)").GetComponent<Door>().args = 0;
                GameObject.Find("Door (1)").GetComponent<Door>().destDoorID = 1;
                Destroy(GameObject.Find("Text (TMP)"));
            }
            else
            {
                GameObject.Find("Door (1)").GetComponent<Door>().args = GameData.mapArgs - 1;
                GameObject.Find("Text (TMP)").GetComponent<TextMeshPro>().text = $"↓ {GameData.mapArgs}층";
            }
        }
        if (GameData.currentScene == "Classroom") // 교실 생성
        {
            if (GameData.mapArgs == GameData.clas && GameData.inSchool)
            {
                GameData.timeSpeed = new TimeSpan(0, 10, 0);
            }
            if (GameData.time.Date == GameData.firstDay.Date)
            {
                TutorialOpenChat(TutorialState.SCHOOL_FIRST, parkmunwon1);
            }
        }
        if (GameData.currentScene == "Hub")
        {
            GameObject.Find("block").SetActive(GameData.inSchool);
        }
        mapInited = true;
        if (doorID != -1)
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
        if (GameData.achCompleted[id])
        {
            return;
        }
        SendMessage($"업적 {data.achievement[id].name}을 달성했습니다");
        GiveExp(data.achievement[id].exp);
        GameData.achCompleted[id] = true;
        achGen.AchRegen();
    }
    public void UpdateDDay() {}
    public void UpdateLv()
    {
        if (!GameData.hiddenLevelMode)
        {
            return;
        }
        string lt = $"Lv {GameData.stat[0] + 1} ({GameData.exp}/{GameData.needExpForLvUP})\n";
        for (int i = 0; i < statProp.Length; i++)
        {
            StatType st = GameData.statTypes[i];
            lt += $"{st.name} {st.prefix}{statProp[i].GetValue(this)}{st.suffix}\n";
        }
        lvInfo.text = lt;
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
    public void updateInventory() { }
    public void UseItem(int id)
    {
        if (((Func<bool>)GlobalEventManager.events[GameData.items[GameData.inventory[id]].use])())
        {
            GameData.inventory.RemoveAt(id);
        }
        updateInventory();
    }
    public void updateShop()
    {
        foreach (Transform item in buyItemDisplay)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < GameData.items.Count; i++)
        {
            Item d = GameData.items[i];
            if (d.cost == 0)
            {
                continue;
            }
            Transform b = Instantiate(buyItemContent).transform;
            b.SetParent(buyItemDisplay, false);
            b.Find("Name").GetComponent<Text>().text = $"{d.name} ({d.cost}원)";
            if (d.descExt == -1)
            {
                b.Find("Desc").GetComponent<Text>().text = d.desc;
            }
            else
            {
                b.Find("Desc").GetComponent<Text>().text = string.Format(d.desc, ((Func<object[]>)GlobalEventManager.events[d.descExt])());
            }
            int i2 = i;
            b.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => BuyItem(i2));
        }
    }
    public void BuyItem(int id)
    {
        if (GameData.money < GameData.items[id].cost)
        {
            OpenDialog("돈이 부족합니다");
            return;
        }
        GameData.money -= GameData.items[id].cost;
        GameData.inventory.Add(id);
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
    public void Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void ChangeTimeSpeed(string speed)
    {
        GameData.timeSpeed = TimeSpan.ParseExact(speed, "hh\\:mm\\:ss", null);
    }
    public void Pause(bool pause)
    {
        GameData.pause = pause;
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
    public void TutorialOpenChat(TutorialState state, Chat chat)
    {
        if ((GameData.tutorialState & state) == 0)
        {
            ChatManager.OpenChat(chat);
            GameData.tutorialState |= state;
        }
    }
    public void UpdateQuestCount()
    {
        questAmount.text = $"{GameData.quest.Count} / {GameData.maxQuest}";
        if (GameData.quest.Count >= GameData.maxQuest)
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
        Quest1 quest = GameData.pendingQuest[currentQuestLevel];
        string req = "";
        for (int i = 0; i < 5; i++)
        {
            int r = quest.req[i];
            if (r != 0)
            {
                req += $"{Util.subjectName[i]} +{r} ({GameData.studyExp[i] + r}) 이상\n";
            }
        }
        newQuestText.text = $"기간 : {GameData.questTime}일 (~{GetDate2() + new TimeSpan(GameData.questTime, 0, 0, 0):yyyy-MM-dd} 8시까지)\n(기간은 퀘스트를 받은 당일 기준의 \"퀘스트 기간\" 강화에 의해 결정됩니다)\n성공 보상 : {quest.reward} XP\n실패 패널티: -{quest.reward} XP\n{req}(기간 및 요구량의 괄호 안 내용은 지금 받을 때 기준이며, 괄호 밖 부분 및 성공 보상 / 실패 패널티는 받는 시간과 무관합니다)";
    }
    public void ChangeQuestLevel(int lv)
    {
        currentQuestLevel = lv;
        UpdateNewQuest();
    }
    public void UpdatePendingQuest()
    {
        GameData.pendingQuest[0].req = new int[5];
        int sub = Random.Range(0, 5);
        GameData.pendingQuest[0].req[sub] = Mathf.Max((int)(GameData.studyExp[sub] * Random.Range(0.1f, 0.3f)), 10);
        GameData.pendingQuest[0].reward = (int)(Random.Range(1f, 1.5f) * Mathf.Max(GameData.pendingQuest[0].req[sub], 100));
        for (int i = 1; i < 5; i++)
        {
            GameData.pendingQuest[i].req = (int[])GameData.pendingQuest[i-1].req.Clone();
            int sub2;
            do
            {
                sub2 = Random.Range(0, 5);
            } while (GameData.pendingQuest[i].req[sub2] != 0);
            for (int j = 0; j < 5; j++)
            {
                if (GameData.pendingQuest[i].req[j] != 0)
                {
                    GameData.pendingQuest[i].req[j] = (int)(GameData.pendingQuest[i].req[j] * Random.Range(1.5f, 2f));
                }
            }
            GameData.pendingQuest[i].req[sub2] = Mathf.Max((int)(GameData.studyExp[sub2] * Random.Range(0.1f, 0.3f)), 10);
            int total = 0;
            for (int j = 0; j < 5; j++)
            {
                total += GameData.pendingQuest[i].req[j];
            }
            GameData.pendingQuest[i].reward = Mathf.Max((int)(Mathf.Max(total, 100) * Random.Range(1f, 1.5f) * Mathf.Pow(1.1f, i)), (int)(GameData.pendingQuest[i-1].reward * 1.1f));
        }
        UpdateNewQuest();
    }
    public DateTime GetDate2()
    {
        DateTime d = GameData.time.Date;
        if (GameData.time.TimeOfDay < new TimeSpan(8, 0, 0))
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
        if (GameData.quest.Count >= GameData.maxQuest)
        {
            OpenDialog($"최대 퀘스트 수({GameData.maxQuest}개)에 도달했습니다");
            return;
        }
        Quest1 q1 = GameData.pendingQuest[currentQuestLevel];
        long[] r = new long[5];
        for (int i = 0; i < 5; i++)
        {
            r[i] = q1.req[i] == 0 ? 0 : GameData.studyExp[i] + q1.req[i];
        }
        GameData.quest.Add(new Quest() {req = r, reward = q1.reward, timeLimit = (GetDate2() + new TimeSpan(GameData.questTime, 0, 0, 0)).ToString("yyyy-MM-dd")});
        newQuestDialog.SetActive(false);
        UpdatePendingQuest();
        UpdateQuestList();
    }
    public void RefreshQuest()
    {
        if (GameData.money >= 300000)
        {
            GameData.money -= 300000;
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
        foreach (Quest q in GameData.quest)
        {
            string req = "";
            for (int i = 0; i < 5; i++)
            {
                if (q.req[i] != 0)
                {
                    req += $"\n<color={(q.req[i] <= GameData.studyExp[i] ? "green" : "red")}>{Util.subjectName[i]} {q.req[i]} 이상 (현재 {GameData.studyExp[i]}{(q.req[i] > GameData.studyExp[i] ? $", {q.req[i] - GameData.studyExp[i]} 남음" : "")})</color>";
                }
            }
            DateTime d = DateTime.ParseExact(q.timeLimit, "yyyy-MM-dd", null);
            Instantiate(questItem, questList).transform.Find("Text").GetComponent<Text>().text = $"기간 : {d.Year}년 {d.Month}월 {d.Day}일 8시까지 (D-{(d - GetDate2()).TotalDays})\n성공 보상 : {q.reward} XP\n실패 패널티 : -{q.reward} XP{req}";
        }
        UpdateQuestCount();
    }
}
