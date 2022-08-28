using UnityEngine;
using GD.MinMaxSlider;

[System.Serializable]
public class RandomNoiseSettings {

    [Header("Simple Noise Settings")]
    [MinMaxSlider(0f, 1f)]
    public Vector2 noiseStrengthFirstLayer = new Vector2(0f, 0.2f);
    [MinMaxSlider(0f, 10f)]
    public Vector2 noiseStrength = new Vector2(0f, 2f);
    [MinMaxSlider(1, 8)]
    public Vector2Int numLayers = new Vector2Int(2, 4);
    [MinMaxSlider(0f, 5f)]
    public Vector2 baseRoughness = new Vector2(0f, 3f);
    [MinMaxSlider(0f, 5f)]
    public Vector2 roughness = new Vector2(0f, 3f);
    [MinMaxSlider(0f, 1f)]
    public Vector2 persistance = new Vector2(0.4f, 0.5f);
    [MinMaxSlider(0.5f, 1.5f)]
    public Vector2 minValue = new Vector2(0.9f, 1.1f);

    [Header("Rigid Noise Settings")]
    [MinMaxSlider(0f, 3f)]
    public Vector2 weightMultiplier = new Vector2(0f, 2f);

}