using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
            sr = gameObject.GetComponent<SpriteRenderer>();
            col = gameObject.GetComponent<Collider2D>();
       
    }

    public IEnumerator StartFlickerEffect()
    {
        if (sr == null || col == null) yield break;

        for (int i = 0; i < 5; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);

        sr.enabled = true;
        col.isTrigger = true;

        Color tempColor = sr.color;
        tempColor.a = 0.5f;
        sr.color = tempColor;
    }

    public void ResetFieldPrefabChanges()
    {
        if (sr != null && col != null)
        {
            Color tempColor = sr.color;
            tempColor.a = 1f;
            sr.color = tempColor;

            col.isTrigger = false;
        }
    }
}
