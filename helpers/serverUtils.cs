using UnityEngine;

namespace acolurk_base.helpers;

public static class serverUtils
{
    public static Vector3 SimulatePuckTrajectory(Vector3 start,
        float finalHeight,
        Vector3 velocity)
    {
        Vector3 pos = start;
        Vector3 vel = velocity;
        Vector3 nextpos;
        Vector3 nextvel;
        float drag = 0.3f;
        float step = Time.deltaTime;
        Vector3 gravity = Physics.gravity;
        float dragFactor = Mathf.Exp(-drag * step);

        while (pos.y > finalHeight || vel.y > 0)
        {
            nextpos = pos + vel * step + 0.5f * gravity * step * step;
            nextvel = (vel + gravity * step) * dragFactor;
            pos = nextpos;
            vel = nextvel;
        }

        return pos;
    }

    public static Vector3 OptimizeLaunchVelocity(Vector3 start,
        Vector3 end,
        float tof,
        int maxIterations)
    {
        Vector3 displacement = (end - start);
        Vector3 gravityEffect = 0.5f * Physics.gravity * tof * tof;
        Vector3 velocity = (displacement - gravityEffect) / tof;
        Vector3 target = end;
        float errorThreshold = 0.01f;
        Vector3 bestVelocity = velocity;
        float bestError = float.MaxValue;
        float baseRate = 0.2f;
        float minRate = 0.01f;
        float prevError = float.MaxValue;
        int stuckCounter = 0;

        for (int i = 0;
             i < maxIterations;
             i++)
        {
            Vector3 landingPos = SimulatePuckTrajectory(start,
                end.y,
                velocity);
            Vector3 errorVector = target - landingPos;
            float error = errorVector.magnitude;

            if (error < errorThreshold)
            {
                break;
            }

            if (error < bestError)
            {
                bestError = error;
                bestVelocity = velocity;
            }

            float adaptiveRate = Mathf.Max(minRate,
                baseRate *
                Mathf.Min(1.0f,
                    error));

            if (Mathf.Abs(error - prevError) < errorThreshold * 0.1f)
            {
                stuckCounter++;
                if (stuckCounter >= 3)
                {
                    adaptiveRate *= 0.5f;
                    stuckCounter = 0;
                }
            }
            else
                stuckCounter = 0;

            prevError = error;

            Vector3 correction = new Vector3(
                errorVector.x * adaptiveRate,
                errorVector.y * adaptiveRate * 0.75f,
                errorVector.z * adaptiveRate
            );

            Vector3 horizontalError = new Vector3(errorVector.x,
                0,
                errorVector.z);
            if (horizontalError.magnitude > errorThreshold &&
                !Mathf.Approximately(Mathf.Sign(errorVector.y),
                    Mathf.Sign(horizontalError.magnitude)))
            {
                correction.y = errorVector.y * adaptiveRate * 1.5f;
            }

            velocity += correction;

            if (i > 0 && i % 10 == 0 && error > errorThreshold * 5)
            {
                velocity += new Vector3(
                                UnityEngine.Random.Range(-0.1f,
                                    0.1f),
                                UnityEngine.Random.Range(-0.1f,
                                    0.1f),
                                UnityEngine.Random.Range(-0.1f,
                                    0.1f)
                            ) *
                            adaptiveRate;
            }

        }

        return bestVelocity;
    }

    public static void CheckPuckAmount(int limit)
    {
        PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
        if (pm.pucks.Count > limit) pm.Server_DespawnPuck(pm.pucks._items[0]);
    }
}