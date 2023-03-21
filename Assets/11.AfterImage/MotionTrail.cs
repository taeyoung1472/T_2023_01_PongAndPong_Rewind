using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trail ������ �����ϱ� ���� Struct
/// </summary>
[System.Serializable]
public class MeshTrailStruct
{
    public GameObject Container;

    public MeshFilter BodyMeshFilter;

    public Mesh bodyMesh;
}

public class MotionTrail : MonoBehaviour
{
    #region ����� �ʱ�ȭ
    [Header("[�ʿ��� �Žñ��]")]
    [SerializeField] private SkinnedMeshRenderer SMR_Body;

    private Transform TrailContainer;
    [SerializeField] private GameObject MeshTrailPrefab;
    private List<MeshTrailStruct> MeshTrailStructs = new List<MeshTrailStruct>();

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> posMemory = new List<Vector3>();
    private List<Quaternion> rotMemory = new List<Quaternion>();

    [Header("[Ʈ���� ����]")]
    [SerializeField] private int TrailCount;
    [SerializeField] private float TrailGap = 0.2f;
    [SerializeField][ColorUsage(true, true)] private Color frontColor;
    [SerializeField][ColorUsage(true, true)] private Color backColor;
    [SerializeField][ColorUsage(true, true)] private Color frontColor_Inner;
    [SerializeField][ColorUsage(true, true)] private Color backColor_Inner;

    public bool isDrawing = false;
    public bool isMotionTrail = false;

    [SerializeField] private Transform basePos = null;
    [SerializeField] private GameObject trailContainer;


    #endregion

    public void Init()
    {
        MakeAfterimage();
        TrailContainer.gameObject.SetActive(false);
        isDrawing = false;
        isMotionTrail = false;
        StopTrail();
    }

    public void MakeAfterimage()
    {
        // ���ο� TailContainer ���ӿ�����Ʈ�� ���� Trail ������Ʈ���� ����
        TrailContainer = trailContainer.transform;
        for (int i = 0; i < TrailCount; i++)
        {
            // ���ϴ� TrailCount��ŭ ����
            MeshTrailStruct pss = new MeshTrailStruct();
            pss.Container = Instantiate(MeshTrailPrefab, TrailContainer);
            pss.BodyMeshFilter = pss.Container.transform.GetChild(0).GetComponent<MeshFilter>();

            pss.bodyMesh = new Mesh();

            // �� mesh�� ���ϴ� skinnedMeshRenderer Bake
            SMR_Body.BakeMesh(pss.bodyMesh);

            // �� MeshFilter�� �˸��� Mesh �Ҵ�
            pss.BodyMeshFilter.mesh = pss.bodyMesh;

            MeshTrailStructs.Add(pss);

            bodyParts.Add(pss.Container);

            // Material �Ӽ� ����
            float alphaVal = (1f - (float)i / TrailCount) * 0.5f;
            pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", alphaVal);

            Color tmpColor = Color.Lerp(frontColor, backColor, (float)i / TrailCount);
            pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_FresnelColor", tmpColor);

            Color tmpColor_Inner = Color.Lerp(frontColor_Inner, backColor_Inner, (float)i / TrailCount);
            pss.BodyMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_BaselColor", tmpColor_Inner);
        }
        isDrawing = false;
        //StartCoroutine(BakeMeshCoroutine());
    }

    public void StartTrail()
    {
        if (isMotionTrail == false)
        {
            isMotionTrail = true;
            isDrawing = true;
            StartCoroutine(BakeMeshCoroutine());
            TrailContainer.gameObject.SetActive(true);
        }

    }
    public void StopTrail()
    {
        isMotionTrail = false;
        //isDrawing = false;
        //StartCoroutine(FadeMotionTrail());
        //StopCoroutine(BakeMeshCoroutine());
        //StopAllCoroutines();
        //DeleteMotion();
    }
    public void DeleteMotion()
    {
        for (int i = 0; i < TrailCount; i++)
        {
            MeshTrailStructs[i].bodyMesh.Clear();

            MeshTrailStructs[i].bodyMesh.Clear();
        }
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].transform.position = basePos.position;

            bodyParts[i].transform.rotation = basePos.rotation;
        }

        //TrailContainer.gameObject.SetActive(false);
    }
    IEnumerator FadeMotionTrail()
    {
        for (int i = 0; i < TrailCount; i++)
        {
            float a = MeshTrailStructs[i].BodyMeshFilter.GetComponent<MeshRenderer>().material.GetFloat("_Alpha");
            while (a >= 0f)
            {
                a -= 0.05f;
                MeshTrailStructs[i].BodyMeshFilter.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", a);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    /// <summary>
    /// Trail�� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator BakeMeshCoroutine()
    {
        // Mesh ��ü�� Swap�ϴ� ���� �ƴ϶� vertices, Triangles�� ����
        // ���� triangles�� �������� ������ �޽��� ����� ������� ����
        //TrailContainer.gameObject.SetActive(true);
        while (isDrawing)
        {
            for (int i = MeshTrailStructs.Count - 2; i >= 0; i--)
            {
                MeshTrailStructs[i + 1].bodyMesh.vertices = MeshTrailStructs[i].bodyMesh.vertices;

                MeshTrailStructs[i + 1].bodyMesh.triangles = MeshTrailStructs[i].bodyMesh.triangles;
            }

            // ù ��° ���� ���� Bake�������
            SMR_Body.BakeMesh(MeshTrailStructs[0].bodyMesh);


            // Snake ����ó�� ������ position�� rotation�� ���
            posMemory.Insert(0, basePos.position);
            rotMemory.Insert(0, basePos.rotation);

            // Trail Count�� �Ѿ�� ����
            if (posMemory.Count > TrailCount)
                posMemory.RemoveAt(posMemory.Count - 1);
            if (rotMemory.Count > TrailCount)
                rotMemory.RemoveAt(rotMemory.Count - 1);
            // ����ص� Pos, Rot �Ҵ�
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].transform.position = posMemory[Mathf.Min(i, posMemory.Count - 1)];
                bodyParts[i].transform.rotation = rotMemory[Mathf.Min(i, rotMemory.Count - 1)];
                
            }
            

            yield return new WaitForSeconds(TrailGap);
        }

        //StartCoroutine(BakeMeshCoroutine());
    }

}
