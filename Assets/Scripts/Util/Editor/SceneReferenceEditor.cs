using UnityEditor;

[CustomEditor(typeof(SceneReference), true)]
public class SceneReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var reference = target as SceneReference;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(reference.ScenePath);

        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("scenePath");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}