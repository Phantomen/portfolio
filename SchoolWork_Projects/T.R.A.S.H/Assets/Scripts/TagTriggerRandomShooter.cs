using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagTriggerRandomShooter : MonoBehaviour {

    [Tooltip("The objects to spawn")]
    [SerializeField]
    private List<GameObject> spawnObjectPrefab = new List<GameObject>();

    [Tooltip("The objects to spawn")]
    [SerializeField]
    private List<Transform> spawnPositionsTransforms = new List<Transform>();


    [Tooltip("The delay between each spawn")]
    [SerializeField]
    private float spawnDelay = 0.5f;


    [Tooltip("")]
    [SerializeField]
    private float startSpawnDelay = 0f;

    private Timer currentDelay;


    [Tooltip("The tags that triggers the shooter")]
    [SerializeField]
    private string[] triggerTags;


    [Tooltip("The random X-position from spawnObject")]
    [SerializeField]
    private Vector2 xLocalPositionLimitOffset;

    [Tooltip("The random Y-position from spawnObject")]
    [SerializeField]
    private Vector2 yLocalPositionLimitOffset;


    [Tooltip("Max degrees")]
    [SerializeField]
    private float maxDegreesFromCenter = 45;

    [Tooltip("Min degrees at end point")]
    [SerializeField]
    private float minDegreesFromCenter = 0;

    [Tooltip("The random degree gets lower at the closer to the end points in the ")]
    [SerializeField]
    private bool degreelowerAtEndPoints = true;


    private bool triggered = false;

    private bool currentlySpawning;



    // Use this for initialization
    void Start ()
    {
        currentDelay = new Timer(startSpawnDelay, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (triggered == true)
        {
            currentDelay.Time += Time.deltaTime;

            if (currentDelay.Expired == true)
            {
                currentDelay.Time -= currentDelay.Duration;

                if (currentDelay.Duration != spawnDelay)
                {
                    currentDelay.Duration = spawnDelay;
                }

                Spawn();
            }
        }
	}

    private void Spawn()
    {
        for (int i = 0; i < spawnObjectPrefab.Count; i++)
        {
            if (spawnObjectPrefab[i] != null)
            {
                float xPositionOffset = Random.Range(xLocalPositionLimitOffset.x, xLocalPositionLimitOffset.y);
                float yPositionOffset = Random.Range(yLocalPositionLimitOffset.x, yLocalPositionLimitOffset.y);

                float newRot;

                if (degreelowerAtEndPoints == true)
                {
                    float mult1, mult2 = 1;

                    if (xPositionOffset > 0)
                    {
                        //mult2 =
                    }

                    else if (xPositionOffset < 0)
                    {
                        //mult1 = 
                    }

                    else
                    {

                    }
                }

                else
                {
                    newRot = Random.Range(-maxDegreesFromCenter, maxDegreesFromCenter);
                }
            }
        }
    }
}
