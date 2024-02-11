using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(Hittable))]
public class EnemyShip : MonoBehaviour
{
    public float VerticalSpeed {get; set;}
    public float HorizontalSpeed {get; set;}

    [SerializeField]
    private Swarm swarm;

    private Shooter shooter;
    private Hittable hittable;
    private bool movingLeft;
    private bool siege = false;

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        hittable = GetComponent<Hittable>();
    }

    private void OnEnable()
    {
        swarm.Add(this);
        hittable.OnDeathEvent.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        swarm.Remove(this);
        hittable.OnDeathEvent.RemoveListener(OnDeath);
    }

    private void Update()
    {
        if (siege) {return;}
        var direction = Vector3.zero;
        direction += HorizontalSpeed * (movingLeft ? Vector3.left : Vector3.right);
        direction += VerticalSpeed * Vector3.down;
        transform.position += Time.deltaTime * direction;
    }

    private void OnDeath(Hittable sender)
    {
        CameraManager.Instance.IfPresent(c => c.ShakeCamera(5f, 0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.gameObject.TryGetComponent<Fragment>().IfPresent(fragment =>
        {
            fragment.Kill();
        });

        collision.collider.gameObject.TryGetComponent<Player>().IfPresent(fragment =>
        {
            fragment.Kill();
        });
    }

    internal void TurnLeft()
    {
        movingLeft = true;
    }

    internal void TurnRight()
    {
        movingLeft = false;
    }

    internal void Shoot()
    {
        shooter.Shoot();
    }

    internal void SetSiege()
    {
        siege = true;
    }
}
