using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(Hittable))]
public class Player : MonoBehaviour
{
    private Shooter shooter;
    private Hittable hittable;
    private new Renderer renderer;
    private float leftBound;
    private float rightBound;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        hittable = GetComponent<Hittable>();
        renderer = GetComponent<Renderer>();
        var camera = Camera.main;
        leftBound = GameManager.Instance
            .Map(g => g.LeftBound)
            .GetOrElse(camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x);
        rightBound =GameManager.Instance
            .Map(g => g.RightBound)
            .GetOrElse(camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, 0)).x);
    }

    private void OnEnable() {
        hittable.OnDeathEvent.AddListener(OnDeath);
    }

    private void OnDisable() {
        hittable.OnDeathEvent.RemoveListener(OnDeath);
    }

    public void Move(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        var targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(value, 0, 0));
        var spriteWidth = renderer.bounds.size.x;
        var newX = Mathf.Clamp(
            targetPosition.x, 
            leftBound + spriteWidth / 2, 
            rightBound - spriteWidth / 2);
        var currentPosition = transform.position;
        transform.position = new Vector3(
            newX, 
            currentPosition.y, 
            currentPosition.z);
    }

    public void Shoot(InputAction.CallbackContext context) {
        if (context.started) {
            shooter.Shoot();
        }
    }

    public void Kill() {
        hittable.Kill();
    }

    private void OnDeath(Hittable _)
    {
        GameManager.Instance.IfPresent(g => {
            g.GameOver();
        });
    }
}
