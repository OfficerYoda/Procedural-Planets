using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings {


    public enum FilterType { Simple, Rigid }
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;


    [System.Serializable]
    public class SimpleNoiseSettings {

        public float strength = 1f;
        [Range(1, 8)]
        public int numLayers = 1;
        public float baseRoughness = 1f;
        public float roughness = 2f;
        public float persistence = 0.5f;
        public Vector3 centre;
        public float minValue;

    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings {

        public void SetParentValues(SimpleNoiseSettings other) {
            strength = other.strength;
            numLayers = other.numLayers;
            baseRoughness = other.baseRoughness;
            roughness = other.roughness;
            persistence = other.persistence;
            centre = other.centre;
            minValue = other.minValue;
        }

        public float weightMultiplier = 0.8f;

    }
}
