using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAIThree : TowersAI
{

    [SerializeField]
    private ParticleSystem m_ParticleSystem;

    private GameObject partSysGO;

    public float SlowedValue = 0.3f;

    public override void Awake()
    {

        base.Awake();

        m_ParticleSystem.Stop();
        partSysGO = new GameObject();
        partSysGO.transform.position = m_ParticleSystem.gameObject.transform.position;
        m_ParticleSystem.gameObject.transform.parent = partSysGO.transform;

    }

    public override void Update()
    {
                  
        if (isSlowWeapon)
        {

            if (isEnemyClosest) m_Transform.RotateAroundLocal(Vector3.up, -2.0f * Time.deltaTime);
            else if (m_ParticleSystem.isPlaying) m_ParticleSystem.Stop();

            SlowerEnemy();

        }
        

    }

    private void SlowerEnemy()
    {

        isEnemyClosest = false;

        for (int i = 0; i < GameManager.AllPawnTransform.Count; i++)
        {

            tempDistance = Vector3.Distance(m_Transform.position, GameManager.AllPawnTransform[i].position);

            if (tempDistance <= radiusAOE)
            {

                GameManager.AllPawn[i].ChangeSpeed(GameManager.AllPawn[i].m_EnemyAI.speed * SlowedValue);

                GameManager.AllPawn[i].ChangeSpeed(GameManager.AllPawn[i].HP -= damage * Time.deltaTime);

                partSysGO.transform.position = gameObject.transform.position;

                if (m_ParticleSystem.isStopped) m_ParticleSystem.Play();

                isEnemyClosest = true;

            }

        }

    }

}
