using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class DeployHandler : MonoBehaviour
{
    public DeployType deployType;

    public Vector3 firstAxis = new Vector3(1, 0, 1);
    public Vector3 secondAxis = new Vector3(-1, 0, -1);

    [Header("[Prefab 정보]")]
    public MapElement prefab;
    public float prefabSizeFactor = 1;
    public bool centerX, centerY;
    [Header("[Decal 정보]")]
    public MapElement decalPrefab;
    [Range(0.0f, 1.0f)] public float decalChance;
    public float decalSizeFactor = 1;
    public bool centerX_decal, centerY_decal, randomX, randomY, rotX, rotY, rotZ;

    public static DrawMode drawMode;

    private Vector2 size;

    private BoxCollider boxCollider;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (drawMode == DrawMode.None)
            return;

        switch (deployType)
        {
            case DeployType.XY:
                firstAxis.x = (float)Math.Round(firstAxis.x, 3);
                firstAxis.y = (float)Math.Round(firstAxis.y, 3);
                secondAxis.x = (float)Math.Round(secondAxis.x, 3);
                secondAxis.y = (float)Math.Round(secondAxis.y, 3);

                firstAxis.z = 0;
                secondAxis.z = 0;
                break;
            case DeployType.XZ:
                firstAxis.x = (float)Math.Round(firstAxis.x, 3);
                firstAxis.z = (float)Math.Round(firstAxis.z, 3);
                secondAxis.x = (float)Math.Round(secondAxis.x, 3);
                secondAxis.z = (float)Math.Round(secondAxis.z, 3);

                firstAxis.y = 0;
                secondAxis.y = 0;
                break;
            case DeployType.YZ:
                firstAxis.y = (float)Math.Round(firstAxis.y, 3);
                firstAxis.z = (float)Math.Round(firstAxis.z, 3);
                secondAxis.y = (float)Math.Round(secondAxis.y, 3);
                secondAxis.z = (float)Math.Round(secondAxis.z, 3);

                firstAxis.x = 0;
                secondAxis.x = 0;
                break;
        }

        Vector3 center = (firstAxis + secondAxis) / 2;

        switch (deployType)
        {
            case DeployType.XY:
                center.z = transform.position.z;
                break;
            case DeployType.XZ:
                center.y = transform.position.y;
                break;
            case DeployType.YZ:
                center.x = transform.position.x;
                break;
        }

        float x = 0.0f;
        float y = 0.0f;
        switch (deployType)
        {
            case DeployType.XY:
                x = Mathf.Abs(firstAxis.x - secondAxis.x);
                y = Mathf.Abs(firstAxis.y - secondAxis.y);
                break;
            case DeployType.XZ:
                x = Mathf.Abs(firstAxis.x - secondAxis.x);
                y = Mathf.Abs(firstAxis.z - secondAxis.z);
                break;
            case DeployType.YZ:
                x = Mathf.Abs(firstAxis.y - secondAxis.y);
                y = Mathf.Abs(firstAxis.z - secondAxis.z);
                break;
        }

        this.size.x = x;
        this.size.y = y;
        Vector3 size = Vector3.one;

        switch (deployType)
        {
            case DeployType.XY:
                size = new Vector3(this.size.x, this.size.y, 0);
                if (drawMode == DrawMode.Mesh)
                    size.z = 0.1f;
                break;
            case DeployType.XZ:
                size = new Vector3(this.size.x, 0, this.size.y);
                if (drawMode == DrawMode.Mesh)
                    size.y = 0.1f;
                break;
            case DeployType.YZ:
                size = new Vector3(0, this.size.x, this.size.y);
                if (drawMode == DrawMode.Mesh)
                    size.x = 0.1f;
                break;
        }

        if (drawMode == DrawMode.Wire)
            Gizmos.DrawWireCube(center, size);
        else
        {
            Gizmos.color = new Color(1, 1, 1, 0.75f);
            Gizmos.DrawCube(center, size);
            Gizmos.color = Color.white;
            return;
        }

        GUIStyle labelStyle = new();

        if (Selection.activeObject == gameObject)
        {
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 32;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
        }
        else
        {
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 16;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;
        }


        switch (deployType)
        {
            case DeployType.XY:
                Handles.Label(center, $"{center.z}M", labelStyle);
                Handles.Label(new Vector3(center.x, center.y + (y / 2), transform.position.z), $"{this.size.x}M", labelStyle);
                Handles.Label(new Vector3(center.x + (x / 2), center.y, transform.position.z), $"{this.size.y}M", labelStyle);
                break;
            case DeployType.XZ:
                Handles.Label(center, $"{center.y}M", labelStyle);
                Handles.Label(new Vector3(center.x, transform.position.y, center.z + (y / 2)), $"{this.size.x}M", labelStyle);
                Handles.Label(new Vector3(center.x + (x / 2), transform.position.y, center.z), $"{this.size.y}M", labelStyle);
                break;
            case DeployType.YZ:
                Handles.Label(center, $"{center.x}M", labelStyle);
                Handles.Label(new Vector3(transform.position.x, center.y, center.z + (y / 2)), $"{this.size.x}M", labelStyle);
                Handles.Label(new Vector3(transform.position.x, center.y + (x / 2), center.z), $"{this.size.y}M", labelStyle);
                break;
        }
    }
