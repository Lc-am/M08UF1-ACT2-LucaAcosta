using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    public UnityEvent<HitCollider, HurtCollider> onHitDelivered;
    [SerializeField] List<string> hittableTags;

    private void OnTriggerEnter(Collider other)
    {
        if (hittableTags.Contains(other.gameObject.tag))
        {
            HurtCollider hurtCollider = other.GetComponent<HurtCollider>();

            if (hurtCollider)
            {
                hurtCollider.NotifyHit(this);
                onHitDelivered.Invoke(this, hurtCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hittableTags.Contains(collision.gameObject.tag))
        {
            HurtCollider hurtCollider = collision.gameObject.GetComponent<HurtCollider>();

            if (hurtCollider)
            {
                hurtCollider.NotifyHit(this);
                onHitDelivered.Invoke(this, hurtCollider);
            }
        }
    }
}
