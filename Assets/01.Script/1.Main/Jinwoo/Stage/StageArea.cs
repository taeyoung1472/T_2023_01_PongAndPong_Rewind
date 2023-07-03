using EPOOutline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class StageArea : MonoBehaviour
{
    [Header("�⺻ ������")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("�ھ� ������")]
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;
    [SerializeField] private Transform endPoint;
    public Transform freeLookCamPos;

    public GameObject gameObjesddct;

    private bool isAreaPlay = false;
    public bool IsAreaPlay { get => isAreaPlay; set => isAreaPlay = value; }

    public UnityEvent areaEndEvent;

    public Action OnEntryArea;
    public Action OnExitArea;

    public Vector3 cmaInitPos;

    private FreeLookCamera freeLookCamera;

    private List<ParticleSystem> fogs = null;
    private List<GimmickVisualLink> linkPaths = null;
    private List<Outlinable> outlines = null;
    private List<Material> collectionMaterials = null;

    private void Start()
    {
        freeLookCamera = FindObjectOfType<FreeLookCamera>();
        isAreaPlay = false;

        defaultPlayerSpawn.gameObject.SetActive(false);
        rewindPlayerSpawn.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        collectionMaterials = new List<Material>();
        fogs = GetComponentsInChildren<ParticleSystem>().ToList().FindAll(x => x.name.Split(" ")[0] == "Fog");
        GetComponentsInChildren<Collection>()
            .ToList().ForEach(x => collectionMaterials.Add(x.GetComponent<MeshRenderer>().material));
        linkPaths = GetComponentsInChildren<GimmickVisualLink>().ToList();
        outlines = GetComponentsInChildren<Outlinable>().ToList();
    }
    public void FogOfAreaSetting(bool curArea)
    {
        StartCoroutine(FogCoroutine(curArea));
    }
    private IEnumerator FogCoroutine(bool curArea)
    {
        yield return null;
        if (collectionMaterials == null)
        {
            collectionMaterials = new List<Material>();
            fogs = GetComponentsInChildren<ParticleSystem>().ToList().FindAll(x => x.name.Split(" ")[0] == "Fog");
            GetComponentsInChildren<Collection>()
                .ToList().ForEach(x => collectionMaterials.Add(x.GetComponent<MeshRenderer>().material));
            linkPaths = GetComponentsInChildren<GimmickVisualLink>().ToList();
            outlines = GetComponentsInChildren<Outlinable>().ToList();
        }
        for (int i = 0; i < outlines.Count; i++)
            outlines[i].enabled = curArea;
        for (int i = 0; i < linkPaths.Count; i++)
            linkPaths[i].gameObject.SetActive(curArea);

        if (curArea)
        {
            for (int i = 0; i < fogs.Count; i++)
                fogs[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            for (int i = 0; i < collectionMaterials.Count; i++)
            {
                collectionMaterials[i].SetFloat("_BottomLine", 0.81f);
                collectionMaterials[i].SetFloat("_TopLine", 0.91f);
            }
        }
        else
        {
            for (int i = 0; i < fogs.Count; i++)
                fogs[i].Play();
            for (int i = 0; i < collectionMaterials.Count; i++)
            {
                collectionMaterials[i].SetFloat("_BottomLine", 0f);
                collectionMaterials[i].SetFloat("_TopLine", 0.577f);
            }
        }
    }
    public void InitArea()
    {
        TimerManager.Instance.InitTimer();
        TimerManager.Instance.SetRewindTime(stagePlayTime + 1);
        //���⼭ ��ȯ ���� �Žñ� ���ָ� �ɵ� (�ʱ�ȭ)
        TimerManager.Instance.ChangeOnTimer(true);
    }
    public void EntryArea(bool isGameStart = false)
    {
        defaultPlayerSpawn.gameObject.SetActive(true);
        rewindPlayerSpawn.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);


        //�Լ� ���� ���� �ſ� �߿�;
        IsAreaPlay = true;

        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);


        //Debug.Log("�Ƹ��ƿ�Ʈ��");
        if (isGameStart)
            RewindManager.Instance.StartAreaPlay();

        OnEntryArea?.Invoke();
        if (freeLookCamera == null)
        {
            freeLookCamera = FindObjectOfType<FreeLookCamera>();
        }
        freeLookCamera.initPos = cmaInitPos;
    }

    public void Rewind()
    {
        StageManager.Instance.InitTransform();
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);
    }

    public void ExitArea()
    {
        OnExitArea?.Invoke();

        defaultPlayerSpawn.gameObject.SetActive(false);
        rewindPlayerSpawn.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        IsAreaPlay = false;
        if (!isAreaClear) //Ŭ���� ����
        {
            StageManager.Instance.InitPlayer(isAreaClear); //false
            EntryArea(true);
        }
        else //Ŭ���� ��
        {
            StageManager.Instance.InitPlayer(isAreaClear); //true
            areaEndEvent?.Invoke();
        }
    }
}
