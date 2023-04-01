using DigitalRuby.ThunderAndLightning;
using Highlighters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GimmickVisualLink : MonoBehaviour
{
    [SerializeField] private LightningBoltPathScript lightning;
    [SerializeField] private Transform[] pathArr;

    float linkTimer = 0.1f;

    Transform start, end;

    private bool isLinked = false;

    public void Link(Transform start, Transform end, Color color)
    {
        this.start = start;
        this.end = end;

        float depth = Mathf.Log10(Vector3.Distance(start.position, end.position)) * 2.5f;
        for (int i = 0; i < pathArr.Length; i++)
        {
            pathArr[i].position = Vector3.Lerp(start.position, end.position, (float)i / (pathArr.Length - 1));
            pathArr[i].position = pathArr[i].position + Vector3.back * Mathf.Sin(((float)i / (pathArr.Length - 1)) * Mathf.PI) * depth;
        }

        lightning.gameObject.SetActive(true);
        lightning.LightningTintColor = color;

        if(start.GetComponent<Highlighter>() == null)
        {
            Highlighter highlighter = start.AddComponent<Highlighter>();
            highlighter.GetRenderers();
            highlighter.Settings.DepthMask = DepthMask.Both;
            highlighter.Settings.UseMeshOutline = true;
            highlighter.Settings.MeshOutlineThickness = 0.02f;
            highlighter.Settings.MeshOutlineFront.Color = color;

            highlighter.Settings.UseOverlay = true;
            highlighter.Settings.UseSingleOverlay = false;
            highlighter.Settings.OverlayFront.Color = new Color(1, 1, 1, 0);
            highlighter.Settings.OverlayBack.Color = color * new Color(1, 1, 1, 0.2f);
        }


        if (end.GetComponent<Highlighter>() == null)
        {
            Highlighter highlighter = end.AddComponent<Highlighter>();
            highlighter.GetRenderers();
            highlighter.Settings.DepthMask = DepthMask.Both;
            highlighter.Settings.UseMeshOutline = true;
            highlighter.Settings.MeshOutlineThickness = 0.02f;
            highlighter.Settings.MeshOutlineFront.Color = color;

            highlighter.Settings.UseOverlay = true;
            highlighter.Settings.UseSingleOverlay = false;
            highlighter.Settings.OverlayFront.Color = new Color(1, 1, 1, 0);
            highlighter.Settings.OverlayBack.Color = color * new Color(1, 1, 1, 0.2f);
        }
        Highlighter.HighlightersNeedReset();

        isLinked = true;
    }

    private void Update()
    {
        if (!isLinked)
            return;

        linkTimer -= Time.deltaTime;
        if(linkTimer < 0)
        {
            linkTimer = 0.1f;
            float depth = Mathf.Log10(Vector3.Distance(start.position, end.position)) * 2.5f;
            for (int i = 0; i < pathArr.Length; i++)
            {
                pathArr[i].position = Vector3.Lerp(start.position, end.position, (float)i / (pathArr.Length - 1));
                pathArr[i].position = pathArr[i].position + Vector3.back * Mathf.Sin(((float)i / (pathArr.Length - 1)) * Mathf.PI) * depth;
            }
        }
    }
}
