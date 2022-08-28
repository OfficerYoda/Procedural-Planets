using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Color Settings")]
public class ColorSettings : ScriptableObject {

    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    public void SetValues(ColorSettings other) {
        planetMaterial = other.planetMaterial;
        biomeColorSettings = other.biomeColorSettings;
        oceanColor = other.oceanColor;
    }

    [System.Serializable]
    public class BiomeColorSettings {

        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;

        [System.Serializable]
        public class Biome {

            public Color tint;
            public Gradient gradient;
            [Range(0, 1)]
            public float tintPercent, startHeight;

        }
    }
}
