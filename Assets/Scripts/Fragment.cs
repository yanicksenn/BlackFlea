using UnityEngine;

[RequireComponent(typeof(Hittable))]
public class Fragment : MonoBehaviour {
    private Hittable hittable;

    private void Awake() {
        hittable = GetComponent<Hittable>();
    }

    public void Kill() {
        hittable.Kill();
    }
}