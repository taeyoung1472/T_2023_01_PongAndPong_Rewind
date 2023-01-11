using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MapDrawManager : MonoSingleTon<MapDrawManager>
{
    [SerializeField] private GimmickSpriteSO gimmickSO;

    [SerializeField] private GameObject gimmickImgPrefab;

    [SerializeField] private Transform gimmickImgParentTrm;

    public List<GameObject> gimmickBtns;

    private bool canDeleteObj = false;

    private GameObject onMapObj;
    public GameObject OnMapObj { get { return onMapObj; } set { onMapObj = value; } }

    [SerializeField] private Button modeChangeBtn;
    [SerializeField] private TextMeshProUGUI modeTxt;
    
    void Awake()
    {
        CreateGimmickSprite();
    }
    /// <summary>
    /// ó���� ��� so�� �ִ� ��������Ʈ ����Ʈ ī��Ʈ��ŭ ��ũ�Ѻ信 ����
    /// </summary>
    private void CreateGimmickSprite()
    {
        for (int i = 0; i < gimmickSO.gimmickSprites.Count; i++)
        {
            GameObject obj = Instantiate(gimmickImgPrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(gimmickImgParentTrm);
            obj.GetComponent<Image>().sprite = gimmickSO.gimmickSprites[i];
            obj.GetComponent<GimmickObjBtn>().i = i;
            gimmickBtns.Add(obj);
            //���߿� �ؽ��䵵 �־�� �ҰͰ����ѵ� �ϴ���
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !canDeleteObj)
        {
            if (onMapObj == null)
            {
                return;
            }
            Vector3 pos = Input.mousePosition;
            pos.z = 15; //ī�޶� z���� -15�⋚���� 15 + (-15)
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
            Debug.Log(targetPos);
            GameObject obj = Instantiate(onMapObj, targetPos, Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && canDeleteObj == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log(ray);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log(hit.transform.gameObject.name);
                Destroy(hit.transform.gameObject);
            }
        }   
    }

    public void PushDeleteMode()
    {
        canDeleteObj = !canDeleteObj;

        if (canDeleteObj)
        {
            modeTxt.text = "����� ���";
        }
        else
        {
            modeTxt.text = "��ġ ���";
        }
    }
}

