using UnityEngine;

[CreateAssetMenu(fileName = "SceneReference" , menuName = "Create scene reference")]
public class SceneReference : ScriptableObject {

    [SerializeField]
    private string scenePath;
    public string ScenePath => scenePath;
}