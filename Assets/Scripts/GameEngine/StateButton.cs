using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StateButton : MonoBehaviour
{

    [Header("Кнопка. Оставте пустым, если скрипт на кнопке.")]
    [SerializeField] 
    private Button m_Button;

    [Header("В какой игровой режим переводит кнопка.")]
    [SerializeField] 
    private GameManager.GameMode m_GameMode;


    private void Awake()
    {

        if (m_Button == null) m_Button = gameObject.GetComponent<Button>();

        m_Button.onClick.AddListener(TaskOnClick);

    }

    private void Reset()
    {

        m_Button = gameObject.GetComponent<Button>();
        if (m_Button == null) m_Button = gameObject.AddComponent<Button>();

    }

    void TaskOnClick()
    {

        GameManager.ChangeMode(m_GameMode);

    }

}
