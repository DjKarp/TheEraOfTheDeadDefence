using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(BehaviorTree))]
public class TowersAI : MonoBehaviour
{

    public TowersType thisTower;
    public enum TowersType
    {

        one,
        two,
        three

    }

    public int towerLevel;

    public float damage = 1.0f;
    public float reload = 1.0f;
    public float radius = 1.0f;

    public bool isDamageAOE = false;
    public float radiusAOE = 1.0f;

    public bool isSlowWeapon = false;

    public Transform m_Transform;

    public float m_Timer;

    public GameObject pulya;
    public Transform[] shootPointTr;
    private List<GameObject> pulyaPool = new List<GameObject>();
    private List<Transform> pulyaPoolTr = new List<Transform>();
    private List<BulletAI> pulyaPoolBulletAI = new List<BulletAI>();

    public Transform target;
    public Pawn targetPawn;

    private BehaviorTree m_BehaviorTree;

    public bool isEnemyClosest = false;

    public float tempDistance;
    public float tempFloat;
    public GameObject tempGO;





    public virtual void Awake()
    {

        m_Transform = gameObject.transform;
        m_BehaviorTree = gameObject.GetComponent<BehaviorTree>();
        m_BehaviorTree.enabled = false;

        if (!isSlowWeapon)
        {

            pulyaPool = new List<GameObject>();
            for (int i = 0; i < 10; i++)
            {

                pulyaPool.Add(Instantiate(pulya, m_Transform));
                pulyaPoolTr.Add(pulyaPool[pulyaPool.Count - 1].transform);
                pulyaPoolBulletAI.Add(pulyaPool[pulyaPool.Count - 1].GetComponent<BulletAI>());
                pulyaPool[pulyaPool.Count - 1].SetActive(false);

            }

        }

        towerLevel = 0;
        
    }

    private void OnDestroy()
    {


    }

    public virtual void Update()
    {

        if (target == null)
        {

            SeachTarget();

        }
        else
        {

            if (!m_BehaviorTree.enabled) m_BehaviorTree.enabled = true;

            tempGO = m_BehaviorTree.GetVariable("Target").GetValue() as GameObject;
            if (tempGO != target.gameObject) m_BehaviorTree.SetVariableValue("Target", target.gameObject);

        }

        if (m_Timer > 0) m_Timer -= Time.deltaTime;
        else
        {

            if (target != null) Shoot();

        }

    }

    public virtual void SeachTarget()
    {

        tempFloat = 1000;

        for (int i = 0; i < GameManager.AllPawnTransform.Count; i++)
        {

            tempDistance = Vector3.Distance(GameManager.AllPawnTransform[i].position, m_Transform.position);
            if (tempDistance < tempFloat)
            {

                tempFloat = tempDistance;
                target = GameManager.AllPawnTransform[i];
                targetPawn = GameManager.AllPawn[i];

            }

        }

    }
    
    public virtual void Shoot()
    {

        tempDistance = Vector3.Distance(target.position, m_Transform.position);
        if (tempDistance < radius)
        {

            m_Timer = reload;

            for (int j = 0; j < shootPointTr.Length; j++)
            {

                for (int i = 0; i < pulyaPool.Count; i++)
                {

                    if (!pulyaPool[i].activeSelf)
                    {

                        pulyaPool[i].SetActive(true);
                        pulyaPoolTr[i].position = shootPointTr[j].position;
                        pulyaPoolBulletAI[i].Initialized(this);

                        break;

                    }

                }

            }

            

        }
        else SeachTarget();

    }
    /*
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);

    }
    */
}
