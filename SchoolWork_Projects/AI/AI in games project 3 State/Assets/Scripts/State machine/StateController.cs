using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class StateController : MonoBehaviour {
	public State currentState, remainState;
    public float gizmoSize = 0.25f;

    public float viewDistance = 20f;
    public float fieldOfView = 90f;

    public float attackRange = 2f;

    public float wanderDistance = 5f;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public GameObject playerGameObject;
	[HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public Vector3 investigateLocation;   //aka last known location
    [HideInInspector] public Vector3 guardTransform;

    public float investigationDistance = 0.6f;

	// Use this for initialization
	void Awake ()
	{
		playerGameObject = GameObject.FindGameObjectWithTag ("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        investigateLocation = transform.position;
        guardTransform = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        stateTimeElapsed += Time.deltaTime;

		currentState.UpdateState (this);
	}

    //Draws gizmo in inspector
	void OnDrawGizmos()
	{
		if (currentState != null)
		{
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere (transform.position, gizmoSize);
		}
	}

    //Changes state
	public void TransitionToState(State nextState)
	{
        //If the next state exists and the next state isn't the current state
        if (nextState != null
            && nextState != currentState
            && nextState != remainState)
        {
			currentState = nextState;
			OnExitState ();
		}
	}

    //Check if time has expired
	public bool CheckIfCountDownElapsed(float duration)
	{
		return (stateTimeElapsed >= duration);
	}

    //When the state changes
	private void OnExitState()
	{
		stateTimeElapsed = 0;
	}

    public void Attack()
    {
        playerGameObject.SetActive(false);
    }
}
