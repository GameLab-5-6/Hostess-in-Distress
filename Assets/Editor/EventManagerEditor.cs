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
            LuggageEvent[]  luggageEvents = FindObjectsByType<LuggageEvent>(FindObjectsSortMode.None);

            foreach (LuggageEvent luggage in luggageEvents)
            {
                luggage.overlapRadius = eventManager.luggageOverlapRadius;
                if (luggage.onOppositeSide)
                    luggage.moveToAmount = -eventManager.luggageMoveToAmount;
                else
                    luggage.moveToAmount = eventManager.luggageMoveToAmount;
                
                luggage.luggageClip = eventManager.luggageClip;
                luggage.luggageVolume = eventManager.luggageVolume;
                luggage.solutionClip = eventManager.solutionClip;
                luggage.solutionVolume = eventManager.solutionVolume;
            }
            
            BabyEvent[] babyEvents = FindObjectsByType<BabyEvent>(FindObjectsSortMode.None);

            foreach (BabyEvent baby in babyEvents)
            {
                baby.clipPrefab = eventManager.clipPrefab;
                baby.eventClip = eventManager.babyClip;
                baby.eventVolume = eventManager.babyVolume;
                baby.solutionClip = eventManager.solutionClip;
                baby.solutionVolume = eventManager.solutionVolume;
                baby.koClip = eventManager.koClip;
                baby.koVolume = eventManager.koVolume;
                baby.timeInBetweenAudio = eventManager.babyTimeInBetweenAudio;
                baby.interactText = eventManager.babyInteractText;
                
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
                child.clipPrefab = eventManager.clipPrefab;
                child.eventClip = eventManager.childClip;
                child.eventVolume = eventManager.childVolume;
                child.solutionClip = eventManager.solutionClip;
                child.solutionVolume = eventManager.solutionVolume;
                child.koClip = eventManager.koClip;
                child.koVolume = eventManager.koVolume;
                child.exclamationPoint = eventManager.exclamationPoint;
                child.timeInBetweenAudio = eventManager.childTimeInBetweenAudio;
                
                child.timeBeforeReactivation = eventManager.childTimeBeforeReactivation;
                child.solSatisfactionChange = eventManager.childSolSatisfactionChange;
                child.solSanityChange = eventManager.childSolSanityChange;
                child.koSatisfactionChange = eventManager.childKoSatisfactionChange;
                child.koSanityChange = eventManager.childKoSanityChange;
            }
            
            DrinkEvent[] drinkEvents = FindObjectsByType<DrinkEvent>(FindObjectsSortMode.None);

            foreach (DrinkEvent drink in drinkEvents)
            {
                drink.clipPrefab = eventManager.clipPrefab;
                drink.eventClip = eventManager.drinkClip;
                drink.eventVolume = eventManager.drinkVolume;
                drink.solutionClip = eventManager.solutionClip;
                drink.solutionVolume = eventManager.solutionVolume;
                drink.koClip = eventManager.koClip;
                drink.koVolume = eventManager.koVolume;
                drink.timeInBetweenAudio = eventManager.drinkTimeInBetweenAudio;
                drink.interactText = eventManager.drinkInteractText;
                
                drink.timeBeforeReactivation = eventManager.drinkTimeBeforeReactivation;
                drink.solSatisfactionChange = eventManager.drinkSolSatisfactionChange;
                drink.solSanityChange = eventManager.drinkSolSanityChange;
                drink.koSatisfactionChange = eventManager.drinkKoSatisfactionChange;
                drink.koSanityChange = eventManager.drinkKoSanityChange;
                drink.failSatisfactionChange = eventManager.drinkFailSatisfactionChange;
                drink.failSanityChange = eventManager.drinkFailSanityChange;
                drink.overlapArea = eventManager.drinkOverlapArea;
                drink.drink1Bubble = eventManager.drink1Bubble;
                drink.drink2Bubble = eventManager.drink2Bubble;
            }
            
            MusicEvent[] musicEvents = FindObjectsByType<MusicEvent>(FindObjectsSortMode.None);

            foreach (MusicEvent music in musicEvents)
            {
                music.clipPrefab = eventManager.clipPrefab;
                music.eventClip = eventManager.musicClip;
                music.eventVolume = eventManager.musicVolume;
                music.solutionClip = eventManager.solutionClip;
                music.solutionVolume = eventManager.solutionVolume;
                music.koClip = eventManager.koClip;
                music.koVolume = eventManager.koVolume;
                music.exclamationPoint = eventManager.exclamationPoint;
                music.timeInBetweenAudio = eventManager.musicTimeInBetweenAudio;
                music.interactText = eventManager.musicInteractText;
                
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
