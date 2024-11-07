using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent<HitCollider, HurtCollider> onHitReceived;

    public void NotifyHit(HitCollider hitCollider)
    {
        onHitReceived?.Invoke(hitCollider, this);
    }
}

