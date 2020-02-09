using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangeOnTime : MonoBehaviour
{

    [Header("Время, через которое надо сменить стейт")]
    [SerializeField]
    [Range(1, 30)]
    private int Timer = 1;
    private float tempTimer;

    [Header("Какой этот игровой режим.")]
    [SerializeField]
    private GameManager.GameMode m_GameModeNow;

    [Header("В какой игровой режим перейдём по истечению времени.")]
    [SerializeField]
    private GameManager.GameMode m_GameModeNext;




    private void Awake()
    {

        tempTimer = Timer;

    }

    private void Update()
    {
        
        if (GameManager.CurrentGameMode == m_GameModeNow)
        {

            if (tempTimer > 0) tempTimer -= Time.deltaTime;
            else GameManager.ChangeMode(m_GameModeNext);

            if (Input.anyKeyDown | Input.GetMouseButtonDown(0)) tempTimer = 0;

        }

    }


}
