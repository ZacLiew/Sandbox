using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tiles", menuName = "Tiles")]
public class TilesSheet : ScriptableObject
{
    public TileSprite[] tiles;
}

[System.Serializable]
public struct TileSprite
{
    public string name;
    public Tile tile;
}