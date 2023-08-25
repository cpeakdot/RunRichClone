using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int moneyAmount = 1;

    private bool isCollected = false;

    private void OnEnable()
    {
        isCollected = false;
    }

    public int Collect()
    {
        if (isCollected)
        {
            return 0;
        }
        else
        {
            isCollected = true;
            return moneyAmount;
        }
    }
}
