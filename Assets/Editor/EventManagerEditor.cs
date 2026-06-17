using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventManager eventManager = (EventManager)target;
        
        DrawDefaultInspector();

        GUILayout.BeginVertical();

        GUILayout.Space(10f);
        
        GUILayout.Label("Apply changes to all Events", EditorStyles.boldLabel);

        if (GUILayout.Button("Override All"))
        {
            BabyEvent[] babyEvents = FindObjectsByType<BabyEvent>(FindObjectsSortMode.None);

            foreach (BabyEvent baby in babyEvents)
            {
                baby.timeBeforeReactivation = eventManager.babyTimeBeforeReactivation;
                baby.solSatisfactionChange = eventManager.babySolSatisfactionChange;
                baby.solSanityChange = eventManager.babySolSanityChange;
                baby.koSatisfactionChange = eventManager.babyKoSatisfactionChange;
                baby.koSanityChange = eventManager.babyKoSanityChange;
                baby.overlapArea = eventManager.babyOverlapArea;
            }
            
            ChildEvent[] childEvents = FindObjectsByType<ChildEvent>(FindObjectsSortMode.None);

            foreach (ChildEvent child in childEvents)
            {
                child.timeBeforeReactivation = eventManager.childTimeBeforeReactivation;
                child.solSatisfactionChange = eventManager.childSolSatisfactionChange;
                child.solSanityChange = eventManager.childSolSanityChange;
                child.koSatisfactionChange = eventManager.childKoSatisfactionChange;
                child.koSanityChange = eventManager.childKoSanityChange;
            }
            
            DrinkEvent[] drinkEvents = FindObjectsByType<DrinkEvent>(FindObjectsSortMode.None);

            foreach (DrinkEvent drink in drinkEvents)
            {
                drink.timeBeforeReactivation = eventManager.drinkTimeBeforeReactivation;
                drink.solSatisfactionChange = eventManager.drinkSolSatisfactionChange;
                drink.solSanityChange = eventManager.drinkSolSanityChange;
                drink.koSatisfactionChange = eventManager.drinkKoSatisfactionChange;
                drink.koSanityChange = eventManager.drinkKoSanityChange;
                drink.failSatisfactionChange = eventManager.drinkFailSatisfactionChange;
                drink.failSanityChange = eventManager.drinkFailSanityChange;
                drink.overlapArea = eventManager.drinkOverlapArea;
            }
            
            MusicEvent[] musicEvents = FindObjectsByType<MusicEvent>(FindObjectsSortMode.None);

            foreach (MusicEvent music in musicEvents)
            {
                music.phonePrefab = eventManager.musicPhonePrefab;
                music.timeBeforeReactivation = eventManager.musicTimeBeforeReactivation;
                music.solSatisfactionChange = eventManager.musicSolSatisfactionChange;
                music.solSanityChange = eventManager.musicSolSanityChange;
                music.koSatisfactionChange = eventManager.musicKoSatisfactionChange;
                music.koSanityChange = eventManager.musicKoSanityChange;
                music.overlapArea = eventManager.musicOverlapArea;
            }
        }
            
        GUILayout.EndVertical();
    }
}
