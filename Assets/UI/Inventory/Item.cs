using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Gameplay")]

    [Header("UI")]
    public bool stackable = true;
    public bool isPlaceable = false;

    [Header("Both")]
    public Sprite image;
}
