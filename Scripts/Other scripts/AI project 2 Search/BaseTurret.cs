using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : MonoBehaviour
{

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private GameObject target;

    public float targetingRadius = 4f;

    public LayerMask enemyMask;

    public int damage = 1;

    public float fireRate = 3;
    public float lineDisplayTime = 0.2f;

    private float currentFireDelay = 0;

    LineRenderer bulletLine;

    // Use this for initialization
    void Start()
    {
        bulletLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();

        if(currentFireDelay > 0)
        {
            currentFireDelay -= Time.deltaTime;
        }

        if (target != null)
        {
            Vector3 dir = new Vector3(target.transform.position.x, gameObject.transform.position.y, target.transform.position.z) - gameObject.transform.position;

            gameObject.transform.right = -dir;

            Shoot();
        }

        if(1 / fireRate - lineDisplayTime > currentFireDelay)
        {
            bulletLine.enabled = false;
        }
    }

    void UpdateTarget()
    {
        List<PathFollow> enemies = new List<PathFollow>();
        Collider[] hits = Physics.OverlapSphere(transform.position, targetingRadius, enemyMask);

        foreach (Collider col in hits)
        {
            enemies.Add(col.GetComponent<PathFollow>());
        }

        if (enemies.Count > 0)
        {
            PathFollow pathTarget;

            if (target && enemies.Contains(target.GetComponent<PathFollow>())) { pathTarget = target.GetComponent<PathFollow>(); }
            else { pathTarget = enemies[0].GetComponent<PathFollow>(); }

            float targetDistToGoal = pathTarget.GetDistanceLeftToGoal();

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetDistanceLeftToGoal() < targetDistToGoal)
                {
                    pathTarget = enemies[i];
                    targetDistToGoal = pathTarget.GetDistanceLeftToGoal();
                }
            }

            target = pathTarget.gameObject;
            return;
        }

        target = null;
    }


    private void Shoot()
    {
        while(currentFireDelay <= 0)
        {
            currentFireDelay += 1f / fireRate;

            if (bulletLine)
            {
                bulletLine.enabled = true;
                bulletLine.SetPosition(0, transform.position);

                bulletLine.SetPosition(1, target.transform.position);
            }

            EnemyHealth eHealth = target.GetComponent<EnemyHealth>();
            eHealth.TakeDamage(damage);
            //if (!eHealth.IsAlive)
            //{
            //    target = null;
            //}
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, targetingRadius);
    }
}
