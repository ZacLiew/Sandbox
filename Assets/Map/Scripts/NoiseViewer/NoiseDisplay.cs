using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDisplay : MonoBehaviour
{
    public SpriteRenderer textureRenderer;

    public void DrawTexture(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        textureRenderer.sprite = sprite;
        textureRenderer.transform.localScale = new Vector3(texture.width, texture.height, 0);
    }
}
