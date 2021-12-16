using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {
	public State currentState;
    public float gizmoSize = 0.25f;

    [HideInInspector] public List<GameObject> minionList = new List<GameObject>();
    [HideInInspector] public List<GameObject> itemList = new List<GameObject>();

	[HideInInspector] public GameObject playerGameObject;
	[HideInInspector] public float stateTimeElapsed;

	// Use this for initialization
	void Awake ()
	{
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
        //If the next state exists and the next state isn't the current state
        if (nextState != null
            && nextState != currentState)
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

        currentState.ResetAction(this);
	}


    //Clears the list of gameobjects that doesn't exist
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

    //Destroys every object in list and then clears the list
    public void MinionListDestroyClear(float t)
    {
        for (int i = 0; i < minionList.Count; i++)
        {
            Destroy(minionList[i], t);
        }

        minionList.Clear();
    }


    //Clears the list of gameobjects that doesn't exist
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

    //Destroys every object in list and then clears the list
    public void ItemListDestroyClear(float t)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i], t);
        }
        itemList.Clear();
    }
}
