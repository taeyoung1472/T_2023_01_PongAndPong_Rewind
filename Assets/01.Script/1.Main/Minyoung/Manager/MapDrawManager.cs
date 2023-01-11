using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapDrawManager : MonoSingleTon<MapDrawManager>
{
    [SerializeField] private GimmickSpriteSO gimmickSO;

    [SerializeField] private GameObject gimmickImgPrefab;

    [SerializeField] private Transform gimmickImgParentTrm;

    public List<GameObject> gimmickBtns;

    private Sprite currentSelectSprite;
    public Sprite CurrentSelectSprite { get { return currentSelectSprite; } set { currentSelectSprite = value; } }


    // private GameObject onMapObj;
    public GameObject OnMapObj;// { get { return onMapObj; } set { onMapObj = value; } }

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
        //transform.GetComponent<SpriteRenderer>().sprite = currentSelectSprite;

        if (Input.GetMouseButtonDown(0))
        {
            if (OnMapObj == null)
            {
                return;
            }

            Vector3 pos = Input.mousePosition;
            pos.z = 15; //ī�޶� z���� -15�⋚���� 15 + (-15)
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
            Debug.Log(targetPos);
            GameObject obj = Instantiate(OnMapObj, targetPos, Quaternion.identity);
            //obj.transform.position = new Vector3(targetPos.x, targetPos.y , 0);
            {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit hit;

                //if (Physics.Raycast(ray, out hit, 100f))
                //{
                //    Vector3 hitPos = hit.point;
                //    GameObject obj = Instantiate(OnMapObj);
                //    obj.transform.position = hitPos;
                //}
            }
        }
    }
}
