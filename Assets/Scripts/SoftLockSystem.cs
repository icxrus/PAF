using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoftLockSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameObject targetObject;
    [SerializeField] private List<GameObject> targetOptions;
    [SerializeField] private List<GameObject> sortedTargetOptions;

    private PlayerInput input;
    private InputAction key_Targeting;

    private int targetCount;

    public GameObject prefabForRing;

    // Start is called before the first frame update
    void Start()
    {
        input = player.GetComponent<PlayerInput>();
        key_Targeting = input.actions["TargetingSwap"];

        targetOptions = new();
        sortedTargetOptions = new();
    }

    private void Update()
    {
        if (key_Targeting.triggered)
        {
            //swap target
        }
    }

    #region Triggers
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!targetOptions.Contains(other.gameObject))
            {
                targetOptions.Add(other.gameObject);
                targetCount++;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (targetOptions.Contains(other.gameObject))
            {
                targetOptions.Remove(other.gameObject);
                sortedTargetOptions.Remove(other.gameObject);
                targetCount--;
                CalculateShortestDistanceInTargetOptions(); //Calculate again to make sure target didn't leave area
            }
        }
    }
    #endregion

    #region Math to the max
    private void CalculateShortestDistanceInTargetOptions()
    {
        GameObject shortestDistEnemy = null;
        float previousDist = 0;
        float currentDist = 0;

        if (targetOptions.Count != 0)
        {
            foreach (var go in targetOptions) //Go through objects to determine closest to player
            {
                if (targetOptions.Count == 1) //If we only have one object, we can assign that object as the closest one.
                {
                    shortestDistEnemy = targetOptions[0];
                    return;
                }

                if (shortestDistEnemy == null) //If we haven't calculated anything yet, we can assign that as the first value.
                {
                    shortestDistEnemy = go;
                    
                    
                }
                else
                {
                    currentDist = CalculateDistanceToPlayer(go.transform);
                    if (currentDist < previousDist) //If new distance is shorter than previous distance, we should record it.
                    {
                        shortestDistEnemy = go;
                        
                    }
                }

                previousDist = currentDist;
            }

            targetObject = shortestDistEnemy;
        }
    }

    private float CalculateDistanceToPlayer(Transform currentEnemyDist)
    {
        var dist = Vector3.Distance(player.transform.position, currentEnemyDist.position);
        return dist;
    }

    public GameObject ReturnCurrentTarget()
    {
        CalculateShortestDistanceInTargetOptions(); //Make sure the target is still available!
        return targetObject;
    }
    
    private void SortListByDistance()
    {
        sortedTargetOptions = targetOptions;
        sortedTargetOptions = sortedTargetOptions.OrderBy(x => (transform.position - x.transform.position).sqrMagnitude).ToList();
    }

    public List<GameObject> ReturnSortedListOfTargets()
    {
        SortListByDistance();
        return sortedTargetOptions;
    }
    #endregion

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
