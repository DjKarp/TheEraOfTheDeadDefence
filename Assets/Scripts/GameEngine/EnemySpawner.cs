using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    public Level m_Level;
    public int currentLevelCount;

    private float m_Timer;
    private float spawnEnemyTimer = 1.5f;
    private int spawnWaveTimer = 5;

    private GameObject tempGO, tempGO2;

    private bool isStart = false;
    

    private void Awake()
    {

       currentLevelCount = 1;
        
        GameManager.EventChangeGameMode += StartWaves;

    }

    private void Start()
    {

        GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length);

    }

    public void StartWaves()
    {

        if (GameManager.CurrentGameMode == GameManager.GameMode.Play)
        {

            StartCoroutine(CourotineSpawner());

        }

    }

    private void OnDisable()
    {

        GameManager.EventChangeGameMode -= StartWaves;

    }

    private void Update()
    {
               
    }
    
    IEnumerator CourotineSpawner()
    {

        GameManager.m_GUIManager.WaveBackCountIsNow(spawnWaveTimer);

        yield return new WaitForSeconds(spawnWaveTimer);

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
    
    public void Spawner333()
    {

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

                //yield return new WaitForSeconds(spawnEnemyTimer);

            }

            currentLevelCount++;
            GameManager.m_GUIManager.WaveBackCountIsNow(spawnWaveTimer);
            GameManager.m_GUIManager.SetWaveCount(m_Level.enemyTypes.Length - i - 1);
            //yield return new WaitForSeconds(spawnWaveTimer);

        }

    }

}
