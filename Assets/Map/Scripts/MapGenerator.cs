using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public NoiseValue[] noiseValue;
    public TilesSheet tileSheet;
    public Tilemap tilemap;
    public GameObject player;

    private void Start()
    {
        noiseValue[0].seed = Random.Range(-10000, 10000);
        Debug.Log(noiseValue[0].seed);
        GenerateMap();
    }

    public void GenerateMap()
    {
        tilemap.ClearAllTiles();
        float[,] noiseMap = Noise.GenerateNoiseMap(noiseValue[0]);
        for (int x = 0; x < noiseValue[0].mapWidth; x++)
        {
            for (int y = 0; y < 1; y++)
            {
                for (int i = 0; i <= noiseMap[x, y + 95] * 10 + 5; i++)
                {
                    tilemap.SetTile(new Vector3Int(x, y + i + 95, 0), tileSheet.tiles[0].tile);
                }
            }
        }
        for (int x = 0; x < noiseValue[0].mapWidth; x++)
        {
            for (int y = 0; y < noiseValue[0].mapHeight; y++)
            {
                float currentHeight = noiseMap[x, y];
                if (currentHeight <= 0.6f)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileSheet.tiles[1].tile);
            }
        }
        StartCoroutine(WaitForGenerate());
    }

    private IEnumerator WaitForGenerate()
    {
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.Scan(AstarPath.active.data.gridGraph);
        player.GetComponent<Player>().MoveToOpenSpace();
    }


}