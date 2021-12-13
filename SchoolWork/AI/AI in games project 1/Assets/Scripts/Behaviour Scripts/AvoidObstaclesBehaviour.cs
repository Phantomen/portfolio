using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/AvoidObstacle")]
public class AvoidObstaclesBehaviour : FlockBehaviour
{
    [Range(2, 10)]
    public int rayCount = 4;

    [Range(0, 10)]
    public float rayLength = 2f;

    [Range(1, 180)]
    public float rayViewRange = 20f;

    private List<bool> rayListHit = new List<bool>();
    private List<float> rayListLength = new List<float>();
    private List<Vector2> rayListDirection = new List<Vector2>();

    public LayerMask obstacleMask = 8;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> neighborContext, List<Transform> attackContext, List<Transform> chaseContext, Flock flock)
    {
        SetListLength();
        UpdateRays(agent);

        Vector2 forceSum = new Vector2();

        for(int i = 0; i < rayCount; i++)
        {
            if (rayListHit[i])
            {
                forceSum += GetInverseFraction(rayListLength[i], rayViewRange) * -rayListDirection[i];
            }
        }

        return forceSum;
    }

    private static float GetInverseFraction(float part, float whole)
    {
        return (whole - part) / whole;
    }

    private void UpdateRays(FlockAgent agent)
    {
        for(int i = 0; i < rayCount; i++)
        {
            float angle = -rayViewRange + (2 * rayViewRange * (i / (rayCount - 1)));
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            //Vector3 rayDirection = rotation * agent.transform.up;

            Ray2D detectionRay = new Ray2D
            {
                origin = agent.transform.position,
                direction = rotation * agent.transform.up
            };

            RaycastHit2D raycastHit2D = Physics2D.Raycast(detectionRay.origin, detectionRay.direction, rayLength, obstacleMask);

            if (raycastHit2D)
            {
                rayListHit[i] = true;
                rayListLength[i] = raycastHit2D.distance;
            }

            else
            {
                rayListHit[i] = false;
                rayListLength[i] = -1f;
            }
            rayListDirection[i] = detectionRay.direction;
        }
    }

    private void SetListLength()
    {
        if(rayListHit.Count != rayCount)
        {
            while(rayListHit.Count < rayCount) { rayListHit.Add(false); }

            while (rayListHit.Count > rayCount) { rayListHit.RemoveAt(rayListHit.Count - 1); }
        }

        if (rayListLength.Count != rayCount)
        {
            while (rayListLength.Count < rayCount) { rayListLength.Add(-1f); }

            while (rayListLength.Count > rayCount) { rayListLength.RemoveAt(rayListLength.Count - 1); }
        }

        if (rayListDirection.Count != rayCount)
        {
            while (rayListDirection.Count < rayCount) { rayListDirection.Add(Vector2.zero); }

            while (rayListDirection.Count > rayCount) { rayListDirection.RemoveAt(rayListDirection.Count - 1); }
        }
    }
}
