using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    private float health;
    private float mana;
    private float damage;
    private PlayerAbilities ability1;
    private PlayerAbilities ability2;
    private float resistance;
    private int level;

    [SerializeField]
    private PlayerStatData playerStats;


    void Awake()
    {
        health = playerStats.GetHealth();
        mana = playerStats.GetMana();
        damage = playerStats.GetFinalDamage();
        ability1 = playerStats.GetAbilities()[0];
        ability2 = playerStats.GetAbilities()[1];
        resistance = playerStats.ReturnResistanceAmount();
        level = playerStats.GetLevel();
    }

    void Update()
    {
        
    }

    public float DamageTest()
    {
        return damage;
    }
}
