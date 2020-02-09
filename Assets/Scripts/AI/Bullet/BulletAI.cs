using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(BehaviorTree))]
public class BulletAI : MonoBehaviour
{

    public TowersAI m_TowersAI;
    
    public Transform m_Transform;
    public Transform targetTransform;
    public Pawn targetPawn;

    public BehaviorTree m_BehaviorTree;
    public Rigidbody m_Rigidbody;

    public float distance;

    public Vector3 startScale;

    public Vector3 direction;

    public GameObject explousen;

    public bool tempBool;
    private GameObject tempGO;


    public virtual void Awake()
    {

        m_Transform = gameObject.transform;
        if (explousen != null) startScale = explousen.transform.localScale;

        m_BehaviorTree = gameObject.GetComponent<BehaviorTree>();
        m_BehaviorTree.enabled = false;

        m_Rigidbody = gameObject.GetComponent<Rigidbody>();

    }

    public virtual void Initialized(TowersAI tAI)
    {

        m_TowersAI = tAI;

        targetTransform = m_TowersAI.target;
        targetPawn = m_TowersAI.targetPawn;

        m_BehaviorTree.enabled = true;

        if (!m_TowersAI.isDamageAOE)
        {

            m_BehaviorTree.SetVariableValue("Target", targetTransform.gameObject);

        }
        else
        {

            tempGO = new GameObject();
            direction = new Vector3(targetTransform.position.x, -0.5f, targetTransform.position.z);
            tempGO.transform.position = direction;
            m_BehaviorTree.SetVariableValue("Target", tempGO);

            LeanTween.rotateAround(gameObject, Vector3.up, 180, 1.0f);
            LeanTween.rotateAround(gameObject, Vector3.left, 180, 1.2f);

        }

    }    

    public virtual void Update()
    {

        ChekCollisionOnTarget();
        
    }
    
    public virtual void ChekCollisionOnTarget()
    {

        if (targetTransform != null)
        {

            if (!m_TowersAI.isDamageAOE)
            {

                distance = Vector3.Distance(m_Transform.position, targetTransform.position);

                if (distance < 0.1f)
                {

                    if (targetPawn.HP > 0) targetPawn.Damage(m_TowersAI);

                    m_BehaviorTree.enabled = false;

                    gameObject.SetActive(false);

                }

            }
            else
            {

                if (m_Transform.position.y < 0.0f)
                {

                    if (m_BehaviorTree.enabled)
                    {

                        m_BehaviorTree.enabled = false;

                        LeanTween.scale(explousen, Vector3.one * m_TowersAI.radiusAOE * 10, 0.5f)
                            .setEase(LeanTweenType.easeOutBounce)
                            .setOnComplete(EndExplosen);

                        for (int i = 0; i < GameManager.AllPawnTransform.Count; i++)
                        {

                            distance = Vector3.Distance(m_Transform.position, GameManager.AllPawnTransform[i].position);

                            if (distance <= m_TowersAI.radiusAOE) GameManager.AllPawn[i].Damage(m_TowersAI);

                        }

                        //gameObject.SetActive(false);

                    }

                }

            }

        }
        else gameObject.SetActive(false);

    }

    public void EndExplosen()
    {

        explousen.transform.localScale = startScale;

        gameObject.SetActive(false);


    }

}
