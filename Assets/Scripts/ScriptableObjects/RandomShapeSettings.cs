using UnityEngine;
using GD.MinMaxSlider;

[CreateAssetMenu(menuName = "Planet/Random/Shape Settings")]
public class RandomShapeSettings : ScriptableObject {

    [Header("Planet")]
    public bool randomizeSeed = true;
    [MinMaxSlider(1, 50)]
    public Vector2 planetRadius = new Vector2(1, 10);

    [Header("Noise")]
    [Range(0, 5)]
    public int noiseLayers = 2;
    public bool useFirstLayerAsMask = true;

    [Header("Noise Settings")]
    public NoiseSettings.FilterType[] filterTypes = new NoiseSettings.FilterType[2];

    public RandomNoiseSettings randomNoiseSettings;

    private void OnValidate() {
        // sets the length of the filterTypes array to noiseLayers
        if(noiseLayers != filterTypes.Length) {
            NoiseSettings.FilterType[] oldTypes = filterTypes;
            filterTypes = new NoiseSettings.FilterType[noiseLayers];

            for(int i = 0; i < Mathf.Min(oldTypes.Length, filterTypes.Length); i++) {
                filterTypes[i] = oldTypes[i];
            }
        }
    }
}
