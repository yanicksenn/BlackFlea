using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitPointIndicator : MonoBehaviour
{
    [SerializeField]
    private Hittable hittable;

    [SerializeField]
    private GameObject indicatorPrefab;

    [SerializeField]
    private OneOffEffect indicatorRemovalEffect;

    private readonly List<GameObject> indicators = new();

    private void OnEnable()
    {
        hittable.OnHitEvent.AddListener(RemoveIndicator);
        hittable.OnDeathEvent.AddListener(RemoveAllIndicators);
    }

    private void OnDisable()
    {
        hittable.OnHitEvent.RemoveListener(RemoveIndicator);
        hittable.OnDeathEvent.RemoveListener(RemoveAllIndicators);
    }

    private void Start()
    {
        for (int i = 0; i < hittable.HitPoints; i++)
        {
            var indicator = Instantiate(indicatorPrefab, transform);
            indicator.transform.SetAsLastSibling();
            indicators.Add(indicator);
        }
    }

    private void RemoveAllIndicators(Hittable _)
    {
        indicators.ToList().ForEach(RemoveIndicator);
    }

    private void RemoveIndicator(Hittable _)
    {
        RemoveIndicator(indicators.Last());
    }

    private void RemoveIndicator(GameObject indicator)
    {
        if (indicatorRemovalEffect != null) {
            Instantiate(indicatorRemovalEffect, indicator.transform.position, Quaternion.identity);
        }

        indicators.Remove(indicator);
        Destroy(indicator);
    }
}