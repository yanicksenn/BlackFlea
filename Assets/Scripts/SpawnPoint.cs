using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    
    [SerializeField]
    private GameObject prefab;

    public void Spawn() {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}