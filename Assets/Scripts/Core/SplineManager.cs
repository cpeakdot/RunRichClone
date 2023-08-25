using Dreamteck.Splines;
using UnityEngine;

[RequireComponent(typeof(SplineComputer))]
public class SplineManager : MonoBehaviour
{
    public static SplineManager Instance { get; private set; }

    public SplineComputer splineComputer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        splineComputer = GetComponent<SplineComputer>();
    }
}
