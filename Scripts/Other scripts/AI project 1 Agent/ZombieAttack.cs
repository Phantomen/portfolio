using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlockAgent))]
[RequireComponent(typeof(Collider2D))]
public class ZombieAttack : MonoBehaviour {

    FlockAgent agent;

	// Use this for initialization
	void Awake ()
    {
        agent = GetComponent<FlockAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        FlockAgent colAgent = col.transform.gameObject.GetComponent<FlockAgent>();
        if (colAgent != null && agent.AgentFlock != colAgent.AgentFlock && colAgent.dying == false)
        {
            agent.AgentFlock.AddAgent(colAgent.transform.position, colAgent.transform.rotation);
            colAgent.KillAgent();
        }
    }
}
