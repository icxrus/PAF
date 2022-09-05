using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "ScriptableObjects/Player Stats")]
public class PlayerStatData : ScriptableObject
{
    private float maxPlayerHealth = 100f;
    private float playerHealth = 100f;
    private float maxPlayerMana = 100f;
    private float playerMana = 100f;
    private float playerBaseDamage = 1f;
    private DamageTypes playerDamageType = DamageTypes.Stab;
    private PlayerAbilities playerAbility1;
    private PlayerAbilities playerAbility2;
    private DamageTypes playerResistance;
    private float playerResistanceAmount = 0f;
    private int playerLevel = 1;
    private int playerDraughtBoost = 0;
    private float levelProgress;

    /// <summary>
    /// Returns current player health.
    /// </summary>
    /// <returns></returns>
    public float GetHealth()
    {
        return playerHealth;
    }

    /// <summary>
    /// Adds float to current health pool. If the pool would go over the max health, the overlap is removed.
    /// </summary>
    /// <param name="healthToBeAdded"></param>
    public void AddHealth(float healthToBeAdded)
    {
        float tempHealth = playerHealth + healthToBeAdded;
        if (tempHealth < maxPlayerHealth)
        {
            playerHealth = tempHealth;
        }
        else
        {
            tempHealth -= maxPlayerHealth;
            playerHealth += healthToBeAdded - tempHealth;
        }
    }

    /// <summary>
    /// Removes float from current health pool. If the pool would go under 0, set health to 0.
    /// </summary>
    /// <param name="healthToBeRemoved"></param>
    public void DecreaseHealth(float healthToBeRemoved)
    {
        float tempHealth = playerHealth - healthToBeRemoved;
        if (tempHealth < 0f)
        {
            playerHealth = 0f;
        }
        else
        {
            playerHealth = tempHealth;
        }
    }

    /// <summary>
    /// Returns current player mana.
    /// </summary>
    /// <returns></returns>
    public float GetMana()
    {
        return playerMana;
    }

    /// <summary>
    /// Adds float to current mana pool. If the pool would go over the max mana, the overlap is removed.
    /// </summary>
    /// <param name="manaToBeAdded"></param>
    public void AddMana(float manaToBeAdded)
    {
        float tempMana = playerMana + manaToBeAdded;
        if (tempMana < maxPlayerMana)
        {
            playerMana = tempMana;
        }
        else
        {
            tempMana -= maxPlayerMana;
            playerMana += manaToBeAdded - tempMana;
        }
    }

    /// <summary>
    /// Removes float from current mana pool. If the pool would go under 0, set mana to 0.
    /// </summary>
    /// <param name="manaToBeRemoved"></param>
    public void DecreaseMana(float manaToBeRemoved)
    {
        float tempMana = playerMana - manaToBeRemoved;
        if (tempMana < 0f)
        {
            playerMana = 0f;
        }
        else
        {
            playerMana = tempMana;
        }
    }

    public float GetDamage()
    {
        return playerBaseDamage;
    }

    public void SetDamage(float newBaseDamage)
    {
        playerBaseDamage = newBaseDamage;
    }

    public float GetFinalDamage()
    {
        float multiplier = 1f;
        if (playerDamageType == DamageTypes.Arcane)
        {
            multiplier = 1f;
        }
        if (playerDamageType == DamageTypes.Draught)
        {
            multiplier = 1.5f;
        }
        if (playerDamageType == DamageTypes.Leech)
        {
            multiplier = 1.5f;
        }
        if (playerDamageType == DamageTypes.Fire)
        {
            multiplier = 2f;
        }
        if (playerDamageType == DamageTypes.Blight)
        {
            multiplier = 2f;
        }
        if (playerDamageType == DamageTypes.Stab)
        {
            multiplier = 1f;
        }

        float finalDmg = playerBaseDamage * multiplier;
        return finalDmg;
    }

    public DamageTypes GetDmgType()
    {
        return playerDamageType;
    }

    public void ChangeDmgType(DamageTypes damageType)
    {
        playerDamageType = damageType;
    }

    public PlayerAbilities[] GetAbilities()
    {
        PlayerAbilities[] abilities = { playerAbility1, playerAbility2 };
        return abilities;
    }

    public void ChangeAbility1(PlayerAbilities ability)
    {
        playerAbility1 = ability;
    }

    public void ChangeAbility2(PlayerAbilities ability)
    {
        playerAbility2 = ability;
    }

    public float ReturnResistanceAmount()
    {
        return playerResistanceAmount;
    }

    public DamageTypes ReturnPlayerResistanceType()
    {
        return playerResistance;
    }

    public void AddResistance(float resistanceAmount, DamageTypes resistanceType)
    {
        playerResistance = resistanceType;
        playerResistanceAmount = resistanceAmount;
    }

    public int GetLevel()
    {
        return playerLevel;
    }

    /// <summary>
    /// Call to add a level for the player when calculating in a loop xp amount to levels, add extra xp to parameter.
    /// </summary>
    public void AddLevel(float levelOverflow)
    {
        playerLevel += 1;
        levelProgress = levelOverflow;
    }

    public int GetBoostAmount()
    {
        return playerDraughtBoost;
    }

    public void AddLevelBoost(int boostAmountToAdd)
    {
        playerDraughtBoost += boostAmountToAdd;
    }
}

public enum DamageTypes
{
    Arcane,
    Draught,
    Leech,
    Fire,
    Blight,
    Stab
}

public enum PlayerAbilities
{
    Lightning,
    Draught_Ice,
    Leech_blast,
    Firebreath,
    Blight_blast,
    Silence,
    Bleed
}