using System.Runtime.CompilerServices;
using acolurk_base.helpers;
using UnityEngine;

namespace acolurk_base.classes;

public class VelocityOptimization
{
    // fields
    private Vector3 target;
    private Vector3 start;
    private float tof;
    private Vector3 velocity;
    float errorThreshold = 0.01f;
    private Vector3 bestVelocity;
    float bestError = float.MaxValue;
    float baseRate = 0.5f;
    float minRate = 0.05f;
    float prevError = float.MaxValue;
    int stuckCounter = 0;
    int stepCounter = 0;

    // constructor
    public VelocityOptimization(Vector3 start, Vector3 target, float tof, float errorThreshold = 0.01f,
        float baseRate = 0.5f, float minRate = 0.05f)
    {
        this.start = start;
        this.target = target;
        this.tof = tof;
        this.errorThreshold = errorThreshold;
        this.baseRate = baseRate;
        this.minRate = minRate;
        this.GetInitialGuess();
    }

    // methods
    public void GetInitialGuess()
    {
        Vector3 displacement = (target - start);
        Vector3 gravityEffect = 0.5f * Physics.gravity * tof * tof;
        this.velocity = (displacement - gravityEffect) / tof;
        this.bestVelocity = velocity;
    }

    public bool Step()
    {
        Vector3 landingPos = launcherHelpers.SimulatePuckTrajectory(start,
            target.y,
            velocity);
        Vector3 errorVector = target - landingPos;
        float error = errorVector.magnitude;

        if (error < errorThreshold)
        {
            return true;
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

        if (stepCounter > 0 && stepCounter % 10 == 0 && error > errorThreshold * 5)
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

        stepCounter++;
        return false;
    }

    public void Reset(Vector3 start, Vector3 target, float tof, float errorThreshold = 0.01f, float baseRate = 0.5f,
        float minRate = 0.05f)
    {
        this.start = start;
        this.target = target;
        this.tof = tof;
        this.errorThreshold = errorThreshold;
        this.baseRate = baseRate;
        this.minRate = minRate;
        GetInitialGuess();
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        GetInitialGuess();
    }

    public void SetStart(Vector3 start)
    {
        this.start = start;
        GetInitialGuess();
    }

    public void SetTOF(float tof)
    {
        this.tof = tof;
        GetInitialGuess();
    }

    public Vector3 GetBestVelocity()
    {
        return bestVelocity;
    }
}