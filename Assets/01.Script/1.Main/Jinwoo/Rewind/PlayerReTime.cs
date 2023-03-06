using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReTime : ReTime
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private LinkedList<Sprite> spriteList;
    private LinkedList<bool> flipList;

    [SerializeField] private List<MonoBehaviour> enableList;

    [SerializeField] private CharacterController characterController;

    protected override void Start()
    {
        base.Start();
        
    }
    public override void Init()
    {
        base.Init();

        InitOnPlay();

        spriteList = new LinkedList<Sprite>();
        flipList = new LinkedList<bool>();

        //Debug.Log("������");

        spriteList.AddFirst(spriteRenderer.sprite);
        flipList.AddFirst(spriteRenderer.transform.localScale.x > 0 ? true : false);
    }
    public void InitOnPlay()
    {
        foreach (var item in enableList)
        {
            item.enabled = true;
        }

        characterController.enabled = true;
        animator.enabled = true;
    }

    public void InitOnRewind()
    {

        foreach (var item in enableList)
        {
            item.enabled = false;
        }

        characterController.enabled = false;
        animator.enabled = false;
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<ReTime>().StartTimeRewind();
            InitOnRewind();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<ReTime>().StopTimeRewind();
        }
    }
    protected override void Record()
    {
        base.Record();

        //Debug.Log("�ƾƾƾƾ�");
        if (spriteList.Count > Mathf.Round(RewindSeconds / Time.fixedDeltaTime))
        {
            spriteList.RemoveLast();
        }
        if (flipList.Count > Mathf.Round(RewindSeconds / Time.fixedDeltaTime))
        {
            flipList.RemoveLast();
        }
        spriteList.AddFirst(spriteRenderer.sprite);
        flipList.AddFirst(spriteRenderer.transform.localScale.x > 0 ? true : false);
    }

    protected override void Rewind()
    {
        

        base.Rewind();

        //����� ù��°�� �����ϰ� ù��°�� ������.
        if (spriteList.Count <= 0)
        {
            Debug.Log("dsfjsd");
            spriteList.Clear();
            flipList.Clear();
            InitOnPlay();
            return;
        }
        spriteRenderer.sprite = spriteList.First.Value;
        spriteRenderer.transform.localScale 
            = flipList.First.Value ? Vector3.one * 0.5f : new Vector3(-1, 1, 1) * 0.5f;
        spriteList.RemoveFirst();
        flipList.RemoveFirst();
    }
}
