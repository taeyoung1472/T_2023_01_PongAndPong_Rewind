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

    Highlighter h1;

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
