using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public class HiveMind : MonoBehaviour
{
    [SerializeField]
    private Swarm swarm;

    [SerializeField]
    private AnimationCurve enemyShipHorizontalSpeed = new();

    [SerializeField]
    private AnimationCurve enemyShipVerticalSpeed = new();

    [SerializeField]
    private float enemyShipShootInterval = 0.75f;

    [SerializeField]
    private float respawnAfterSeconds = 3f;

    [SerializeField]
    private List<SpawnPoint> spawnPoints = new();

    [SerializeField, Space]
    private UnityEvent onAllEnemyShipsDeadEvent = new();
    public UnityEvent OnAllEnemyShipsDeadEvent => onAllEnemyShipsDeadEvent;

    private float leftBound;
    private float rightBound;
    private int maxEnemies;
    private bool siege;

    private void Awake()
    {
        leftBound = GameManager.Instance.Map(g => g.LeftBound).GetOrElse(-3);
        rightBound = GameManager.Instance.Map(g => g.RightBound).GetOrElse(3);
        RespawnAllEnemyShips();
    }

    private void OnEnable()
    {
        foreach (var instance in swarm.Instances)
        {
            OnAddEntityToSwarm(instance);
        }
        swarm.OnAddEntityEvent.AddListener(OnAddEntityToSwarm);
        swarm.OnRemoveEntityEvent.AddListener(OnRemoveEntityFromSwarm);
    }

    private void OnDisable()
    {
        foreach (var instance in swarm.Instances)
        {
            OnRemoveEntityFromSwarm(instance);
        }
        swarm.OnAddEntityEvent.RemoveListener(OnAddEntityToSwarm);
        swarm.OnRemoveEntityEvent.RemoveListener(OnRemoveEntityFromSwarm);
    }

    private void OnAddEntityToSwarm(EnemyShip enemyShip)
    {
        enemyShip.TryGetComponent<Hittable>()
            .IfPresent(hittable => hittable.OnDeathEvent.AddListener(OnDeath));
    }

    private void OnRemoveEntityFromSwarm(EnemyShip enemyShip)
    {
        enemyShip.TryGetComponent<Hittable>()
            .IfPresent(hittable => hittable.OnDeathEvent.RemoveListener(OnDeath));
    }

    private void Start()
    {
        StartCoroutine(UnityUtils.RepeatEvery(enemyShipShootInterval, RandomEnemyShoot, IsSieging));
    }

    private void Update()
    {
        var bounds = GetBounds();
        if (bounds.center.x + bounds.extents.x > rightBound)
        {
            swarm.SendSignalToSwarmTurnLeft();
        }

        if (bounds.center.x - bounds.extents.x < leftBound)
        {
            swarm.SendSignalToSwarmTurnRight();
        }
    }

    private void RespawnAllEnemyShips()
    {
        spawnPoints.ForEach(s => s.Spawn());
        maxEnemies = spawnPoints.Count;
        UpdateSpeed();
    }

    private void OnDeath(Hittable hittable)
    {
        // TODO: The instance that just died is not yet removed from the list,
        // thus subtracting one will lead to the actual number.
        var count = swarm.Instances.Count();
        if (count - 1 <= 0)
        {
            OnAllEnemyShipsDeadEvent.Invoke();
            StartCoroutine(UnityUtils.DoAfterSeconds(respawnAfterSeconds, RespawnAllEnemyShips));
        }
        UpdateSpeed();
    }

    private void RandomEnemyShoot()
    {
        var enemiesThatCanShoot = swarm.Instances.Where(enemy => CanShoot(enemy)).ToList();
        var count = enemiesThatCanShoot.Count();
        if (count > 0)
        {
            enemiesThatCanShoot[UnityEngine.Random.Range(0, count) % count].Shoot();
        }
    }

    private bool CanShoot(EnemyShip enemyShip)
    {
        var hits = Physics2D.RaycastAll(enemyShip.transform.position, Vector2.down)
            .Where(hit => hit.collider.gameObject != enemyShip.gameObject);

        if (hits.Count() == 0)
        {
            return true;
        }

        return hits.First().collider.TryGetComponent<EnemyShip>()
            .Map(enemy => !swarm.Instances.Contains(enemy))
            .GetOrElse(true);
    }

    private Bounds GetBounds()
    {
        var bounds = new Bounds();
        foreach (var instance in swarm.Instances)
        {
            bounds.Encapsulate(instance.transform.position);
        }
        return bounds;
    }

    public void SetSiege()
    {
        siege = true;
        swarm.SendSignalToSwarmSetSiege();
    }

    public bool IsSieging()
    {
        return siege;
    }

    private void UpdateSpeed() {
        UpdateHorizontalSpeed();
        UpdateVerticalSpeed();
    }

    private void UpdateHorizontalSpeed()
    {
        swarm.SendSignalToChangeHorizontalSpeed(DetermineCurrentHorizontalSpeed());
    }

    private float DetermineCurrentHorizontalSpeed()
    {
        return enemyShipHorizontalSpeed.Evaluate(1.0f - ((float)swarm.Instances.Count() / maxEnemies));
    }

    private void UpdateVerticalSpeed()
    {
        swarm.SendSignalToChangeVerticalSpeed(DetermineCurrentVerticalSpeed());
    }

    private float DetermineCurrentVerticalSpeed()
    {
        return enemyShipVerticalSpeed.Evaluate(1.0f - ((float)swarm.Instances.Count() / maxEnemies));
    }
}