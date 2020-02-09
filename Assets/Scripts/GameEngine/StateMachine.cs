using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[RequireComponent(typeof(CanvasGroup))]
public class StateMachine : MonoBehaviour
{

    [SerializeField] 
    private GameManager.GameMode ThisState;

    [SerializeField] 
    private float InSpeed = 1;

    [SerializeField] 
    private float OutSpeed = 4;

    private bool Fade;

    [SerializeField] 
    CanvasGroup m_CanvasGroup;



    void Awake()
    {

        if (m_CanvasGroup == null) m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

        Fade = true;

        GameManager.EventChangeGameMode += Onchange;
        Onchange();

        if (GameManager.CurrentGameMode == ThisState) m_CanvasGroup.alpha = 1;
        else m_CanvasGroup.alpha = 0;
        
    }

    private void Update()
    {
        
        if (Fade == false)
        {

            if (m_CanvasGroup.alpha < 0.01f)
            {

                m_CanvasGroup.alpha = 0;
                m_CanvasGroup.interactable = Fade;
                m_CanvasGroup.blocksRaycasts = Fade;

            }
            else m_CanvasGroup.alpha = Mathf.Lerp(1, 0, OutSpeed);

        }
        else
        {
            
            if (m_CanvasGroup.alpha > 0.99f)
            {

                m_CanvasGroup.interactable = Fade;
                m_CanvasGroup.blocksRaycasts = Fade;
                m_CanvasGroup.alpha = 1;

            }
            else m_CanvasGroup.alpha = Mathf.Lerp(0, 1, InSpeed);

        }

    }

    public void Onchange()
    {
        
        if (GameManager.CurrentGameMode == ThisState) Fade = true;
        else Fade = false;

        m_CanvasGroup.interactable = Fade;
        m_CanvasGroup.blocksRaycasts = Fade;

    }

    private void OnDestroy()
    {

        GameManager.EventChangeGameMode -= Onchange;

    }

#if UNITY_EDITOR

    static StateMachine()
    {

        Selection.selectionChanged += SelectionChange;

    }

    static void SelectionChange()
    {

        if (Selection.activeGameObject && !Selection.activeGameObject.GetComponent<StateMachine>()) return;

        StateMachine[] stateMachines = FindObjectsOfType<StateMachine>();

        for (int i = 0; i < stateMachines.Length; i++) if (stateMachines[i].OnSelectionChange()) break;

        for (int i = 0; i < stateMachines.Length; i++) stateMachines[i].OnSelectionChange();

    }

    public bool OnSelectionChange()
    {

        bool result = false;

        if (!Application.isPlaying)
        {

            if (Selection.activeGameObject == gameObject)
            {

                GameManager.CurrentGameMode = ThisState;
                result = true;

            }

            if (GameManager.CurrentGameMode == ThisState) m_CanvasGroup.alpha = 1;
            else m_CanvasGroup.alpha = 0;

        }

        return result;

    }

#endif

}
