using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
public class NoiseViewer : MonoBehaviour
{
    public bool autoUpdate;
    public NoiseValue noiseValue;
    public DrawMode drawMode;
    public enum DrawMode { NoiseMap, ColorMap };
    public TerrainType[] regions;

    public void GenerateNoiseMapViewer()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(noiseValue);

        Color[] colorMap= new Color[noiseValue.mapWidth * noiseValue.mapHeight];
        for (int y = 0; y < noiseValue.mapHeight; y++)
        {
            for (int x = 0; x < noiseValue.mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * noiseValue.mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        NoiseDisplay display = FindObjectOfType<NoiseDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, noiseValue.mapWidth, noiseValue.mapHeight));
    }
}

