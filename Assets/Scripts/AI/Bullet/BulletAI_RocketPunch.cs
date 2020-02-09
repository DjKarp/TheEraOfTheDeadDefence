using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAI_RocketPunch : BulletAI
{


    private float explosenTime;
    public float damageScills = 3;
    public float radiusScills = 100;

    private ParticleSystem m_ParticleSystem;

    public override void Awake()
    {

        m_Transform = gameObject.transform;
        
        if (explousen != null) startScale = explousen.transform.localScale;

        explosenTime = 0;

        m_ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        //m_ParticleSystem.Stop();

    }

    public override void Initialized(TowersAI tAI)
    {


    }

    public override void ChekCollisionOnTarget()
    {

        explosenTime += Time.deltaTime;        

        if (explosenTime >= 1.5f)
        {

            LeanTween.scale(explousen, Vector3.one * radiusScills, 0.6f)
                        .setEase(LeanTweenType.easeOutBounce);

            for (int i = 0; i < GameManager.AllPawnTransform.Count; i++)
            {

                distance = Vector3.Distance(m_Transform.position, GameManager.AllPawnTransform[i].position);

                if (distance <= radiusScills) GameManager.AllPawn[i].Damage(damageScills);

            }

            Destroy(gameObject, 1.5f);

        }

    }

}
