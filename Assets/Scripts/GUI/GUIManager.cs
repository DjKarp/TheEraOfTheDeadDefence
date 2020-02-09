using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{

    //private static GUIManager instance = null;

    [SerializeField]
    private TextMeshProUGUI repaitPartsCount;
    private GameObject repaitPartsCountGO;

    [Header("GameMenu")]
    [SerializeField]
    private Button buildOneBtn;
    [SerializeField]
    private Button buildTwoBtn;
    [SerializeField]
    private Button buildThreeBtn;
    private GameObject buildOneBtnGO, buildTwoBtnGO, buildThreeBtnGO;
    private Transform buildOneBtnTr, buildTwoBtnTr, buildThreeBtnTr;
    private Text buildOneBtnText, buildTwoBtnText, buildThreeBtnText;    

    [Header("Подсказка")]
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private Text tooltipText;
    private float tooltipHideDalay = 2.0f;

    [Header("Обратный отсчёт волн")]
    [SerializeField]
    private GameObject waveBackCountTimeGO;
    [SerializeField]
    private TextMeshProUGUI waveBackCountTime;
    private float waveBackCountTimer;

    [Header("Количество оставшихся волн")]
    [SerializeField]
    private GameObject waveCountGO;
    private TextMeshProUGUI waveCount;

    [Header("Кнопка ускорения времени")]
    [SerializeField]
    private Button speedUpTimeButt;
    private TextMeshProUGUI speedUpTimeText;

    [Header("Текст между уровнями")]
    [SerializeField]
    private TextMeshProUGUI historyText;

    [Header("Кнопка скилла ")]
    [SerializeField]
    private Button rocketSkillsBtn;
    private TextMeshProUGUI rocketSkillsText;
    private float reloadTimer = 0;

    private float scaleEffectTime = 1.0f;

    private Image m_RocketScillsColorFon;

    private string oneTowerText = "Стрелки - 20RP";
    private string twoTowerText = "Гранатомётчики - 10RP";
    private string threeTowerText = "Химические войска - 10RP";

    private string oneUpgradeText = "Увеличение урона - 22RP";
    private string twoUpgradeText = "Апгрейд башни - 37RP";
    private string threeUpgradeText = "Увеличение радиуса - 18RP";

    private int tempInt;
    private int tempInt2;


    /*
    public static GUIManager Instance
    {

        get
        {
            if (instance == null)
            {

                instance = FindObjectOfType<GUIManager>();

                if (instance == null)
                {

                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    instance = go.AddComponent<GUIManager>();
                    DontDestroyOnLoad(go);

                }

            }

            return instance;

        }

    }
    */
    private void Awake()
    {
        /*
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {

            Destroy(gameObject);

        }
        */
        GameManager.EventChangeGameMode += OnChangeGameMode;

        buildOneBtnGO = buildOneBtn.gameObject;
        buildTwoBtnGO = buildTwoBtn.gameObject;
        buildThreeBtnGO = buildThreeBtn.gameObject;
        buildOneBtnTr = buildOneBtnGO.transform;
        buildTwoBtnTr = buildTwoBtnGO.transform;
        buildThreeBtnTr = buildThreeBtnGO.transform;
        buildOneBtnText = buildOneBtnGO.GetComponentInChildren<Text>();
        buildTwoBtnText = buildTwoBtnGO.GetComponentInChildren<Text>();
        buildThreeBtnText = buildThreeBtnGO.GetComponentInChildren<Text>();

        repaitPartsCountGO = repaitPartsCount.gameObject;
        repaitPartsCount.text = GameManager.repairPartsCount.ToString();
        
        waveCount = waveCountGO.GetComponent<TextMeshProUGUI>();

        rocketSkillsText = rocketSkillsBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        speedUpTimeButt.onClick.AddListener(TimeSpeedUpButtonClick);
        speedUpTimeText = speedUpTimeButt.GetComponentInChildren<TextMeshProUGUI>();
        m_RocketScillsColorFon = rocketSkillsBtn.gameObject.GetComponent<Image>();

        rocketSkillsBtn.onClick.AddListener(StartRocket);

        waveBackCountTimeGO.SetActive(false);

        tooltip.SetActive(false);
                
        StartBuildBtnOn();

        currentLevelCount = 1;

    }

    private void Start()
    {

        GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length);

        GameManager.m_GUIManager.WaveBackCountIsNow(spawnWaveTimer);
        GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length - EnemyTypesCount);

    }

    private void Update()
    {

        if (GameManager.CurrentGameMode == GameManager.GameMode.Play)
        {

            WaveBackCount();
            SpawnEnemy();
            ReloadTimer();
            m_Timer += Time.deltaTime;

        }

    }

    public void OnChangeGameMode()
    {

        if (GameManager.CurrentGameMode == GameManager.GameMode.Winner || GameManager.CurrentGameMode == GameManager.GameMode.Winner)
        {

            if (Time.timeScale != 1) Time.timeScale = 1;

        }

        if (GameManager.CurrentGameMode == GameManager.GameMode.Play)
        {

            //MyStartCoroutine();
            EnemyWaweCount = 0;
            m_Timer = 0;
            EnemyTypesCount = 0;

        }

    }

    public void OnChengeMouseMode()
    {

        switch (GameManager.CurrentMouseMode)
        {

            case GameManager.MouseMode.Empty:
                StartBuildBtnOn();
                break;

            case GameManager.MouseMode.Build:
                StartBuildBtnOn();
                break;

            case GameManager.MouseMode.ChooseBld:
                EnableChooseBuildBtn();
                break;

            case GameManager.MouseMode.Upgrade:
                EnableUpgradeBuildBtn();
                break;

            case GameManager.MouseMode.Skills:
                reloadTimer = 60.0f;
                break;

        }

    }
    
    public void EnableChooseBuildBtn()
    {

        EnableAndActiveButton();

        buildOneBtnText.text = oneTowerText;
        buildTwoBtnText.text = twoTowerText;
        buildThreeBtnText.text = threeTowerText;

    }

    public void EnableUpgradeBuildBtn()
    {

        EnableAndActiveButton();

        buildOneBtnText.text = oneUpgradeText;
        buildTwoBtnText.text = twoUpgradeText;
        buildThreeBtnText.text = threeUpgradeText;

    }

    private void EnableAndActiveButton()
    {

        buildOneBtnGO.SetActive(true);
        buildTwoBtnGO.SetActive(true);
        buildThreeBtnGO.SetActive(true);

        buildOneBtnTr.position = new Vector3(Input.mousePosition.x - 150, Input.mousePosition.y, 0.0f);
        buildTwoBtnTr.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 150, 0.0f); ;
        buildThreeBtnTr.position = new Vector3(Input.mousePosition.x + 150, Input.mousePosition.y, 0.0f); ;

        buildOneBtnTr.localScale = Vector3.zero;
        buildTwoBtnTr.localScale = Vector3.zero;
        buildThreeBtnTr.localScale = Vector3.zero;

        ApplyAnumationScale(buildOneBtnGO);
        ApplyAnumationScale(buildTwoBtnGO);
        ApplyAnumationScale(buildThreeBtnGO);

    }

    public void StartBuildBtnOn()
    {

        buildOneBtnGO.SetActive(false);
        buildTwoBtnGO.SetActive(false);
        buildThreeBtnGO.SetActive(false);

    }
    
    public void AddRepairParts(int price)
    {

        GameManager.repairPartsCount += price;
        ApplyChangeRapairPartsCount();

    }

    public bool SubtractRepairParts(int price)
    {

        if (price <= GameManager.repairPartsCount)
        {

            GameManager.repairPartsCount -= price;
            ApplyChangeRapairPartsCount();

            return true;

        } 
        else
        {

            return false;

        }

    }

    public void ReloadTimer()
    {

        if (reloadTimer > 0)
        {

            reloadTimer -= Time.deltaTime;

            rocketSkillsText.text = Mathf.RoundToInt(reloadTimer).ToString();            

        }
        else
        {

            reloadTimer = 0; ;
            rocketSkillsText.text = "ракетный удар";

        }

    }

    private void ApplyChangeRapairPartsCount()
    {

        repaitPartsCount.text = GameManager.repairPartsCount.ToString();
        ScaleEffectOnEnableForText(repaitPartsCountGO);

    }

    public void ShowToolTip(string message, bool isRepairParts)
    {

        tooltip.SetActive(true);
        tooltipText.text = message;
        if (isRepairParts) ScaleEffectOnEnableForText(repaitPartsCountGO);
        StartCoroutine(CoroutineHideToolTip());

    }

    IEnumerator CoroutineHideToolTip()
    {

        yield return new WaitForSeconds(tooltipHideDalay);

        tooltip.SetActive(false);

    }

    public void WaveBackCountIsNow(int time)
    {

        tempInt = time;
        waveBackCountTimeGO.SetActive(true);
        waveBackCountTimer = time;

    }

    public void WaveBackCount()
    {

        if (waveBackCountTimer >= 0)
        {

            if (m_Timer > 1.4f)m_Timer = 1.4f;
            waveBackCountTimer -= Time.deltaTime;
            tempInt2 = Mathf.RoundToInt(waveBackCountTimer);
            if (tempInt2 < tempInt)
            {

                tempInt = tempInt2;
                waveBackCountTime.text = tempInt2.ToString();
                ScaleEffectOnEnableForText(waveBackCountTimeGO);

            }

        }
        else if (waveBackCountTimeGO.activeSelf) waveBackCountTimeGO.SetActive(false);

    }

    public void SetWaveCount(int countValue)
    {

        waveCount.text = countValue.ToString();
        ScaleEffectOnEnableForText(waveCountGO);

    }

    public void TimeSpeedUpButtonClick()
    {

        if (Time.timeScale == 1)
        {

            speedUpTimeText.text = "замедлить время";
            Time.timeScale = 5;

        }
        else
        {

            speedUpTimeText.text = "ускорить время";
            Time.timeScale = 1;

        }

    }  

    public void StartRocket()
    {

        if (reloadTimer == 0) GameManager.ChangeMouseMode(GameManager.MouseMode.Skills);

    }

    private void ApplyAnumationScale(GameObject go)
    {

        LeanTween.scale(go, Vector3.one, scaleEffectTime)
            .setEase(LeanTweenType.easeOutElastic);

    }
    private void ScaleEffectOnEnableForText(GameObject go)
    {

        LeanTween.scale(go, Vector3.one, scaleEffectTime / 2)
            .setFrom(Vector3.zero * 0.5f)
            .setEase(LeanTweenType.easeOutBack);

    }

    public void ChangeHistoryText(int number)
    {


    }

    public void SpawnEnemy()
    {

        if(m_Timer > spawnEnemyTimer && EnemyWaweCount < m_Level.EnemyCount.Length)
        {                   
                       

            switch (m_Level.enemyTypes[EnemyWaweCount])
            {

                case Level.EnemyType.FirstEnemyType:
                    tempGO = new GameObject();
                    tempGO2 = new GameObject();
                    tempGO = Instantiate(GameManager.EnemyGO[0]);
                    tempGO2 = Instantiate(GameManager.EnemyGO[0]);
                    break;

                case Level.EnemyType.TwoEnemyType:
                    tempGO = new GameObject();
                    tempGO = Instantiate(GameManager.EnemyGO[1]);
                    break;

                case Level.EnemyType.ThreeEnemyType:
                    tempGO = new GameObject();
                    tempGO2 = new GameObject();
                    GameObject tempGO3 = new GameObject();
                    GameObject tempGO4 = new GameObject();
                    tempGO = Instantiate(GameManager.EnemyGO[2]);
                    tempGO2 = Instantiate(GameManager.EnemyGO[2]);
                    tempGO3 = Instantiate(GameManager.EnemyGO[2]);
                    tempGO4 = Instantiate(GameManager.EnemyGO[2]);
                    break;

                case Level.EnemyType.ForEnemyType:
                    tempGO = new GameObject();
                    tempGO = Instantiate(GameManager.EnemyGO[3]);
                    break;

                case Level.EnemyType.FiveEnemyType:
                    tempGO = new GameObject();
                    tempGO = Instantiate(GameManager.EnemyGO[4]);
                    break;

            }

            tempGO.transform.position = GameManager.EnemyExitPointTr.position;
            tempGO2.transform.position = GameManager.EnemyExitPointTr.position;

            EnemyWaweCount++;
            m_Timer -= spawnEnemyTimer;

        }
        else if(m_Timer > spawnWaveTimer && EnemyTypesCount < m_Level.EnemyCount.Length)
        {

            GameManager.m_GUIManager.WaveBackCountIsNow(spawnWaveTimer);
            GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length - EnemyTypesCount - 1);

            EnemyTypesCount++;
            EnemyWaweCount = 0;
            m_Timer -= spawnWaveTimer;

        }
        else
        {

            if (GameManager.AllPawn.Count <= 0 & GameManager.EscapingCount > 0 & EnemyWaweCount >= m_Level.EnemyCount.Length & EnemyTypesCount >= m_Level.EnemyCount.Length)                
            {

                EnemyWaweCount = 0;
                m_Timer = 0;
                EnemyTypesCount = 0;

                GameManager.ChangeMode(GameManager.GameMode.Winner);
                
            }

        }

    }

    public void NextLev()
    {

        GameManager.NextLevel();

    }

    public void Lev3()
    {

        GameManager.Level3();

    }

    public Level m_Level;
    public int currentLevelCount;

    private float m_Timer;
    private float spawnEnemyTimer = 1.5f;
    private int spawnWaveTimer = 5;
    private bool isSpawn = false;
    private int EnemyWaweCount = 0;
    private int EnemyTypesCount = 0;

    private GameObject tempGO, tempGO2;

    private bool isStart = false;
    /*
    public void MyStartCoroutine()
    {

        StartCoroutine(CourotineSpawner());

    }
    
    IEnumerator CourotineSpawner()
    {

        GameManager.m_GUIManager.WaveBackCountIsNow(5);

        yield return new WaitForSeconds(5);

        for (int i = 0; i < m_Level.enemyTypes.Length; i++)
        {

            for (int j = 0; j < m_Level.EnemyCount[i]; j++)
            {

                tempGO = new GameObject();

                switch (m_Level.enemyTypes[i])
                {

                    case Level.EnemyType.FirstEnemyType:
                        tempGO2 = new GameObject();
                        tempGO = Instantiate(GameManager.EnemyGO[0]);
                        tempGO2 = Instantiate(GameManager.EnemyGO[0]);
                        break;

                    case Level.EnemyType.TwoEnemyType:
                        tempGO = Instantiate(GameManager.EnemyGO[1]);
                        break;

                    case Level.EnemyType.ThreeEnemyType:
                        tempGO2 = new GameObject();
                        GameObject tempGO3 = new GameObject();
                        GameObject tempGO4 = new GameObject();
                        tempGO = Instantiate(GameManager.EnemyGO[2]);
                        tempGO2 = Instantiate(GameManager.EnemyGO[2]);
                        tempGO3 = Instantiate(GameManager.EnemyGO[2]);
                        tempGO4 = Instantiate(GameManager.EnemyGO[2]);
                        break;

                    case Level.EnemyType.ForEnemyType:
                        tempGO = Instantiate(GameManager.EnemyGO[3]);
                        break;

                    case Level.EnemyType.FiveEnemyType:
                        tempGO = Instantiate(GameManager.EnemyGO[4]);
                        break;

                }

                tempGO.transform.position = GameManager.EnemyExitPointTr.position;
                tempGO2.transform.position = GameManager.EnemyExitPointTr.position;

                yield return new WaitForSeconds(spawnEnemyTimer);

            }

            currentLevelCount++;
            GameManager.m_GUIManager.WaveBackCountIsNow(spawnWaveTimer);
            GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length - i - 1);
            yield return new WaitForSeconds(spawnWaveTimer);

        }

        while (GameManager.AllPawn.Count > 0)
        {


            yield return null;

        }

        if (GameManager.EscapingCount > 0)
        {

            GameManager.ChangeMode(GameManager.GameMode.Winner);
            //GameManager.NextLevel();

        }

    }
    */
}
