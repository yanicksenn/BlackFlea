using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Hittable : MonoBehaviour
{
    [SerializeField]
    private int hitPoints = 1;
    public int HitPoints => hitPoints;

    [SerializeField]
    private bool doNotDestroy;
    public bool DoNotDestroy => doNotDestroy;

    [SerializeField]
    private OneOffEffect hitEffectPrefab;
    public OneOffEffect HitEffectPrefab => hitEffectPrefab;

    [SerializeField]
    private OneOffEffect deathEffectPrefab;
    public OneOffEffect DeathEffectPrefab => deathEffectPrefab;

    [SerializeField, Space]
    private HitEvent onHitEvent = new();
    public HitEvent OnHitEvent => onHitEvent;

    [SerializeField]
    private DeathEvent onDeathEvent = new();
    public DeathEvent OnDeathEvent => onDeathEvent;

    public bool IsDead => hitPoints <= 0;

    public void Kill()
    {
        hitPoints = 1;
        Hit();
    }

    public void Hit()
    {
        if (IsDead)
        {
            return;
        }

        hitPoints--;
        if (hitPoints > 0)
        {
            OnHitEvent.Invoke(this);
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            }
        }
        else
        {
            OnDeathEvent.Invoke(this);
            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, transform.rotation);
            }

            if (DoNotDestroy)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [Serializable]
    public class HitEvent : UnityEvent<Hittable> { }

    [Serializable]
    public class DeathEvent : UnityEvent<Hittable> { }
}