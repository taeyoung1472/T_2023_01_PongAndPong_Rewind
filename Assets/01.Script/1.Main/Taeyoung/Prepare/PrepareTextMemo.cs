using System.Collections;
using TMPro;
using UnityEngine;

public class PrepareTextMemo : MonoBehaviour
{
    private TextMeshPro tmp;
    private PrepareManager manager;
    private SpriteRenderer spriteRenderer;

    public void SetText(string text)
    {
        tmp.SetText(text);

        StartCoroutine(ScaleSync());
    }

    IEnumerator ScaleSync()
    {
        yield return null;

        Vector2 size = tmp.GetRenderedValues() + Vector2.one;
        spriteRenderer.size = new Vector2(size.x, size.y);
    }

    public void Init(TextMeshPro tmp, PrepareManager manager)
    {
        this.manager = manager;
        this.tmp = tmp;
        manager.StartWriting(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        transform.localScale = Vector3.one;
    }
}
