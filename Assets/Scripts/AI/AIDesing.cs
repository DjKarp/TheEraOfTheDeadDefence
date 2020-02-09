using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AIDesing : ScriptableObject
{

    [Header("Максимальное количество здоровья.")]
    [SerializeField]
    public float maxHP;

    [Header("Скорость движения.")]
    [SerializeField]
    public float Speed;

    [Header("Стоимость при убийстве.")]
    [SerializeField]
    public int Price;

}
