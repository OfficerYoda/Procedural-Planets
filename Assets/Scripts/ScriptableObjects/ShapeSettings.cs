using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Shape Settings")]
public class ShapeSettings : ScriptableObject {

    public int seed = 0;
    public float planetRadius = 1;
    public NoiseLayer[] noiseLayers;

    public void SetValues(ShapeSettings other) {
        seed = other.seed;
        planetRadius = other.planetRadius;
        noiseLayers = other.noiseLayers;
    }
}

[System.Serializable]
public class NoiseLayer {
    public bool enabled = true;
    public bool useFirstLayerAsMask = true;
    public NoiseSettings noiseSettings;
}
