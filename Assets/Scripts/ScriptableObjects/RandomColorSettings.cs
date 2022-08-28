using System;
using UnityEngine;
using GD.MinMaxSlider;

[CreateAssetMenu(menuName = "Planet/Random/Color Settings")]
public class RandomColorSettings : ScriptableObject {

    public Material planetMaterial;

    [Header("Ocean")]
    public RandomHSVSettings oceanBaseColor;
    [MinMaxSlider(0f, 1f)]
    public Vector2 oceanBrightUp = new Vector2(0.1f, 0.5f);
    [Range(0, 1)]
    public float ocean2ndKeyAdd = 0.25f;
    [Range(0, 1)]
    public float ocean2ndKeyLocation = 0.75f;

    [Header("Biome Color Settings")]
    public int numBiomes = 3;
    [MinMaxSlider(2, 10)]
    public Vector2Int biomeColorKeys = new Vector2Int(2, 5);
    public RandomHSVSettings tintColor;
    [MinMaxSlider(0f, 1f)]
    public Vector2 tintPercent = new Vector2(0f, 0.35f);
    [Range(0f, 1f)]
    public float maxStartHeightOffset = 0.2f;

    [Header("Noise")]
    public NoiseSettings.FilterType filterType = NoiseSettings.FilterType.Simple;
    public RandomNoiseSettings biomeMixNoise;
    [MinMaxSlider(-1f, 1f)]
    public Vector2 noiseOffset = new Vector2(-0.5f, 0.5f);
    [MinMaxSlider(0f, 1f)]
    public Vector2 noiseStrength = new Vector2(0f, 0.2f);
    [MinMaxSlider(0f, 1f)]
    public Vector2 blendAmout = new Vector2(0.1f, 0.3f);

    [System.Serializable]
    public class RandomHSVSettings {

        [MinMaxSlider(0f, 1f)]
        public Vector2 hue = new Vector2(0, 1);
        [MinMaxSlider(0f, 1f)]
        public Vector2 sat = new Vector2(0.5f, 1f);
        [MinMaxSlider(0f, 1f)]
        public Vector2 val = new Vector2(0.2f, 0.5f);
        [MinMaxSlider(0f, 1f)]
        public Vector2 alp = new Vector2(1f, 1f);

    }
}