using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
//using Cinemachine;
using UnityEngine;

public class CutSceneManager : MonoSingleTon<CutSceneManager>
{
    [SerializeField] private TalkingCheck[] npc;
    public PlayableDirector playerCutScene;

    [SerializeField] private List<MonoBehaviour> enableList;

    [SerializeField] private Player player;

    void Awake()
    {
        MainMenuManager.isOpend = true;
        FadeInOutManager.Instance.FadeOut(2f);
    }

    void Update()
    {

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

        if (successNpc >= npc.Length) //¸ðµç NPC¿Í ´ëÈ­ÇÔ
        {
            Debug.Log("ÄÆ¾À½ÃÀÛ!!" + successNpc);

            playerCutScene.Play();
        }
        else 
        {
           
        }
    }
    public void PlayerEnableList()
    {
        foreach (var item in enableList)
        {
            item.enabled = true;
        }
        
    }
    public void PlayerDisableList()
    {
        foreach (var item in enableList)
        {
            item.enabled = false;
        }
        foreach (var item in npc)
        {
            item.gameObject.SetActive(false);
        }
    }
    public void PlayerTurn()
    {
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    IEnumerator PlayerAloneCutScene()
    {
        yield return new WaitForSeconds(2f);
        playerCutScene.Play();
    }
}
