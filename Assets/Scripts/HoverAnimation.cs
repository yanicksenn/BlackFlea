using static UnityEngine.Random;
using UnityEngine;

public class HoverAnimation : MonoBehaviour
{
    [SerializeField]
    private float distanceMultiplier = 1;

    [SerializeField]
    private float speedMultiplier = 1;

    private Vector3 targetPosition;

    private void Start()
    {
        FindRandomTargetLocation();
    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speedMultiplier);
        if ((targetPosition - transform.localPosition).sqrMagnitude < 0.005)
        {
            FindRandomTargetLocation();
        }
    }

    private void FindRandomTargetLocation()
    {
        targetPosition = new Vector3(
            /* x= */ Range(-1f, 1f),
            /* y= */ Range(-1f, 1f)
        ).normalized * distanceMultiplier;
    }
}
