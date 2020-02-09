using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{

    public static int repairPartsCount;

    public static int priceOneTower = 20;
    public static int priceTwoTower = 10;
    public static int priceThreeTower = 10;

    public static int priceDamageUpgrade = 22;
    public static int priceTowerUP = 37;
    public static int priceRadiusUpgrade = 18;

    public static LayerMask LayerRoad;
    public static LayerMask LayerBuild;      

    public static event Action EventChangeGameMode;
    public static event Action EventChangeMouseMode;

    public static int Parts;

    public static int EscapingCount;
    public static float Timer;
    public static string SceneName;
    public static float WaitToStartTimer;

    public static Camera m_Camera;
    private Ray m_Ray= new Ray();
    private RaycastHit hit;
    private float rayDistance = 100;
    private float lerpGUISpeed = 1.0f;

    public static GameMode CurrentGameMode = GameMode.Logo;

    public enum GameMode
    {

        Logo,
        StartMenu,
        WaitToStart,
        Play, 
        Pause, 
        Resume,
        Winner,
        Looser,
        Restart, 
        NextLevel,
        Settings,
        ExitGame

    }

    public static MouseMode CurrentMouseMode;

    public enum MouseMode
    {

        Empty, 
        Build, 
        ChooseBld,
        Upgrade,
        Skills

    }
    
    public enum TowersType
    {

        one, 
        two,
        three

    }

    public static List<Pawn> AllPawn = new List<Pawn>();
    public static List<Transform> AllPawnTransform = new List<Transform>();

    public static List<EscapingAI> AllEscapingAI = new List<EscapingAI>();
    public static List<Transform> AllEscapingTransform = new List<Transform>();

    public static List<GameObject> BuildPointGO = new List<GameObject>();
    public static List<MeshRenderer> BuildPointMR = new List<MeshRenderer>();
    public static List<Transform> BuildPointTrn = new List<Transform>();
    public static List<GameObject> BuildPointOld = new List<GameObject>();
    private int oldCountPoint = 0;

    public static List<GameObject> TowersGOOne = new List<GameObject>();
    public static List<GameObject> TowersGOTwo = new List<GameObject>();
    public static List<GameObject> TowersGOThree = new List<GameObject>();

    public static List<GameObject> AllBuildTowers = new List<GameObject>();

    public static List<GameObject> EnemyGO = new List<GameObject>();

    public static GameObject EnemyExitPoint;
    public static Transform EnemyExitPointTr;

    public static EnemySpawner m_EnemySpawner;
    public static GUIManager m_GUIManager;

    private GameObject tree;
    private GameObject bomb;

    private Transform cursorBuildPoint;
    private MeshRenderer cursorBuildPointMR;
    private Material cursorBuildPointMatNo;
    private Material cursorBuildPointMatYes;

    private GameObject upgradeTower;
    private TowersAI m_TowersAI;

    private float tempFloat;
    private int tempInt;
    private string tempString;
    private GameObject tempGO;
    private Transform tempTr;


    public static List<StateMachine> m_StateMashine = new List<StateMachine>();



    private void Awake()
    {

        if (Application.isPlaying)
        {

            RestartLevel();
            Initialization();

        }

    }

    private void Start()
    {

        

    }

    public void Initialization()
    {

        m_Camera = Camera.main;

        LayerRoad = LayerMask.NameToLayer("Road");
        LayerBuild = LayerMask.NameToLayer("BuildPlace");

        tempString = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetString("Level") : "001";

        EnemyExitPoint = GameObject.FindGameObjectWithTag("SpawnAndExitPoint");
        EnemyExitPointTr = EnemyExitPoint.transform;

        tempGO = GameObject.Find("CursorBuildPoint");
        cursorBuildPoint = tempGO.transform;
        cursorBuildPointMR = tempGO.GetComponentInChildren<MeshRenderer>();
        cursorBuildPointMatNo = Resources.Load<Material>("Materials/TransparentMaterialNo");
        cursorBuildPointMatYes = Resources.Load<Material>("Materials/TransparentMaterialYes");
        cursorBuildPointMR.material = cursorBuildPointMatNo;

        tree = Resources.Load<GameObject>("Prefabs/Tree");
        bomb = Resources.Load<GameObject>("Prefabs/Bombs");

        if (BuildPointGO.Count == 0)
        {

            BuildPointGO.AddRange(GameObject.FindGameObjectsWithTag("BuildPoint"));

            foreach (GameObject go in BuildPointGO)
            {

                BuildPointTrn.Add(go.transform);
                BuildPointMR.Add(BuildPointTrn[BuildPointTrn.Count - 1].parent.GetComponent<MeshRenderer>());
                BuildPointMR[BuildPointMR.Count - 1].enabled = false;

            }

            int randomDel = BuildPointGO.Count / 3;
            System.Random m_Random = new System.Random();
            for (int i = 0; i < randomDel; i++)
            {

                int n = m_Random.Next(0, BuildPointGO.Count);
                GameObject GO = Instantiate(tree, gameObject.transform);
                GO.transform.position = BuildPointTrn[n].position;
                GO.transform.rotation = Quaternion.Euler(new Vector3(GO.transform.rotation.x, UnityEngine.Random.Range(90.0f, 360.0f), GO.transform.position.z));
                GO.transform.localScale = new Vector3(UnityEngine.Random.Range(0.01f, 0.15f), UnityEngine.Random.Range(0.01f, 0.18f), UnityEngine.Random.Range(0.01f, 0.1f));
                BuildPointGO.Remove(BuildPointGO[n]);
                BuildPointTrn.Remove(BuildPointTrn[n]);

            }

            foreach (GameObject go in BuildPointGO)
            {

                BuildPointTrn.Add(go.transform);
                BuildPointMR.Add(BuildPointTrn[BuildPointTrn.Count - 1].parent.GetComponent<MeshRenderer>());
                BuildPointMR[BuildPointMR.Count - 1].enabled = false;

            }

        }

        TowersGOOne.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerOne"));
        TowersGOOne.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerOneLevel2"));
        TowersGOOne.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerOneLevel3"));

        TowersGOTwo.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerTwo"));
        TowersGOTwo.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerTwoLevel2"));
        TowersGOTwo.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerTwoLevel3"));

        TowersGOThree.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerThree"));
        TowersGOThree.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerThreeLevel2"));
        TowersGOThree.Add(Resources.Load<GameObject>("Prefabs/Towers/TowerThreeLevel3"));

        for (int i = 1; i < 6; i++) EnemyGO.Add(Resources.Load<GameObject>("Prefabs/Enemy/Enemy0" + i));

        m_StateMashine.AddRange(FindObjectsOfType<StateMachine>());        

        m_GUIManager = FindObjectOfType<GUIManager>();
        m_GUIManager.m_Level = Resources.Load<Level>("LevelWaveDesign/Level" + int.Parse(SceneManager.GetActiveScene().name));

        repairPartsCount = 20;
        if (SceneManager.GetActiveScene().name == "002") repairPartsCount = 100;
        if (SceneManager.GetActiveScene().name == "003") repairPartsCount = 170;

    }
    
    public void RestartLevel()
    {
          
        AllPawn = new List<Pawn>();    
        AllPawnTransform = new List<Transform>();
    
        AllEscapingAI = new List<EscapingAI>();    
        AllEscapingTransform = new List<Transform>();
    
        BuildPointGO = new List<GameObject>();    
        BuildPointMR = new List<MeshRenderer>();    
        BuildPointTrn = new List<Transform>();    
        BuildPointOld = new List<GameObject>();   
        oldCountPoint = 0;
   
        TowersGOOne = new List<GameObject>();     
        TowersGOTwo = new List<GameObject>();    
        TowersGOThree = new List<GameObject>();
    
        AllBuildTowers = new List<GameObject>();
    
        EnemyGO = new List<GameObject>();

        CurrentMouseMode = MouseMode.Empty;

    }

    private void Update()
    {

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        switch (CurrentGameMode)
        {

            case GameMode.Play:
                MouseControl();
                CheckLoose();
                break;

        }

        KeyBoardHack();
        
    }

    private void KeyBoardHack()
    {

        if (Input.GetKeyUp(KeyCode.B)) ChangeMouseMode(MouseMode.Build);
        else if (Input.GetKeyUp(KeyCode.C)) ChangeMouseMode(MouseMode.ChooseBld);
        else if (Input.GetKeyUp(KeyCode.W)) ChangeMode(GameMode.Winner);
        else if (Input.GetKeyUp(KeyCode.A)) m_GUIManager.AddRepairParts(20);

    }

    private void CheckLoose()
    {
        
        if (EscapingCount <= 0) ChangeMode(GameMode.Looser);

    }

    public static void ChangeMode(GameMode m_GameMode)
    {

        if (Application.isPlaying)
        {

            CurrentGameMode = m_GameMode;

            switch (m_GameMode)
            {

                case GameMode.ExitGame:
                    Application.Quit();
                    break;

                case GameMode.Play:
                    break;

                case GameMode.NextLevel:
                    NextLevel();
                    break;

            }
            
            EventChangeGameMode();
            
        }

    }

    public void OnChange()
    {

        switch (CurrentGameMode)
        {

            case GameMode.ExitGame:
                Application.Quit();
                break;

            case GameMode.Play:
                StartCoroutine("MyStartCoroutine");
                break;

            case GameMode.NextLevel:
                break;

        }

    }

    private void StartCoroutine(object v)
    {
        throw new NotImplementedException();
    }

    private void MouseControl()
    {

        switch (CurrentMouseMode)
        {

            case MouseMode.Empty:
                MouseEmptyMode();
                break;

            case MouseMode.Build:
                MouseBuildMode();
                break;

            case MouseMode.ChooseBld:
                MouseChooseBldMode();
                break;

            case MouseMode.Upgrade:
                MouseChooseBldMode();
                break;

            case MouseMode.Skills:
                MouseSkillsdMode();
                break;
                
        }

    }

    public static void ChangeMouseMode(MouseMode m_MouseMode)
    {

        if (Application.isPlaying)
        {

            switch (m_MouseMode)
            {

                case MouseMode.Empty:
                    CurrentMouseMode = MouseMode.Empty;
                    break;

                case MouseMode.Build:
                    CurrentMouseMode = MouseMode.Build;
                    break;

                case MouseMode.ChooseBld:
                    CurrentMouseMode = MouseMode.ChooseBld;
                    break;

                case MouseMode.Upgrade:
                    CurrentMouseMode = MouseMode.Upgrade;
                    break;

                case MouseMode.Skills:
                    CurrentMouseMode = MouseMode.Skills;
                    break;

            }

            m_GUIManager.OnChengeMouseMode();

            //EventChangeMouseMode();

        }

    }

    private void MouseEmptyMode()
    {

        if (Input.GetMouseButtonDown(0))
        {

            m_Ray = m_Camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            if (Physics.Raycast(m_Ray, out hit, rayDistance))
            {

                if (hit.transform.gameObject.tag == "Towers")
                {

                    upgradeTower = hit.transform.parent.gameObject;
                    m_TowersAI = upgradeTower.GetComponent<TowersAI>();

                    ChangeMouseMode(MouseMode.Upgrade);

                }
                else
                {

                    ChangeMouseMode(MouseMode.Build);

                }               

            }

        }           

    }

    private void MouseBuildMode()
    {

        m_Ray = m_Camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        if (Physics.Raycast(m_Ray, out hit, rayDistance))
        {

            if (BuildPointTrn.Count > 0)
            {

                float tempDistance;
                tempFloat = 1000.0f;
                for (int i = 0; i < BuildPointTrn.Count; i++)
                {

                    tempDistance = Vector3.Distance(BuildPointTrn[i].position, new Vector3(hit.point.x, BuildPointTrn[i].position.y, hit.point.z));
                    if (tempDistance < tempFloat)
                    {

                        tempFloat = tempDistance;
                        tempTr = BuildPointTrn[i];
                        oldCountPoint = i;

                    }

                }
                                
                if (tempFloat > 1.6f)
                {

                    cursorBuildPoint.position = new Vector3(hit.point.x, tempTr.position.y, hit.point.z);
                    cursorBuildPointMR.material = cursorBuildPointMatNo;

                }
                else
                {

                    cursorBuildPoint.position = tempTr.position;
                    cursorBuildPointMR.material = cursorBuildPointMatYes;

                    if (Input.GetMouseButtonDown(0))
                    {

                        ChangeMouseMode(MouseMode.ChooseBld);
                        //BuildOneTower();

                    }

                }
                
            }            

        } 
        else
        {

            cursorBuildPoint.position = new Vector3(-100, -100, -100);

        }

        if (Input.GetMouseButtonDown(1)) HideBuildCursor();

    }

    private void MouseChooseBldMode()
    {
        
        if (Input.GetMouseButtonDown(1)/* | (Input.GetMouseButtonDown(0) & EventSystem.current.IsPointerOverGameObject())*/) ChangeMouseMode(MouseMode.Build);

    }

    private void MouseSkillsdMode()
    {

        if (Input.GetMouseButtonDown(0))
        {

            m_Ray = m_Camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            if (Physics.Raycast(m_Ray, out hit, rayDistance))
            {

                if (hit.transform.gameObject.tag == "Plane")
                {

                    Instantiate(bomb, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                    ChangeMouseMode(MouseMode.Empty);

                }

            }

        }

    }


    public void BuildOneTower()
    {

        if (CurrentMouseMode == MouseMode.ChooseBld)
        {
            
            BuildTowers(TowersType.one, priceOneTower);

        } 
        else if (CurrentMouseMode == MouseMode.Upgrade)
        {

            if (m_GUIManager.SubtractRepairParts(priceDamageUpgrade))
            {

                m_TowersAI.damage++;
                HideBuildCursor();

            }
            else
            {

                m_GUIManager.ShowToolTip("Мало запасных частей. Надо ещё - " + (priceDamageUpgrade - repairPartsCount).ToString(), true);

            }            

        } 

    }

    public void BuildTwoTower()
    {

        if (CurrentMouseMode == MouseMode.ChooseBld)
        {

            BuildTowers(TowersType.two, priceTwoTower);

        }
        else if (CurrentMouseMode == MouseMode.Upgrade)
        {

            if (m_TowersAI.towerLevel < 2)
            {

                if (m_GUIManager.SubtractRepairParts(priceTowerUP))
                {

                    m_TowersAI.towerLevel++;

                    switch (m_TowersAI.thisTower)
                    {

                        case TowersAI.TowersType.one:
                            tempGO = Instantiate(TowersGOOne[m_TowersAI.towerLevel]);
                            break;

                        case TowersAI.TowersType.two:
                            tempGO = Instantiate(TowersGOTwo[m_TowersAI.towerLevel]);
                            break;

                        case TowersAI.TowersType.three:
                            tempGO = Instantiate(TowersGOThree[m_TowersAI.towerLevel]);
                            break;

                    }

                    TowersAI towAI = tempGO.GetComponent<TowersAI>();
                    towAI.damage = m_TowersAI.damage;
                    towAI.towerLevel = m_TowersAI.towerLevel;
                    towAI.radius = m_TowersAI.radius;

                    AllBuildTowers.Add(tempGO);
                    AllBuildTowers[AllBuildTowers.Count - 1].transform.position = upgradeTower.transform.position;

                    AllBuildTowers.Remove(upgradeTower);
                    Destroy(upgradeTower);

                    HideBuildCursor();

                }
                else
                {

                    m_GUIManager.ShowToolTip("Мало запасных частей. Надо ещё - " + (priceTowerUP - repairPartsCount).ToString(), true);

                }                

            }
            else
            {

                m_GUIManager.ShowToolTip("Башня максимального уровня.", false);

            }           

        }

    }

    public void BuildThreeTower()
    {

        if (CurrentMouseMode == MouseMode.ChooseBld)
        {

            BuildTowers(TowersType.three, priceThreeTower);

        }
        else if (CurrentMouseMode == MouseMode.Upgrade)
        {

            if (m_GUIManager.SubtractRepairParts(priceRadiusUpgrade))
            {

                m_TowersAI.radius += 2;
                HideBuildCursor();

            }
            else
            {

                m_GUIManager.ShowToolTip("Мало запасных частей. Надо ещё - " + (priceRadiusUpgrade - repairPartsCount).ToString(), true);

            }

        }

    }

    public void BuildTowers(TowersType m_TowersType, int priceTower)
    {              

        if (m_GUIManager.SubtractRepairParts(priceTower))
        {

            switch (m_TowersType)
            {

                case TowersType.one:
                    tempGO = Instantiate(TowersGOOne[0]);
                    break;

                case TowersType.two:
                    tempGO = Instantiate(TowersGOTwo[0]);
                    break;

                case TowersType.three:
                    tempGO = Instantiate(TowersGOThree[0]);
                    break;

            }
                     
            AllBuildTowers.Add(tempGO);
            AllBuildTowers[AllBuildTowers.Count - 1].transform.position = cursorBuildPoint.position;
            HideBuildCursor();

        } 
        else
        {

            m_GUIManager.ShowToolTip("Мало запасных частей. Надо ещё - " + (priceTower - repairPartsCount).ToString(), true);

        }

        
        BuildPointOld.Add(BuildPointGO[oldCountPoint]);
        BuildPointGO.Remove(BuildPointGO[oldCountPoint]);
        BuildPointMR.Remove(BuildPointMR[oldCountPoint]);
        BuildPointTrn.Remove(BuildPointTrn[oldCountPoint]);            

    }

    public void RevertBuildPlace()
    {



    }
    
    public void HideBuildCursor()
    {

        ChangeMouseMode(MouseMode.Empty);
        cursorBuildPoint.position = new Vector3(-100, -100, -100);

    }

    public static void NextLevel()
    {

        ChangeMode(GameMode.Play);

        string tempStr = SceneManager.GetActiveScene().name;

        switch (tempStr)
        {

            case "001":
                SceneManager.LoadScene("002");
                break;

            case "002":
                SceneManager.LoadScene("003");
                break;

            case "003":
                SceneManager.LoadScene("001");
                break;


        }       

    }

    public static void Level3()
    {

        ChangeMode(GameMode.Play);

        SceneManager.LoadScene("003");

    }

}
