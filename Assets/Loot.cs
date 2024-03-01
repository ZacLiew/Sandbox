using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private SpriteRenderer sp;
    private BoxCollider2D BCollider;
    [SerializeField] private float moveSpeed;

    private Item item;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        BCollider = GetComponent<BoxCollider2D>();
    }
    public void Initialize(Item i)
    {
        Debug.Log(i.image);
        this.item = i;
        sp.sprite = i.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && item != null)
        {
            bool isAdded = InventoryManager.instance.AddItem(item);

            if (isAdded)
                StartCoroutine(MoveAndCollect(collision.transform));
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(BCollider);
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;
        }
        Destroy(gameObject);
    }
}
