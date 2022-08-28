using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter {

    NoiseSettings.SimpleNoiseSettings setings;
    Noise noise;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings setings, int seed) {
        this.setings = setings;
        noise = new Noise(seed);
    }

    public float Evaluate(Vector3 point) {
        float noiseValue = 0;
        float frequency = setings.baseRoughness;
        float amplitude = 1f;

        for(int i = 0; i < setings.numLayers; i++) {
            float v = noise.Evaluate(point * frequency + setings.centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= setings.roughness;
            amplitude *= setings.persistence;
        }

        noiseValue = noiseValue - setings.minValue;
        return noiseValue * setings.strength;
    }
}