#endif

    public void Generate()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        float minX = 0, maxX = 0, minY = 0, maxY = 0;

        switch (deployType)
        {
            case DeployType.XY:
                minX = Mathf.Min(firstAxis.x, secondAxis.x);
                maxX = Mathf.Max(firstAxis.x, secondAxis.x);

                minY = Mathf.Min(firstAxis.y, secondAxis.y);
                maxY = Mathf.Max(firstAxis.y, secondAxis.y);
                break;
            case DeployType.XZ:
                minX = Mathf.Min(firstAxis.x, secondAxis.x);
                maxX = Mathf.Max(firstAxis.x, secondAxis.x);

                minY = Mathf.Min(firstAxis.z, secondAxis.z);
                maxY = Mathf.Max(firstAxis.z, secondAxis.z);
                break;
            case DeployType.YZ:
                minX = Mathf.Min(firstAxis.y, secondAxis.y);
                maxX = Mathf.Max(firstAxis.y, secondAxis.y);

                minY = Mathf.Min(firstAxis.z, secondAxis.z);
                maxY = Mathf.Max(firstAxis.z, secondAxis.z);
                break;
        }
        foreach (var collider in transform.GetComponents<BoxCollider>())
        {
            DestroyImmediate(collider);
        }
        boxCollider = transform.AddComponent<BoxCollider>();
        switch (deployType)
        {
            case DeployType.XY:
                boxCollider.size = new Vector3(maxX - minX, maxY - minY, 0);
                break;
            case DeployType.XZ:
                boxCollider.size = new Vector3(maxX - minX, 0f, maxY - minY);
                break;
            case DeployType.YZ:
                boxCollider.size = new Vector3(0f, maxX - minX, maxY - minY);
                break;
        }

        float curX = minX;
        float curY = minY;
        if (prefab != null)
        {
            while (curX < maxX)
            {
                while (curY < maxY)
                {
                    Vector3 pos = new();
                    switch (deployType)
                    {
                        case DeployType.XY:
                            pos = new Vector3(curX + (centerX ? (prefab.size.x * prefabSizeFactor) / 2 : 0), curY + (centerY ? (prefab.size.y * prefabSizeFactor) / 2 : 0), transform.position.z);
                            curY += prefab.size.y * prefabSizeFactor;
                            break;
                        case DeployType.XZ:
                            pos = new Vector3(curX + (centerX ? (prefab.size.x * prefabSizeFactor) / 2 : 0), transform.position.y, curY + (centerY ? (prefab.size.z * prefabSizeFactor) / 2 : 0));
                            curY += prefab.size.z * prefabSizeFactor;
                            break;
                        case DeployType.YZ:
                            pos = new Vector3(transform.position.x, curX + (centerX ? (prefab.size.y * prefabSizeFactor) / 2 : 0), curY + (centerY ? (prefab.size.z * prefabSizeFactor) / 2 : 0));
                            curY += prefab.size.z * prefabSizeFactor;
                            break;
                    }
                    GameObject obj = Instantiate(prefab, pos, Quaternion.identity).gameObject;
                    obj.transform.localScale = Vector3.one * prefabSizeFactor;
                    obj.transform.parent = transform;
                }
                curY = minY;
                switch (deployType)
                {
                    case DeployType.XY:
                        curX += prefab.size.x * prefabSizeFactor;
                        break;
                    case DeployType.XZ:
                        curX += prefab.size.x * prefabSizeFactor;
                        break;
                    case DeployType.YZ:
                        curX += prefab.size.y * prefabSizeFactor;
                        break;
                }
            }
        }
        if (decalPrefab != null)
        {
            curX = minX;
            curY = minY;
            while (curX < maxX)
            {
                while (curY < maxY)
                {
                    Vector3 decalPos = new();
                    switch (deployType)
                    {
                        case DeployType.XY:
                            decalPos = new Vector3(curX + (centerX_decal ? (decalPrefab.size.x * decalSizeFactor) / 2 : 0), curY + (centerY_decal ? (decalPrefab.size.y * decalSizeFactor) / 2 : 0), transform.position.z);
                            curY += (!randomY ? decalPrefab.size.y : decalPrefab.size.y * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                            break;
                        case DeployType.XZ:
                            decalPos = new Vector3(curX + (centerX_decal ? (decalPrefab.size.x * decalSizeFactor) / 2 : 0), transform.position.y, curY + (centerY_decal ? (decalPrefab.size.z * decalSizeFactor) / 2 : 0));
                            curY += (!randomY ? decalPrefab.size.z : decalPrefab.size.z * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                            break;
                        case DeployType.YZ:
                            decalPos = new Vector3(transform.position.x, curX + (centerX_decal ? (decalPrefab.size.y * decalSizeFactor) / 2 : 0), curY + (centerY_decal ? (decalPrefab.size.z * decalSizeFactor) / 2 : 0));
                            curY += (!randomY ? decalPrefab.size.z : decalPrefab.size.z * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                            break;
                    }

                    if (UnityEngine.Random.value < decalChance)
                    {
                        Quaternion rot = Quaternion.identity;
                        rot.eulerAngles = new Vector3(rotX ? Random.Range(0.0f, 180.0f) : 0, rotY ? Random.Range(0.0f, 180.0f) : 0, rotZ ? Random.Range(0.0f, 180.0f) : 0);

                        GameObject decal = Instantiate(decalPrefab, decalPos, rot).gameObject;
                        decal.transform.parent = transform;
                        decal.transform.localScale = Vector3.one * decalSizeFactor;
                        decal.transform.GetChild(UnityEngine.Random.Range(0, decal.transform.childCount)).gameObject.SetActive(true);
                    }
                }
                curY = minY;
                switch (deployType)
                {
                    case DeployType.XY:
                        curX += (!randomX ? decalPrefab.size.x : decalPrefab.size.x * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                        break;
                    case DeployType.XZ:
                        curX += (!randomX ? decalPrefab.size.x : decalPrefab.size.x * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                        break;
                    case DeployType.YZ:
                        curX += (!randomX ? decalPrefab.size.y : decalPrefab.size.y * UnityEngine.Random.Range(0.5f, 1.5f)) * decalSizeFactor;
                        break;
                }
            }
        }
    }
    public void Clear()
    {
        foreach (var collider in transform.GetComponents<BoxCollider>())
        {
            DestroyImmediate(collider);
        }
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
public enum DeployType
{
    XY,
    XZ,
    YZ
}
public enum DrawMode
{
    Wire,
    Mesh,
    None
}