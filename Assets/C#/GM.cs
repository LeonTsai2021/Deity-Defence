using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//新版本場景切換寫法
using UnityEngine.SceneManagement;
//使用Unity UI 程式庫
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [Header("遊戲暫停UI物件")]
    public GameObject PauseUI;

    [Header("產生怪物物件")]
    public GameObject Npc;
    [Header("要產生怪物的物件位置")]
    public GameObject CreateNpcPos;
    [Header("一個關卡內會有多少隻NPC")]
    public float TotalNpcNum;
    //在程式中計算場景上有多少怪物數量
    int ScriptNpcNum;
    [Header("固定每幾秒產生一隻NPC")]
    public float CreateTime;

    [Header("剩下多少怪物數量的Bar")]
    public Image MonsterNumBar;
    //怪物死亡數量
    public float MonsterDieNum;

    [Header("玩家血條")]
    public Image PlayerHPBar;
    [Header("玩家總血量")]
    public float TotalPlayerHP;
    float ScriptHP;
    [Header("怪物攻擊防禦牆扣玩家多少血量")]
    public float HurtPlayerHP;

    [Header("遊戲結束的UI")]
    public GameObject GameOverUI;
    [Header("分數的文字")]
    public Text ScoreText;
    int TotalScore;
    [Header("打死一隻怪物加分")]
    public int AddScore;

    [Header("Boss")]
    public GameObject Boss;

    [Header("信仰條")]
    public Image MagicBar;
    [Header("大絕招的圖")]
    public Image MagicImage;

    [Header("設定多少時間以後累積滿信仰條")]
    public float MagicTimer;
    [Header("程式中計算累積信仰條")]
    public float ScriptMagicTimer;


    //遊戲結束的下一戰按鈕
    [Header("下一戰")]
    public Button NextGame;

    [Header("關卡文字")]
    public Text LevelText, GameOverText;

    [Header("遊戲結束畫面Bonus的分數")]
    public int AddBonus;
    [Header("遊戲結束畫面Bonus文字")]
    public Text BonusText;
    [Header("遊戲結束畫面總分數文字")]
    public Text TotalScoreText;
    // Start is called before the first frame update

    void Awake()
    {
        //關卡文字讀取儲存數值
        LevelText.text = StaticVar.SaveLevelID.ToString();
        //遊戲結束的關卡文字讀取儲存數值
        GameOverText.text = StaticVar.SaveLevelID.ToString();
        //怪物數量讀取儲存數值
        TotalNpcNum = StaticVar.SaveNPCNum;
    }
    void Start()
    {
        //InvokeRepeating持續呼叫function以秒為單位(要呼叫的function名稱,遊戲一開始要等待幾秒才呼叫function,第一次結束以後固定每隔幾秒再繼續呼叫function)
        InvokeRepeating("CreateNpc", CreateTime, CreateTime);

        //玩家程式中的血量=屬性面板中的血量值
        ScriptHP = TotalPlayerHP;

        //信仰條預設為0
        MagicBar.fillAmount = 0;
        //大絕招圖片顏色預設為灰色
        MagicImage.color = Color.gray;
    }
    void CreateNpc() {
        //GetComponent<Collider>().bounds.max抓到Collider邊界最大值
        //GetComponent<Collider>().bounds.min抓到Collider邊界最小值
        Vector3 MaxBounds = CreateNpcPos.GetComponent<Collider>().bounds.max;
        Vector3 MinBounds = CreateNpcPos.GetComponent<Collider>().bounds.min;
        //如果程式計數的怪物數量<自設定的總體怪物數量，代表可以繼續生成怪物
        if (ScriptNpcNum < TotalNpcNum) {
            //動態生成怪物
            Instantiate(Npc, new Vector3(Random.Range(MinBounds.x, MaxBounds.x), MinBounds.y, MinBounds.z), CreateNpcPos.transform.rotation);
            //怪物數量累加
            ScriptNpcNum++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //敵妖量顯示剩下多少怪物
        MonsterNumBar.fillAmount = 1f-(MonsterDieNum / TotalNpcNum);
        //如果怪物死亡數量=每個關卡的總數量
        if (MonsterDieNum == TotalNpcNum) {
            //如果場景上沒有任何的Boss
            if (GameObject.FindGameObjectsWithTag("Boss").Length < 1)
            {
                //就動態生成一隻Boss
                Instantiate(Boss, CreateNpcPos.transform.position, CreateNpcPos.transform.rotation);
            }
        }
        //時間進行累加
        ScriptMagicTimer += Time.deltaTime;
        //大絕招條的值=程式中計算的時間/預設定時間
        MagicBar.fillAmount = ScriptMagicTimer / MagicTimer;
        //程式中計算的時間>=預設定時間
        if (ScriptMagicTimer >= MagicTimer)
        {
            //大絕招的圖片變亮
            MagicImage.color = Color.white;
        }
    }
    public void PauseGame() {
        //遊戲整體環境時間暫停，但此寫法不會影響Update運作
        Time.timeScale = 0;
        //遊戲暫停開啟
        PauseUI.SetActive(true);
    }
    public void ContinueGame() {
        //遊戲整體環境時間復原
        Time.timeScale = 1;
        //遊戲暫停開啟
        PauseUI.SetActive(false);
    }
    public void BackMenu() {
        //遊戲整體環境時間復原
        Time.timeScale = 1;
        //新版本切換到下一個場景寫法 SceneManager.LoadScene("場景名稱");
        SceneManager.LoadScene("SampleScene");
    }
    //玩家被怪物攻擊

    //玩家被Boss攻擊
    public void HurtPlayer() {
        ScriptHP -= HurtPlayerHP;
        PlayerHPBar.fillAmount = ScriptHP / TotalPlayerHP;
        if (ScriptHP <= 0)
        {
            //遊戲結束
            GameOver(false);
        }
    }
    public void BossHurtPlayer()
    {
        ScriptHP -= HurtPlayerHP * 2f;
        PlayerHPBar.fillAmount = ScriptHP / TotalPlayerHP;
        if (ScriptHP <= 0)
        {
            //遊戲結束
            GameOver(true);
        } 
    }
    //顯示遊戲分數
    public void Score() {
        TotalScore += AddScore;
        ScoreText.text = TotalScore + "";
    }
    public void BossScore()
    {
        TotalScore += AddScore * 100;
        ScoreText.text = TotalScore + "";
    }
    public void GameOver(bool ControlButton)
    {
        if (ControlButton)
        {
            AddBonus = 10000;
        }
        else
        {
            AddBonus = 0;
        }
        TotalScore += AddBonus;
        BonusText.text = AddBonus + "";
        TotalScoreText.text = TotalScore + "";
        //如果玩家死亡就無法點下一戰;如果boss死亡就可以點
        NextGame.interactable = ControlButton;
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void Regame()
    {
        Time.timeScale = 1;
        //Application.loadedLevel讀取當前關卡ID
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Next_Game()
    {
        Time.timeScale = 1;
        StaticVar.SaveLevelID++;
        StaticVar.SaveNPCNum += 3;
        Application.LoadLevel(Application.loadedLevel);
    }

}
