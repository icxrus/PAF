using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ParticleObjectData : ScriptableObject
{
    public GameObject prefab;
    public AudioClip audioClip;
}
