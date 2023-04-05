using UnityEngine;

public class StageArea : MonoBehaviour
{
    [Header("�⺻ ������")]
    public int stagePlayTime;
    public bool isAreaClear = false;

    [Header("�ھ� ������")]
    [SerializeField] private Transform defaultPlayerSpawn;
    [SerializeField] private Transform rewindPlayerSpawn;

    public void InitArea()
    {
        TimerManager.Instance.InitTimer();
        TimerManager.Instance.SetRewindTime(stagePlayTime + 1);
        //���⼭ ��ȯ ���� �Žñ� ���ָ� �ɵ� (�ʱ�ȭ)
        TimerManager.Instance.ChangeOnTimer(true);
    }
    public void EntryArea(bool isNew = false)
    {
        //�Լ� ���� ���� �ſ� �߿�;
        InitArea();
        StageManager.Instance.SpawnPlayer(defaultPlayerSpawn, true);

        RewindManager.Instance.StartAreaPlay();
    }

    public void Rewind()
    {
        StageManager.Instance.SpawnPlayer(rewindPlayerSpawn, false);
    }

    public void ExitArea()
    {
        if (!isAreaClear)
        {
            StageManager.Instance.InitPlayer(isAreaClear);
            EntryArea();
        }
        else
        {
            StageManager.Instance.InitPlayer(isAreaClear);

        }
    }
}
