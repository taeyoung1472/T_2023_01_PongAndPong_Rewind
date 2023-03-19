using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleTon<StageManager>
{
    public Dictionary<int, Stage> stageDictionary;
    public Stage stagePrefab;
    public Stage currentStage;

    [SerializeField] private PlayerRewind playerPrefab;
    [SerializeField] private GameObject rewindPlayerPrefab;

    private PlayerRewind playerObj;
    private GameObject rePlayerObj;

    public Image fadeImg;
    private void Awake()
    {
        Init();
        SpawnStage();
    }
    public void Init()
    {

    }

    public void SpawnStage()
    {
        currentStage = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity);
        currentStage.Init();
    }
    public void NextStage()
    {

    }

    public void SpawnPlayer(Transform spawnPos, bool isDefaultPlayer)
    {
        if (isDefaultPlayer)
            playerObj = Instantiate(playerPrefab, spawnPos.position, Quaternion.identity);
        else
            rePlayerObj = Instantiate(rewindPlayerPrefab, spawnPos.position, Quaternion.identity);
    }
    public void InitPlayer(bool isClear)
    {
        if (isClear)
        {
            playerObj.gameObject.SetActive(false);
            rePlayerObj.SetActive(false);
        }
        else
        {
            playerObj.gameObject.SetActive(false);
            rePlayerObj.SetActive(false);

        }
    }
}
