using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {
	public State currentState;
    public float gizmoSize = 0.25f;
    //public EnemyStats enemyStats;
    //public State remainState;

    public List<PhaseBulletSpawnPoints> phaseBulletSpawnList = new List<PhaseBulletSpawnPoints>();

    [HideInInspector] public List<GameObject> minionList = new List<GameObject>();
    [HideInInspector] public List<GameObject> itemList = new List<GameObject>();

	[HideInInspector] public GameObject playerGameObject;
	[HideInInspector] public float stateTimeElapsed;

    //Set in inspector the 
 //   public Transform bulletSpawn;

	//private float nextFire;

	// Use this for initialization
	void Awake ()
	{
        //If no spaww point has been set for the bullet
        //if (bulletSpawn == null)
        //{
        //    bulletSpawn = transform;
        //}

		playerGameObject = GameObject.FindGameObjectWithTag ("Player");

        currentState.ResetAction(this);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        stateTimeElapsed += Time.deltaTime;

        ClearMinionListOfNull();
        ClearItemListOfNull();

		currentState.UpdateState (this);
	}

    //Draws gizmo in inspector
	void OnDrawGizmos()
	{
		if (currentState != null)
		{
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawSphere (transform.position, gizmoSize);
		}
	}

    //Changes state
	public void TransitionToState(State nextState)
	{
        //if (nextState != remainState)
        if (nextState != null
            && nextState != currentState)
        {
			currentState = nextState;
			//remainState = nextState;
			OnExitState ();
		}
	}

    //Check if time has expired
    //Use timer script instead???
	public bool CheckIfCountDownElapsed(float duration)
	{
		//stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
	}

    //When the state changes
	private void OnExitState()
	{
		stateTimeElapsed = 0;

        currentState.ResetAction(this);
	}


    public void ClearMinionListOfNull()
    {
        for (int i = 0; i < minionList.Count;)
        {
            if (minionList[i] != null)
            {
                i++;
            }

            else
            {
                minionList.RemoveAt(i);
            }
        }
    }

    public void MinionListDestroyClear(float t)
    {
        for (int i = 0; i < minionList.Count; i++)
        {
            Destroy(minionList[i], t);
        }
        ClearMinionListOfNull();
    }



    public void ClearItemListOfNull()
    {
        for (int i = 0; i < itemList.Count;)
        {
            if (itemList[i] != null)
            {
                i++;
            }

            else
            {
                itemList.RemoveAt(i);
            }
        }
    }

    public void ItemListDestroyClear(float t)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i], t);
        }
        ClearItemListOfNull();
    }
}
