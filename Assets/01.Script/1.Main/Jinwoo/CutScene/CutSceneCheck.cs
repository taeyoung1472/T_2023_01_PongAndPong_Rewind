using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneCheck : MonoBehaviour
{
    [SerializeField] private TalkingCheck[] npc;
    public PlayableDirector playerCutScene;

    [SerializeField] private Collider cutSceneCheck;

    private void Start()
    {
        cutSceneCheck = GetComponent<Collider>();
        cutSceneCheck.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCutScene.Play();
            cutSceneCheck.enabled = false;
        }
    }
    public void CheckAllTalkNPC()
    {
        int successNpc = 0;
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i].GetIsTalk())
            {
                successNpc++;
            }
        }

        if (successNpc >= npc.Length) //��� NPC�� ��ȭ��
        {
            Debug.Log("�ƾ� �ݶ��̴�On" + successNpc);

            cutSceneCheck.enabled = true;
            
            UIGetter.Instance.PushUIs();
        }

    }
}
