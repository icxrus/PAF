using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTravel : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float destructionTimer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destructionTimer);
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
