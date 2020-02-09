using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Pawn : MonoBehaviour
{

    public float HP;
    public float maxHP;

    public int deadPrice;
    
    [SerializeField]
    public AIDesing m_AIDesing;

    public EnemyAI m_EnemyAI;

    public bool isSlower = false;
    public float slowerTime;

    public RectTransform hpBar;


    public void Awake()
    {

        maxHP = m_AIDesing.maxHP;
        HP = maxHP;

        deadPrice = m_AIDesing.Price;

        m_EnemyAI = gameObject.GetComponent<EnemyAI>();

        GameManager.AllPawn.Add(this);
        GameManager.AllPawnTransform.Add(gameObject.transform);

        hpBar = GetComponentInChildren<RectTransform>();

    }

    private void Update()
    {

        CheckDie();

        if (isSlower)
        {

            if (slowerTime > 0) slowerTime -= Time.deltaTime;
            else
            {

                isSlower = false;
                ChangeSpeed(m_EnemyAI.speed);

            }

        }        

    }

    public void Spawn (Vector3 m_position, Quaternion m_rotation)
    {

        transform.position = m_position;
        transform.rotation = m_rotation;

        gameObject.SetActive(true);

        GameManager.AllPawn.Add(this);
        GameManager.AllPawnTransform.Add(gameObject.transform);

    }

    public void Damage(float damage)
    {

        HP = Mathf.Clamp(HP - damage, 0.0f, maxHP);
        CheckDie();

    }
    public void Damage (TowersAI m_Towers)
    {

        if (!m_Towers.isDamageAOE) Damage(m_Towers.damage);
        else Damage(m_Towers.damage / 2);

    }

    private void CheckDie() 
    { 
        
        if (HP <= 0.0f)
        {

            hpBar.localScale = Vector3.zero;
            DieWhitEscaping();

        }
        else
        {

            hpBar.localScale = new Vector3((HP / maxHP), hpBar.localScale.y, hpBar.localScale.z);

        }
    
    }

    public void ChangeSpeed(float subtrackt)
    {

        if (!isSlower)
        {

            isSlower = true;

        }

        slowerTime = 2.0f;

    }

    public void Die()
    {
        
        GameManager.AllPawn.Remove(this);
        GameManager.AllPawnTransform.Remove(gameObject.transform);

        GameManager.m_GUIManager.AddRepairParts(deadPrice);

        Destroy(gameObject, 0.0f);

    }

    public void DieWhitEscaping()
    {

        m_EnemyAI.EscapingIsFree();

        Die();

    }

}
