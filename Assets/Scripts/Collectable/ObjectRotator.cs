using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private Transform objTransform;
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        objTransform.Rotate(rotationVector * (rotationSpeed * Time.deltaTime));
    }
}
