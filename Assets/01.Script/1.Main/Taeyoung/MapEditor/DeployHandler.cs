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
    [SerializeField] private MapElement prefab;
    [SerializeField] private bool centerX, centerY;
    [Header("[Decal 정보]")]
    [SerializeField] private MapElement decalPrefab;
    [SerializeField, Range(0.0f, 1.0f)] private float decalChance;
    [SerializeField] private bool centerX_decal, centerY_decal, rotX, rotY, rotZ;

    private Vector2 size;

    public void OnDrawGizmos()
    {
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

        size.x = x;
        size.y = y;

        switch (deployType)
        {
            case DeployType.XY:
                Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 0));
                break;
            case DeployType.XZ:
                Gizmos.DrawWireCube(center, new Vector3(size.x, 0, size.y));
                break;
            case DeployType.YZ:
                Gizmos.DrawWireCube(center, new Vector3(0, size.x, size.y));
                break;
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
                Handles.Label(new Vector3(center.x, center.y + (y / 2), transform.position.z), $"{size.x}M", labelStyle);
                Handles.Label(new Vector3(center.x + (x / 2), center.y, transform.position.z), $"{size.y}M", labelStyle);
                break;
            case DeployType.XZ:
                Handles.Label(center, $"{center.y}M", labelStyle);
                Handles.Label(new Vector3(center.x, transform.position.y, center.z + (y / 2)), $"{size.x}M", labelStyle);
                Handles.Label(new Vector3(center.x + (x / 2), transform.position.y, center.z), $"{size.y}M", labelStyle);
                break;
            case DeployType.YZ:
                Handles.Label(center, $"{center.x}M", labelStyle);
                Handles.Label(new Vector3(transform.position.x, center.y, center.z + (y / 2)), $"{size.x}M", labelStyle);
                Handles.Label(new Vector3(transform.position.x, center.y + (x / 2), center.z), $"{size.y}M", labelStyle);
                break;
        }
    }

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

        float curX = minX;
        float curY = minY;
        if(prefab != null)
        {
            while (curX < maxX)
            {
                while (curY < maxY)
                {
                    Vector3 pos = new();
                    switch (deployType)
                    {
                        case DeployType.XY:
                            pos = new Vector3(curX + (centerX ? prefab.size.x / 2 : 0), curY + (centerY ? prefab.size.y / 2 : 0), transform.position.z);
                            curY += prefab.size.y;
                            break;
                        case DeployType.XZ:
                            pos = new Vector3(curX + (centerX ? prefab.size.x / 2 : 0), transform.position.y, curY + (centerY ? prefab.size.z / 2 : 0));
                            curY += prefab.size.z;
                            break;
                        case DeployType.YZ:
                            pos = new Vector3(transform.position.x, curX + (centerX ? prefab.size.y / 2 : 0), curY + (centerY ? prefab.size.z / 2 : 0));
                            curY += prefab.size.z;
                            break;
                    }
                    GameObject obj = Instantiate(prefab, pos, Quaternion.identity).gameObject;
                    obj.transform.parent = transform;
                }
                curY = minY;
                switch (deployType)
                {
                    case DeployType.XY:
                        curX += prefab.size.x;
                        break;
                    case DeployType.XZ:
                        curX += prefab.size.x;
                        break;
                    case DeployType.YZ:
                        curX += prefab.size.y;
                        break;
                }
            }
        }
        if(decalPrefab != null)
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
                            decalPos = new Vector3(curX + (centerX_decal ? decalPrefab.size.x / 2 : 0), curY + (centerY_decal ? decalPrefab.size.y / 2 : 0), transform.position.z);
                            curY += decalPrefab.size.y * UnityEngine.Random.Range(0.5f, 1.5f);
                            break;
                        case DeployType.XZ:
                            decalPos = new Vector3(curX + (centerX_decal ? decalPrefab.size.x / 2 : 0), transform.position.y, curY + (centerY_decal ? decalPrefab.size.z / 2 : 0));
                            curY += decalPrefab.size.z * UnityEngine.Random.Range(0.5f, 1.5f);
                            break;
                        case DeployType.YZ:
                            decalPos = new Vector3(transform.position.x, curX + (centerX_decal ? decalPrefab.size.y / 2 : 0), curY + (centerY_decal ? decalPrefab.size.z / 2 : 0));
                            curY += decalPrefab.size.z * UnityEngine.Random.Range(0.5f, 1.5f);
                            break;
                    }

                    if (UnityEngine.Random.value < decalChance)
                    {
                        Quaternion rot = Quaternion.identity;
                        rot.eulerAngles = new Vector3(rotX ? Random.Range(0.0f, 180.0f) : 0, rotY ? Random.Range(0.0f, 180.0f) : 0, rotZ ? Random.Range(0.0f, 180.0f) : 0);

                        GameObject decal = Instantiate(decalPrefab, decalPos, rot).gameObject;
                        decal.transform.parent = transform;
                        decal.transform.GetChild(UnityEngine.Random.Range(0, decal.transform.childCount)).gameObject.SetActive(true);
                    }
                }
                curY = minY;
                switch (deployType)
                {
                    case DeployType.XY:
                        curX += decalPrefab.size.x * UnityEngine.Random.Range(0.5f, 1.5f);
                        break;
                    case DeployType.XZ:
                        curX += decalPrefab.size.x * UnityEngine.Random.Range(0.5f, 1.5f);
                        break;
                    case DeployType.YZ:
                        curX += decalPrefab.size.y * UnityEngine.Random.Range(0.5f, 1.5f);
                        break;
                }
            }
        }
    }
}
public enum DeployType
{
    XY,
    XZ,
    YZ
}