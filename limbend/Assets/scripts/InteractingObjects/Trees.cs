using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour, IResourses
{
    private int HealthTree;
    void Start()
    {
        HealthTree = 100;
    }
    public void TakeDamage(int damage)
    {
        //if (a insteadOf axe)
        HealthTree -= damage;
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + Random.Range(0.01f, 0.1f), gameObject.transform.position.y + Random.Range(0.01f, 0.1f));
        if (HealthTree <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {

    }
}