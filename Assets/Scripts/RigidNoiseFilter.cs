using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter {

    NoiseSettings.RigidNoiseSettings setings;
    Noise noise;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings setings, int seed) {
        this.setings = setings;
        noise = new Noise(seed);
    }

    public float Evaluate(Vector3 point) {
        float noiseValue = 0;
        float frequency = setings.baseRoughness;
        float amplitude = 1f;
        float weight = 1;

        for(int i = 0; i < setings.numLayers; i++) {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + setings.centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * setings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= setings.roughness;
            amplitude *= setings.persistence;
        }

        noiseValue = noiseValue - setings.minValue;
        return noiseValue * setings.strength;
    }
}
