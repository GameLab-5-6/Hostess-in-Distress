using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrinkEvent))]
public class DrinkEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrinkEvent drinkEvent = (DrinkEvent)target;
        
        DrawDefaultInspector();

        GUILayout.BeginVertical();

            GUILayout.Space(10f);
            
            GUILayout.Label("Apply changes to all Drink Events", EditorStyles.boldLabel);

            if (GUILayout.Button("Override All"))
            {
                DrinkEvent[] drinkEvents = FindObjectsByType<DrinkEvent>(FindObjectsSortMode.None);

                foreach (DrinkEvent drink in drinkEvents)
                {
                    drink.timeBeforeReactivation = drinkEvent.timeBeforeReactivation;
                    drink.solSatisfactionChange = drinkEvent.solSatisfactionChange;
                    drink.solSanityChange = drinkEvent.solSanityChange;
                    drink.koSatisfactionChange = drinkEvent.koSatisfactionChange;
                    drink.koSanityChange = drinkEvent.koSanityChange;
                    drink.failSatisfactionChange = drinkEvent.failSatisfactionChange;
                    drink.failSanityChange = drinkEvent.failSanityChange;
                    drink.overlapArea = drinkEvent.overlapArea;
                }
            }
            
        GUILayout.EndVertical();
    }
}
