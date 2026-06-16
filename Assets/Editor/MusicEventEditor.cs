using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicEvent))]
public class MusicEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MusicEvent musicEvent = (MusicEvent)target;
        
        DrawDefaultInspector();

        GUILayout.BeginVertical();

        GUILayout.Space(10f);
            
        GUILayout.Label("Apply changes to all Music Events", EditorStyles.boldLabel);

        if (GUILayout.Button("Override All"))
        {
            MusicEvent[] musicEvents = FindObjectsByType<MusicEvent>(FindObjectsSortMode.None);

            foreach (MusicEvent music in musicEvents)
            {
                music.phonePrefab = musicEvent.phonePrefab;
                music.timeBeforeReactivation = musicEvent.timeBeforeReactivation;
                music.solSatisfactionChange = musicEvent.solSatisfactionChange;
                music.solSanityChange = musicEvent.solSanityChange;
                music.koSatisfactionChange = musicEvent.koSatisfactionChange;
                music.koSanityChange = musicEvent.koSanityChange;
                music.overlapArea = musicEvent.overlapArea;
            }
        }
            
        GUILayout.EndVertical();
    }
}
