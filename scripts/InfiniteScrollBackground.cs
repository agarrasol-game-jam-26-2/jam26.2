using UnityEngine;

public class InfiniteScrollBackground : MonoBehaviour
{
    public float scrollSpeed = 1f;

    private SpriteRenderer spriteRenderer;
    private float spriteHeight;
    private Transform bgTransform2;
    private SpriteRenderer spriteRenderer2;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("InfiniteScrollBackground: SpriteRenderer não encontrado!");
            return;
        }

        spriteHeight = spriteRenderer.bounds.size.y;

        GameObject bg2 = new GameObject(gameObject.name + " (Copy)");
        bg2.transform.SetParent(transform.parent);
        bg2.transform.position = new Vector3(transform.position.x, transform.position.y + spriteHeight, transform.position.z);
        bg2.transform.localScale = transform.localScale;
        bgTransform2 = bg2.transform;

        spriteRenderer2 = bg2.AddComponent<SpriteRenderer>();
        spriteRenderer2.sprite = spriteRenderer.sprite;
        spriteRenderer2.material = spriteRenderer.material;
        spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder;
        spriteRenderer2.color = spriteRenderer.color;
    }

    void Update()
    {
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        bgTransform2.position += Vector3.down * scrollSpeed * Time.deltaTime;

        if (transform.position.y <= -spriteHeight)
            transform.position = new Vector3(transform.position.x, bgTransform2.position.y + spriteHeight, transform.position.z);

        if (bgTransform2.position.y <= -spriteHeight)
            bgTransform2.position = new Vector3(bgTransform2.position.x, transform.position.y + spriteHeight, bgTransform2.position.z);
    }
}
