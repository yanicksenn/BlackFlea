using System.Collections;
using UnityEngine;

public class OneOffEffect : MonoBehaviour {

    [SerializeField]
    private float ttl = 1;
    public float Ttl => ttl;

    private void Awake()
    {
        StartCoroutine(UnityUtils.DoAfterSeconds(ttl, () => {
            Destroy(gameObject);
        }));
    }
}