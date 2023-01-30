using CommandPatterns;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapDrawManager : MonoSingleTon<MapDrawManager>
{
    [SerializeField] private TotalGimmickSO totalGimmickSO;

    [SerializeField] private GimmickObjBtn gimmickImgPrefab;

    [SerializeField] private Transform gimmickImgParentTrm;

    private List<GimmickObjBtn> gimmickBtns = new();

    private bool canDeleteObj = false;

    private GameObject onMapObj;
    public GameObject OnMapObj { get { return onMapObj; } set { onMapObj = value; } }

    [SerializeField] private Button modeChangeBtn;
    [SerializeField] private TextMeshProUGUI modeTxt;

    private GameObject ghostObj;

    public Camera cam;

    private LayerMask rayWallLayer;
    private LayerMask platformLayer;

    public GameObject explainTab;




    void Awake()
    {
        CreateGimmickSprite();
        rayWallLayer = 1 << LayerMask.NameToLayer("RayWall");
        platformLayer = 1 << LayerMask.NameToLayer("Platform");
    }


    private void Update()
    {
        DrawGhostObject();
        SpawnBlock();
    }

    private Vector3 VirtualPos()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, rayWallLayer))
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                return hit.point;
            }

            if (Physics.Raycast(hit.point, Vector3.down, out hit, Mathf.Infinity, platformLayer))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }

    /// <summary>
    /// 처음에 기믹 so에 있는 스프라이트 리스트 카운트만큼 스크롤뷰에 생성
    /// </summary>
    private void CreateGimmickSprite()
    {
        for (int i = 0; i < totalGimmickSO.gimmickInfo.Count; i++)
        {
            GimmickObjBtn obj = Instantiate(gimmickImgPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(gimmickImgParentTrm);
            obj.gimmickInfo = totalGimmickSO.gimmickInfo[i];
            obj.Init();
            gimmickBtns.Add(obj);
            //나중에 텍스토도 있어야 할것같긴한데 일단은
        }
    }

    private void SpawnBlock()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !canDeleteObj)
        {
            if (onMapObj == null)
            {
                return;
            }
            Vector3 pos = Input.mousePosition;
            GameObject obj = Instantiate(onMapObj, VirtualPos(), Quaternion.identity);

        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && canDeleteObj == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log(hit.transform.gameObject.name);
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void DrawGhostObject()
    {
        if (ghostObj != null)
        {
            ghostObj.transform.position = VirtualPos();
        }
    }
    public void SetGhostObject(GameObject obj)
    {
        if (ghostObj)
        {
            Destroy(ghostObj.gameObject);
        }

        ghostObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
        ghostObj.GetComponent<TransformInfo>().enabled = false;
        ghostObj.transform.parent = transform;
        ghostObj.name = "GhostObject";

        ghostObj.layer = 1 << LayerMask.NameToLayer("Default");

        MeshRenderer ghostMats = ghostObj.GetComponent<MeshRenderer>();

        foreach (var mat in ghostMats.materials)
        {
            mat.color = new Color(0, 1, 0, 0.5f);
        }
    }

    public void PushDeleteMode()
    {
        canDeleteObj = !canDeleteObj;

        if (canDeleteObj)
        {
            modeTxt.text = "지우는 모드";
        }
        else
        {
            modeTxt.text = "배치 모드";
        }
    }
}

