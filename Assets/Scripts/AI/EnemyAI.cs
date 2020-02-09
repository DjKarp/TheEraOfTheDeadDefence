using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(Pawn))]
public class EnemyAI : MonoBehaviour
{

    private Pawn m_Pawn;
    private AIDesing m_AIDesing;

    public float speed;

    private BehaviorTree m_BehaviorTree;
    private Transform my_Tramsform;

    private Transform targetTransform;

    private float tempFloat;
    private float tempDistance;
    private Transform tempTransform;

    private bool isExit = false;
    private bool IsMoveExit = false;

    private GameObject escaping;


    private void Awake()
    {

        m_BehaviorTree = gameObject.GetComponent<BehaviorTree>();

        my_Tramsform = gameObject.transform;
        tempTransform = my_Tramsform;

        SearchEscaping();

        m_Pawn = gameObject.GetComponent<Pawn>();
        m_AIDesing = m_Pawn.m_AIDesing;

        speed = m_AIDesing.Speed;
        SetSpeed(speed);
        
    }

    private void Update()
    {

        if (GameManager.CurrentGameMode == GameManager.GameMode.Play)
        {

            if (!IsMoveExit) SearchEscaping();
            else if (IsExit() && !isExit) EscapingIsDead();

        }
        
    }

    private void SearchEscaping()
    {

        if (targetTransform == null || !targetTransform.gameObject.activeSelf)
        {

            tempFloat = 1000.0f;

            foreach (Transform m_Transform in GameManager.AllEscapingTransform)
            {

                tempDistance = Vector3.Distance(m_Transform.position, my_Tramsform.position);

                if (tempDistance < tempFloat)
                {

                    tempFloat = tempDistance;
                    tempTransform = m_Transform;

                }

            }

            if (tempTransform != targetTransform)
            {

                targetTransform = tempTransform;
                m_BehaviorTree.SetVariableValue("Target", targetTransform.gameObject);

            }

        }

        if (Vector3.Distance(targetTransform.position, my_Tramsform.position) <= 4) SearchExit(targetTransform.gameObject);

    }

    private void SearchExit(GameObject go)
    {

        tempTransform = GameManager.EnemyExitPoint.transform;
        m_BehaviorTree.SetVariableValue("Target", GameManager.EnemyExitPoint);

        IsMoveExit = true;

        SetSpeed(0);

        escaping = go;
        escaping.SetActive(false);

    }

    private bool IsExit()
    {

        tempDistance = Vector3.Distance(my_Tramsform.position, tempTransform.position);

        if (tempDistance <= 1.0f) return true;
        else return false;

    }

    private void EscapingIsDead()
    {

        isExit = true;
                
        Destroy(escaping);
        m_Pawn.Die();

    }

    public void EscapingIsFree()
    {

        if (escaping != null)
        {

            escaping.transform.position = new Vector3(my_Tramsform.position.x, escaping.transform.position.y, my_Tramsform.position.z);
            escaping.SetActive(true);

        }       
        
    }

    private void SetSpeed(float newSpeed)
    {

        speed = Mathf.Clamp(newSpeed, 0.0f, 1000);
        m_BehaviorTree.SetVariableValue("Speed", speed);

    }

    public void ChangeSpeed(float m_speed)
    {

        speed = Mathf.Max(0.0f, speed - m_speed);
        m_BehaviorTree.SetVariableValue("Speed", speed);

    }
    
}
