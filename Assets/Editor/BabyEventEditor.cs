using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BabyEvent))]
public class BabyEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BabyEvent babyEvent = (BabyEvent)target;
        
        DrawDefaultInspector();

        GUILayout.BeginVertical();

            GUILayout.Space(10f);
        
            GUILayout.Label("Apply changes to all Baby Events", EditorStyles.boldLabel);

            if (GUILayout.Button("Override All"))
            {
                BabyEvent[] babyEvents = FindObjectsByType<BabyEvent>(FindObjectsSortMode.None);

                foreach (BabyEvent baby in babyEvents)
                {
                    baby.timeBeforeReactivation = babyEvent.timeBeforeReactivation;
                    baby.solSatisfactionChange = babyEvent.solSatisfactionChange;
                    baby.solSanityChange = babyEvent.solSanityChange;
                    baby.koSatisfactionChange = babyEvent.koSatisfactionChange;
                    baby.koSanityChange = babyEvent.koSanityChange;
                    baby.overlapArea = babyEvent.overlapArea;
                }
            }
            
        GUILayout.EndVertical();
    }
}
