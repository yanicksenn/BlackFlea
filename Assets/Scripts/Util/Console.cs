using UnityEngine;

[CreateAssetMenu(fileName = "Console", menuName = "Create console")]
public class Console : ScriptableObject
{
    [SerializeField]
    private bool enabled;

    public void Log(string message)
    {
        if (enabled)
        {
            Debug.Log(message);
        }
    }
}