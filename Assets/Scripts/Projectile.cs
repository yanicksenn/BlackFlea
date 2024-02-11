using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed => movementSpeed;

    [SerializeField]
    private float spreadRadius = 0.1f;
    public float SpreadRadius => spreadRadius;

    [SerializeField]
    private float ttl = 1;
    public float Ttl => ttl;

    [SerializeField, Space]
    private HitEvent onHitEvent = new();
    public HitEvent OnHitEvent => onHitEvent;

    public GameObject Sender { get; set; }
    public bool IsConsumed { get; private set; }

    private readonly RaycastHit2D[] collisionCache = new RaycastHit2D[1];
    private new Rigidbody2D rigidbody;
    private Vector3 previousPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(UnityUtils.DoAfterSeconds(ttl, () => {
            Destroy(gameObject);
        }));
    }

    private void Start()
    {
        rigidbody.AddForce(rigidbody.mass * movementSpeed * transform.up / Time.fixedDeltaTime);
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var currentPosition = transform.position;
        var amountOfCollisions = Physics2D.LinecastNonAlloc(previousPosition, currentPosition, collisionCache);
        previousPosition = currentPosition;
        if (amountOfCollisions == 0)
        {
            return;
        }

        var impactCollision = collisionCache[0];
        var impactGameObject = impactCollision.collider.gameObject;
        if (impactGameObject == Sender)
        {
            return;
        }

        if (IsConsumed)
        {
            return;
        }

        IsConsumed = true;
        OnHit(impactGameObject);
        if (spreadRadius > 0)
        {
            var spreadCollisions = Physics2D.CircleCastAll(impactCollision.point, spreadRadius, Vector2.zero);
            foreach (var spreadCollision in spreadCollisions)
            {
                var spreadGameObject = spreadCollision.collider.gameObject;
                if (spreadGameObject != impactGameObject)
                {
                    OnHit(spreadCollision.collider.gameObject);
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnHit(GameObject gameObject)
    {
        gameObject.TryGetComponent<Hittable>(hittable =>
        {
            hittable.Hit();
        });
        OnHitEvent.Invoke(gameObject);
    }

    [Serializable]
    public class HitEvent : UnityEvent<GameObject> { }
}