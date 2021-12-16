using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Wanderer")]
public class WandererBehaviour : FlockBehaviour
{

    public float wanderDistance = 3f;
    public float wanderRadius = 0.5f;
    public float wanderDisplacement = 0.2f;

    private Vector2 wanderTarget = new Vector2(0, 0);

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {
        wanderTarget += new Vector2(Random.Range(-1f, 1f) * wanderDisplacement, Random.Range(-1f, 1f) * wanderDisplacement);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector2 targetLocal = wanderTarget + new Vector2(0, wanderDistance);
        //(Vector2)agent.transform.RotateAroundLocal(new Vector3(0, 0, 1), agent.transform.rotation.z);
        Vector2 targetWorld = agent.transform.TransformPoint(targetLocal);

        return targetWorld - (Vector2)agent.transform.position;
    }
}
