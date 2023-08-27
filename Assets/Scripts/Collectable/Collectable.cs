using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] protected int moneyAmount = 1;

    protected bool isCollected = false;

    public bool CollectedStatus
    {
        get => isCollected;
        set => isCollected = value;
    }

    private void OnEnable()
    {
        isCollected = false;
    }

    public virtual int Collect()
    {
        if (isCollected)
        {
            return 0;
        }
        isCollected = true;
        this.gameObject.SetActive(false);
        return moneyAmount;
    }
}
