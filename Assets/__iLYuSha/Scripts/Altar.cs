using UnityEngine;
using UnityEngine.SceneManagement;

public enum AltarState
{
    Ready = 0,
    Opening = 2,
    HolyGrail = 3,
    RoundTable = 4,
    DragonFang = 5,
    Crown = 6,
    Altar = 7,
    Destruction = 12,
    Redemption = 14,
    Prophet = 17,
    Epilogue =18,
}
public partial class Altar : MonoBehaviour
{
    private AltarState altarState;
    private const int commandsCount = 2;
    private string[] commands = new string[commandsCount] { "NoWakaka", "NoPage" };
    private AudioSource audioSource;
    public AudioClip[] clips;
    private TimerRecordManager timerManager;

    void Awake()
    {
        if (ArduinoController.instance == null)
        {
            PlayerPrefs.SetInt("lastScene", SceneManager.GetActiveScene().buildIndex + 100);
            SceneManager.LoadScene("Arduino Controller");
        }
        ArduinoController.commandsCount = commandsCount; // 設定本專案單次接收Unity訊息的數量
        ArduinoController.msgQueueCombine = true;
        bookCamera.position += new Vector3(0.4f, 0, 0);
        audioSource = GetComponent<AudioSource>();
        Initialize();
    }
    void Initialize()
    {
        timerManager = FindObjectOfType<TimerRecordManager>();
        timerManager.AddMissionType("Part I");
        timerManager.AddMission(AltarState.HolyGrail.ToString());
        timerManager.AddMission(AltarState.RoundTable.ToString());
        timerManager.AddMission(AltarState.DragonFang.ToString());
        timerManager.AddMission(AltarState.Crown.ToString());
        timerManager.AddMissionType("Part II");
        timerManager.AddMission(AltarState.Altar.ToString());
        timerManager.AddMission(AltarState.Destruction.ToString());
        timerManager.AddMission(AltarState.Redemption.ToString());
        timerManager.AddMission(AltarState.Prophet.ToString());
    }
    void Update ()
    {
        if (ArduinoController.msgQueue.Count > 0)
            ArduinoMsg();
        else
            RealtimeControl();

        if (Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0))
            StartCoroutine(Page18());

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
                ForcePage((int)AltarState.HolyGrail);
            else if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
                ForcePage((int)AltarState.RoundTable);
            else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
                ForcePage((int)AltarState.DragonFang);
            else if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
                ForcePage((int)AltarState.Crown);
            else if (Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5))
                ForcePage((int)AltarState.Altar);
            else if (Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7))
                ForcePage((int)AltarState.Destruction);
            else if (Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8))
                ForcePage((int)AltarState.Redemption);
            else if (Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9))
                ForcePage((int)AltarState.Prophet);
        }
    }
    void ArduinoMsg()
    {
        for (int i = 0; i < ArduinoController.msgQueue.Count; i++)
        {
            string msg = ArduinoController.msgQueue.Dequeue();
            ArduinoController.instance.ArduinoMsg(msg);
            commands = msg.Split('/');
            RealtimeControl();
        }
    }
    void RealtimeControl()
    {
        KeyboardOpen();

        if (Time.time > timerOpen)
            OpenCheck();
        if (Time.time > timerFlip)
            FlipCheck();

        if((int)altarState<7)
            TimerOpen();
    }
    void TimerOpen()
    {
        if (Input.GetKey(KeyCode.R))
            commands[1] = "Medieval Start";

        if (commands[1] == "Medieval Start")
        {
            timerManager.GameStart();
            timerManager.MissionStart(AltarState.HolyGrail.ToString());
            timerManager.MissionStart(AltarState.RoundTable.ToString());
            timerManager.MissionStart(AltarState.DragonFang.ToString());
            timerManager.MissionStart(AltarState.Crown.ToString());
            timerManager.MissionStart(AltarState.Altar.ToString());
        }
        if (commands[1].Contains("HOLY_GRAIL_PASS"))
            timerManager.MissionRecord(AltarState.HolyGrail.ToString());
        if (commands[1].Contains("ROUND_TABLE_PASS"))
            timerManager.MissionRecord(AltarState.RoundTable.ToString());
        if (commands[1].Contains("DRAGON_FANG_PASS"))
            timerManager.MissionRecord(AltarState.DragonFang.ToString());
        if (commands[1].Contains("CROWN_PASS"))
            timerManager.MissionRecord(AltarState.Crown.ToString());
    }
    void LateUpdate()
    {
        commands = new string[commandsCount] { "NoWakaka", "NoPage" };
    }
}
