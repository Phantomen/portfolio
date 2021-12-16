using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float health = 5;
    private float currentHealth;

    public bool isAlive = true;

    public EnemySpawner enemySpawner;
    //public bool IsAlive { get { return IsAlive; } }

	// Use this for initialization
	void Start () {
        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (gameObject != null)
        {
            isAlive = false;
            //enemySpawner.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
