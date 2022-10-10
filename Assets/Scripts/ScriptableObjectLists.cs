using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectLists : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> ConsumableList = new();
    [SerializeField] private List<ScriptableObject> WeaponList = new();
    [SerializeField] private List<ScriptableObject> MiscList = new();
    [SerializeField] private List<ScriptableObject> ArmorList = new();
    private void Start()
    {
        ScriptableObject[] tmp = Resources.LoadAll<ConsumClass>("ScriptableObjects/");
        ConsumableList = new List<ScriptableObject>(tmp);

        ScriptableObject[] tmp2 = Resources.LoadAll<WeaponClass>("ScriptableObjects/");
        WeaponList = new List<ScriptableObject>(tmp2);

        ScriptableObject[] tmp3 = Resources.LoadAll<MiscClass>("ScriptableObjects/");
        MiscList = new List<ScriptableObject>(tmp3);

        ScriptableObject[] tmp4 = Resources.LoadAll<ArmorClass>("ScriptableObjects/");
        ArmorList = new List<ScriptableObject>(tmp4);
    }
    
    
}
