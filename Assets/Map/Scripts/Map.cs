using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap highlightTilemap;
    [SerializeField] private Tile[] highlightTilesArray;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform player;
    [SerializeField] private float playerMineLength = 3;

    private Vector3Int highlightedPos;
    private Dictionary<string, Tile> highlightTiles;


    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Item[] item;

    private Dictionary<string, Item> itemDict;
    // Start is called before the first frame update
    void Start()
    {
        highlightedPos = Vector3Int.zero;
        highlightTiles = new Dictionary<string, Tile>();
        foreach (Tile i in highlightTilesArray)
        {
            highlightTiles[i.name] = i;
        }
        itemDict = new Dictionary<string, Item>();
        foreach (Item i in item)
        {
            itemDict[i.name] = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighlightTile();
    }

    private void UpdateHighlightTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = tilemap.WorldToCell(mousePos);
        Vector3 direction = (mousePos - player.position).normalized;

        Tile selectedHighlightTile = highlightTiles["red"];
        if (RayCheckInRange(player.position, direction, playerMineLength, whatIsGround, mousePos))
        {
            selectedHighlightTile = highlightTiles["white"];
        }

        if (highlightedPos != tilePos || selectedHighlightTile != highlightTilemap.GetTile(highlightedPos))
        {
            highlightTilemap.SetTile(highlightedPos, null);
            highlightTilemap.SetTile(tilePos, selectedHighlightTile);
            highlightedPos = tilePos;
        }
    }

    public void ChangeTile(string tileMapName, Vector3 targetPos, Tile tile)
    {
        Tilemap map = tilemap;
        Vector3Int tilePos = tilemap.WorldToCell(targetPos);

        if (tileMapName == "Main")
        {
            map = tilemap;
            if (tile == null)
            {
                Item i = itemDict[map.GetTile(tilePos).name];
                Vector3 lootPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, tilePos.z);
                GameObject tmp = Instantiate(itemPrefab, lootPos, Quaternion.identity);
                tmp.GetComponent<Loot>().Initialize(i);
            }
            Bounds bounds = new Bounds(targetPos, new Vector3(1f, 1f, 1f));
            AstarPath.active.UpdateGraphs(bounds);
        }
        else if (tileMapName == "highlight")
            map = highlightTilemap;
        map.SetTile(tilePos, tile);
    }

    public bool CheckEmpty(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(targetPos);

        if (hit == null)
            return true;
        return false;
    }

    public bool RayCheckInRange(Vector3 position, Vector3 direction, float length, LayerMask layer, Vector3 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, length, layer);
        Vector3Int tilePos = tilemap.WorldToCell(targetPos);

        if (hit.collider != null)
        {
            Vector3Int hitPos = tilemap.WorldToCell(new Vector3(hit.point.x + direction.x, hit.point.y + direction.y, 0));
            if (hitPos == tilePos)
                return (true);
        }
        return (false);
    }
}