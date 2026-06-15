using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChildEvent))]
public class ChildEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChildEvent childEvent = (ChildEvent)target;
        
        DrawDefaultInspector();

        GUILayout.BeginVertical();

            GUILayout.Space(10f);
            
            GUILayout.Label("Apply changes to all Child Events", EditorStyles.boldLabel);

            if (GUILayout.Button("Override All"))
            {
                ChildEvent[] childEvents = FindObjectsByType<ChildEvent>(FindObjectsSortMode.None);

                foreach (ChildEvent child in childEvents)
                {
                    child.timeBeforeReactivation = childEvent.timeBeforeReactivation;
                    child.solSatisfactionChange = childEvent.solSatisfactionChange;
                    child.solSanityChange = childEvent.solSanityChange;
                    child.koSatisfactionChange = childEvent.koSatisfactionChange;
                    child.koSanityChange = childEvent.koSanityChange;
                }
            }
            
        GUILayout.EndVertical();
    }
}
