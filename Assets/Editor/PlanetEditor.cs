using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor {

    Planet planet;
    Editor shapeEditor;
    Editor colorEditor;
    Editor rdmShapeEditor;
    Editor rdmColorEditor;

    private void OnEnable() {
        planet = (Planet)target;
    }

    public override void OnInspectorGUI() {
        using(var check = new EditorGUI.ChangeCheckScope()) {
            base.OnInspectorGUI();
            if(check.changed)
                planet.GeneratePlanet();
        }

        GUILayout.Space(5);
        if(GUILayout.Button("Generate Planet")) planet.GeneratePlanet();
        GUILayout.Space(5);
        if(GUILayout.Button("Generate Random Planet")) planet.GenerateRandomPlanet();
        if(GUILayout.Button("Generate Random Colors")) planet.OnRandomColorUpdated();
        if(GUILayout.Button("Generate Random Shape")) planet.OnRandomShapeUpdated();

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
        DrawSettingsEditor(planet.randomShapeSettings, planet.OnRandomShapeUpdated, ref planet.rdmShapeSettingsFoldout, ref rdmShapeEditor);
        DrawSettingsEditor(planet.randomColorSettings, planet.OnRandomColorUpdated, ref planet.rdmColorSettingsFoldout, ref rdmColorEditor);
    }

    private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor) {

        if(settings == null) return;
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        using(var check = new EditorGUI.ChangeCheckScope()) {
            if(foldout) {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();
            }

            if(check.changed) {
                onSettingsUpdated?.Invoke();
            }
        }
    }
}
