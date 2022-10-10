using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "ScriptableObjects/Enemy Base Stats")]
public class EnemyBaseData : ScriptableObject
{
    public float enemyHealth = 100;
    public float enemyDamage = 1;

    public float ReturnHealth()
    {
        return enemyHealth;
    }

    public float ReturnBaseDamage()
    {
        return enemyDamage;
    }
}
