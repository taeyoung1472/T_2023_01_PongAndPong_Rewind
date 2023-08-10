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
            cutSceneCheck.enabled = false;
            StartCoroutine(PlayElevatorCutScene());
        }
    }
    private IEnumerator PlayElevatorCutScene()
    {
        FadeInOutManager.Instance.FadeIn(0.25f);
        yield return new WaitForSeconds(0.5f);
        FadeInOutManager.Instance.FadeOut(0.25f);
        UIGetter.Instance.PushUIs();
        playerCutScene.Play();
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

        if (successNpc >= npc.Length) //모든 NPC와 대화함
        {
            Debug.Log("컷씬 콜라이더On" + successNpc);

            cutSceneCheck.enabled = true;
            
            UIGetter.Instance.PushUIs();
        }

    }
}
