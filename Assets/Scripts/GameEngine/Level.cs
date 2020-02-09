using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject
{

    [Header("Волны врагов. Их вид.")]
    [SerializeField] public EnemyType[] enemyTypes;

    [Header("Численность врагов в волнах")]
    [SerializeField] public int[] EnemyCount;
    
    
    public enum EnemyType
    {

        FirstEnemyType,
        TwoEnemyType,
        ThreeEnemyType,
        ForEnemyType,
        FiveEnemyType

    }

}


