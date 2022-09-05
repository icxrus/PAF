using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyBaseData enemyData;
    private PlayerStatData _playerData;
    private float _Health;
    private float _Damage;
    // Start is called before the first frame update
    void Start()
    {
        _Health = enemyData.ReturnHealth();
        _Damage = enemyData.ReturnBaseDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            var playerdata = GameObject.FindWithTag("Player");
            var data = playerdata.GetComponent<PlayerStatManager>();
            float incomingDMG = data.DamageTest();
            _Health -= incomingDMG;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Health < 1)
        {
            //Die
            //Destroy(this);
            Debug.Log("death");
        }
    }
}
