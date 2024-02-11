using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Swarm", menuName = "Create swarm")]
public class Swarm : AbstractGroup<EnemyShip, EnemyShipEvent>
{ 
    [SerializeField, Space]
    private EnemyShipEvent onAddEntityEvent = new();
    public override EnemyShipEvent OnAddEntityEvent => onAddEntityEvent;

    [SerializeField, Space]
    private EnemyShipEvent onRemoveEntityEvent = new();
    public override EnemyShipEvent OnRemoveEntityEvent => onRemoveEntityEvent;

    [SerializeField]
    private UnityEvent onSwarmTurnLeftEvent = new();
    public UnityEvent OnSwarmTurnLeftEvent => onSwarmTurnLeftEvent;

    [SerializeField]
    private UnityEvent onSwarmTurnRightEvent = new();
    public UnityEvent OnSwarmTurnRightEvent => onSwarmTurnRightEvent;

    internal void SendSignalToSwarmTurnLeft()
    {
        SendSignalToSwarm(ship => ship.TurnLeft());
        onSwarmTurnLeftEvent.Invoke();
    }

    internal void SendSignalToSwarmTurnRight()
    {
        SendSignalToSwarm(ship => ship.TurnRight());
        onSwarmTurnRightEvent.Invoke();
    }

    internal void SendSignalToChangeVerticalSpeed(float newScore)
    {
        SendSignalToSwarm(ship => ship.VerticalSpeed = newScore);
    }

    internal void SendSignalToChangeHorizontalSpeed(float newScore)
    {
        SendSignalToSwarm(ship => ship.HorizontalSpeed = newScore);
    }

    internal void SendSignalToSwarmSetSiege()
    {
        SendSignalToSwarm(ship => ship.SetSiege());
    }

    private void SendSignalToSwarm(Action<EnemyShip> action)
    {
        foreach (var instance in Instances)
        {
            action.Invoke(instance);
        }
    }
}