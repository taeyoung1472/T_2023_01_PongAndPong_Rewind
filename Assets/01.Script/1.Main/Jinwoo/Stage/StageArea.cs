using UnityEngine;

public class StageArea : MonoBehaviour
{
    [Header("�⺻ ������")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("�ھ� ������")]
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;

    public GameObject gameObjesddct;

    public void InitArea()
    {
        TimerManager.Instance.InitTimer();
        TimerManager.Instance.SetRewindTime(stagePlayTime + 1);
        //���⼭ ��ȯ ���� �Žñ� ���ָ� �ɵ� (�ʱ�ȭ)
        TimerManager.Instance.ChangeOnTimer(true);
    }
    public void EntryArea(bool isGameStart = false)
    {
        //�Լ� ���� ���� �ſ� �߿�;
        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);

        Debug.Log("�Ƹ��ƿ�Ʈ��");
        if(isGameStart)
            RewindManager.Instance.StartAreaPlay();
    }

    public void Rewind()
    {
        StageManager.Instance.InitTransform();
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);
    }

    public void ExitArea()
    {
        if (!isAreaClear)
        {
            StageManager.Instance.InitPlayer(isAreaClear);
            EntryArea(true);
        }
        else
        {
            StageManager.Instance.InitPlayer(isAreaClear);

        }
    }
}
