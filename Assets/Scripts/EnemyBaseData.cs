using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "ScriptableObjects/Enemy Base Stats")]
public class EnemyBaseData : ScriptableObject
{
    public float enemyHealth = 100f;
    public float enemyLevelMultiplier = 1.75f;
    public float enemyDamage = 1f;
    private float enemyHealthMultiplier;

    private void Awake()
    {
        enemyDamage *= enemyLevelMultiplier;
        enemyHealthMultiplier = enemyLevelMultiplier / 2;
        enemyHealth += enemyHealth * enemyHealthMultiplier;
    }

    private void OnValidate()
    {
        enemyDamage *= enemyLevelMultiplier;
        enemyHealthMultiplier = enemyLevelMultiplier / 2;
        enemyHealth += enemyHealth * enemyHealthMultiplier;
    }

    public float ReturnHealth()
    {
        return enemyHealth;
    }

    public float ReturnBaseDamage()
    {
        return enemyDamage;
    }
}
