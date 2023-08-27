using UnityEngine;

public class CollectableGate : Collectable
{
    [SerializeField] private CollectableGate neighbourGate;
    public override int Collect()
    {
        neighbourGate.CollectedStatus = true;
        
        if (isCollected) return 0;
        isCollected = true;
        return moneyAmount;
    }
}
