using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPlanetGenerator {

    private static GradientAlphaKey[] alphaKeys = { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };

    public RandomShapeGenerator randomShapeGenerator = new RandomShapeGenerator();
    public RandomColorGenerator randomColorGenerator = new RandomColorGenerator();

    public void GenerateRandomValues(ref ShapeSettings shapeSettings, ref ColorSettings colorSettings, RandomShapeSettings randomShapeSettings, RandomColorSettings randomColorSettings) {
        shapeSettings.SetValues(randomShapeGenerator.GenerateRandomSettings(randomShapeSettings));
        colorSettings.SetValues(randomColorGenerator.GenerateRandomSettings(colorSettings, randomColorSettings));
    }

    public static NoiseSettings GenerateNoiseSettings(RandomNoiseSettings settings, NoiseSettings.FilterType filterType, bool firstLayer) {
        NoiseSettings.SimpleNoiseSettings simpleSettings = new NoiseSettings.SimpleNoiseSettings();
        simpleSettings.strength = strength();
        simpleSettings.numLayers = Random.Range(settings.numLayers.x, settings.numLayers.y);
        simpleSettings.baseRoughness = Random.Range(settings.baseRoughness.x, settings.baseRoughness.y);
        simpleSettings.roughness = Random.Range(settings.roughness.x, settings.roughness.y);
        simpleSettings.persistence = Random.Range(settings.persistance.x, settings.persistance.y);
        simpleSettings.centre = Vector3.zero;
        simpleSettings.minValue = Random.Range(settings.minValue.x, settings.minValue.y);

        NoiseSettings.RigidNoiseSettings rigidSettings = new NoiseSettings.RigidNoiseSettings();
        rigidSettings.SetParentValues(simpleSettings);
        rigidSettings.weightMultiplier = Random.Range(settings.weightMultiplier.x, settings.weightMultiplier.y);

        NoiseSettings noiseSettings = new NoiseSettings();
        noiseSettings.filterType = filterType;
        noiseSettings.simpleNoiseSettings = simpleSettings;
        noiseSettings.rigidNoiseSettings = rigidSettings;

        float strength() {
            if(firstLayer) {
                return Random.Range(settings.noiseStrengthFirstLayer.x, settings.noiseStrengthFirstLayer.y);
            } else {
                return Random.Range(settings.noiseStrength.x, settings.noiseStrength.y);
            }
        }

        return noiseSettings;
    }

    public class RandomShapeGenerator {

        public ShapeSettings GenerateRandomSettings(RandomShapeSettings rdmSettings) {
            ShapeSettings settings = ScriptableObject.CreateInstance<ShapeSettings>();
            settings.seed = rdmSettings.randomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : 0;
            settings.planetRadius = Random.Range(rdmSettings.planetRadius.x, rdmSettings.planetRadius.y);
            settings.noiseLayers = GenerateNoiseLayers(rdmSettings);

            return settings;
        }

        private NoiseLayer[] GenerateNoiseLayers(RandomShapeSettings settings) {
            NoiseLayer[] noiseLayers = new NoiseLayer[settings.noiseLayers];

            for(int i = 0; i < noiseLayers.Length; i++) {
                NoiseLayer layer = new NoiseLayer();
                layer.enabled = true;
                layer.useFirstLayerAsMask = settings.useFirstLayerAsMask;
                layer.noiseSettings = GenerateNoiseSettings(settings.randomNoiseSettings, settings.filterTypes[i], i == 0);

                noiseLayers[i] = layer;
            }

            return noiseLayers;
        }
    }

    public class RandomColorGenerator {

        public ColorSettings GenerateRandomSettings(ColorSettings oldSettings, RandomColorSettings rdmSettings) {
            ColorSettings settings = ScriptableObject.CreateInstance<ColorSettings>();
            settings.planetMaterial = rdmSettings.planetMaterial;
            settings.biomeColorSettings = GenerateBiomeSettings(rdmSettings);
            settings.oceanColor = GenerateOceanGradient(rdmSettings);

            return settings;
        }

        private Gradient GenerateOceanGradient(RandomColorSettings settings) {
            Gradient oceanGradient = new Gradient();
            Color oceanClr = GenerateRandomColor(settings.oceanBaseColor);
            Color key2 = BrighterColor(oceanClr, settings.ocean2ndKeyAdd);
            GradientColorKey[] colorKeys = { new GradientColorKey(oceanClr, 0f), new GradientColorKey(key2, settings.ocean2ndKeyLocation), new GradientColorKey(BrighterColor(key2, settings.oceanBrightUp.x, settings.oceanBrightUp.y), 1f) };
            oceanGradient.SetKeys(colorKeys, alphaKeys);

            return oceanGradient;
        }

        private ColorSettings.BiomeColorSettings GenerateBiomeSettings(RandomColorSettings settings) {
            ColorSettings.BiomeColorSettings biomeColorSettings = new ColorSettings.BiomeColorSettings();
            biomeColorSettings.biomes = GenerateBiomes(settings);
            biomeColorSettings.noise = GenerateNoiseSettings(settings.biomeMixNoise, settings.filterType, false);
            biomeColorSettings.noiseOffset = Random.Range(settings.noiseOffset.x, settings.noiseOffset.y);
            biomeColorSettings.noiseStrength = Random.Range(settings.noiseStrength.x, settings.noiseStrength.y);
            biomeColorSettings.blendAmount = Random.Range(settings.blendAmout.x, settings.blendAmout.y);

            return biomeColorSettings;
        }

        private ColorSettings.BiomeColorSettings.Biome[] GenerateBiomes(RandomColorSettings settings) {
            ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[settings.numBiomes];

            float stepSize = 1f / settings.numBiomes;
            for(int i = 0; i < settings.numBiomes; i++) {
                ColorSettings.BiomeColorSettings.Biome biome = new ColorSettings.BiomeColorSettings.Biome();
                biome.tint = GenerateRandomColor(settings.tintColor);
                biome.gradient = GenerateRandomGradient(biome.tint, settings.biomeColorKeys, settings.tintColor);
                biome.tintPercent = Random.Range(settings.tintPercent.x, settings.tintPercent.y);
                biome.startHeight = Mathf.Max(0, i * stepSize + Random.Range(-settings.maxStartHeightOffset, settings.maxStartHeightOffset));

                biomes[i] = biome;
            }

            return biomes;
        }

        private Gradient GenerateRandomGradient(Color tintColor, Vector2Int clrKeysRange, RandomColorSettings.RandomHSVSettings colorSettings) {

            int numClrKeys = Random.Range(clrKeysRange.x, clrKeysRange.y + 1);
            GradientColorKey[] clrKeys = new GradientColorKey[numClrKeys];
            float keySteps = 1f / (numClrKeys - 1);

            for(int i = 1; i < numClrKeys - 1; i++) {
                Color color = GenerateRandomColor(colorSettings);
                clrKeys[i] = new GradientColorKey(color, i * keySteps);
            }

            clrKeys[0] = new GradientColorKey(tintColor, 0f);
            clrKeys[numClrKeys - 1].time = 1f;

            Gradient gradient = new Gradient();
            gradient.SetKeys(clrKeys, alphaKeys);

            return gradient;
        }

        private Color GenerateRandomColor(RandomColorSettings.RandomHSVSettings s) {
            return Random.ColorHSV(s.hue.x, s.hue.y, s.sat.x, s.sat.y, s.val.x, s.val.y, s.alp.x, s.alp.y);
        }

        private Color BrighterColor(Color color, float minAdd, float maxAdd) {
            return BrighterColor(color, Random.Range(minAdd, maxAdd));
        }

        private Color BrighterColor(Color color, float add) {
            Color.RGBToHSV(color, out float h, out float s, out float v);

            return Color.HSVToRGB(h, s, Mathf.Min(1, v + add));
        }
    }
}