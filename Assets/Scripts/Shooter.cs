using System;
using UnityEngine;
using UnityEngine.Events;

public class Shooter : MonoBehaviour {

    [SerializeField]
    private Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    [SerializeField]
    private float shootCooldown;

    [SerializeField, Space]
    private ShootEvent onShootEvent = new();
    public ShootEvent OnShootEvent => onShootEvent;

    private float currentShootCooldown;

    public void Shoot() {
        if (currentShootCooldown > 0) {
            return;
        }

        if (projectilePrefab != null) {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.Sender = gameObject;
            OnShootEvent.Invoke(this);
            currentShootCooldown = shootCooldown;
        }
    }

    private void Update() {
        currentShootCooldown -= Time.deltaTime;
    }

    [Serializable]
    public class ShootEvent : UnityEvent<Shooter> {

    }
}