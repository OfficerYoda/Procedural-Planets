using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public RandomShapeSettings randomShapeSettings;
    public RandomColorSettings randomColorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout, colorSettingsFoldout, rdmShapeSettingsFoldout, rdmColorSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();
    RandomPlanetGenerator randomPlanetGenerator = new RandomPlanetGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void Initialize() {
        shapeGenerator.UpdateSettings(shapeSettings, shapeSettings.seed);
        colorGenerator.UpdateSettings(colorSettings, shapeSettings.seed);

        if(meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for(int i = 0; i < 6; i++) {
            if(meshFilters[i] == null) {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet() {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void GenerateRandomPlanet() {
        randomPlanetGenerator.GenerateRandomValues(ref shapeSettings, ref colorSettings, randomShapeSettings, randomColorSettings);

        GeneratePlanet();
    }

    public void OnShapeSettingsUpdated() {
        if(!autoUpdate) return;

        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated() {
        if(!autoUpdate) return;

        Initialize();
        GenerateColors();
    }

    public void OnRandomPlanetSettingUpdated() {

        GenerateRandomPlanet();
    }

    public void OnRandomShapeUpdated() {
        if(!autoUpdate) return;

        shapeSettings.SetValues(randomPlanetGenerator.randomShapeGenerator.GenerateRandomSettings(randomShapeSettings));
        OnShapeSettingsUpdated();
    }

    public void OnRandomColorUpdated() {
        if(!autoUpdate) return;

        colorSettings.SetValues(randomPlanetGenerator.randomColorGenerator.GenerateRandomSettings(colorSettings, randomColorSettings));
        OnColorSettingsUpdated();
    }

    private void GenerateMesh() {
        for(int i = 0; i < 6; i++) {
            if(meshFilters[i].gameObject.activeSelf) {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    private void GenerateColors() {
        colorGenerator.UpdateColors();
        for(int i = 0; i < 6; i++) {
            if(meshFilters[i].gameObject.activeSelf) {
                terrainFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }
}
