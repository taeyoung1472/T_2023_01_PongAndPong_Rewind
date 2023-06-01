using UnityEngine;

public class CircularBuffer<T>
{
    private T[] dataArray;
    private int bufferCurrentPosition = -1;
    private int bufferCapacity;
    private float howManyRecordsPerSecond; //프레임 단위 (만약 이게 50이면 1초에 50 프레임 기록한단거임)

    /// <summary>
    /// 시간 되감기를 위한 순환 버퍼 구조 사용
    /// </summary>
    public CircularBuffer()
    {
        
    }
    /// <summary>
    /// 버퍼 초기화
    /// </summary>
    public void InitBuffer()
    {
        howManyRecordsPerSecond = Time.timeScale / Time.fixedDeltaTime;
        bufferCapacity = (int)(RewindManager.Instance.howManySecondsToTrack * howManyRecordsPerSecond);
        dataArray = new T[bufferCapacity];
        //Debug.Log(dataArray.Length);
        RewindManager.Instance.RestoreBuffers += OnBuffersRestore;
    }
    public void InitBuffer(int bufferSize)
    {
        howManyRecordsPerSecond = Time.timeScale / Time.fixedDeltaTime;
        bufferCapacity = bufferSize;
        dataArray = new T[bufferCapacity];
        //Debug.Log(dataArray.Length);
        RewindManager.Instance.RestoreBuffers += OnBuffersRestore;
    }

    /// <summary>
    /// 버퍼의 마지막 위치에 값 쓰기
    /// </summary>
    /// <param name="val"></param>
    public void WriteLastValue(T val)
    {
        bufferCurrentPosition++;
        if (bufferCurrentPosition >= bufferCapacity)
        {
            bufferCurrentPosition = 0;
            dataArray[bufferCurrentPosition] = val;
        }
        else
        {
            dataArray[bufferCurrentPosition] = val;
        }
    }
    /// <summary>
    /// 버퍼에 기록된 마지막 값 읽기
    /// </summary>
    /// <returns></returns>
    public T ReadLastValue()
    {
        return dataArray[bufferCurrentPosition];
    }
    /// <summary>
    /// 순환 버퍼에서 지정된 값 읽기
    /// </summary>
    /// <param name="seconds">과거 몇 초를 읽어야 하는지 정의하는 변수
    /// (예: 초=5이면 추적된 개체가 정확히 5초 전에 있었던 값을 함수가 반환함)</param>
    /// <returns></returns>
    public T ReadFromBuffer(float seconds)
    {
        int howManyBeforeLast = (int)(howManyRecordsPerSecond * seconds); //현재 시간에서 기록될 수 있는 크기의 최대 (한마디로 최대 돌아갈 수 있는 크기)

        if ((bufferCurrentPosition - howManyBeforeLast) < 0)
        {
            int showingIndex = bufferCapacity - (howManyBeforeLast - bufferCurrentPosition);
            return dataArray[showingIndex];
        }
        else
        {
            return dataArray[bufferCurrentPosition - howManyBeforeLast];
        }
    }
    private void MoveLastBufferPosition(float seconds)
    {
        int howManyBeforeLast = (int)(howManyRecordsPerSecond * seconds);

        if ((bufferCurrentPosition - howManyBeforeLast) < 0)
        {
            bufferCurrentPosition = bufferCapacity - (howManyBeforeLast - bufferCurrentPosition);
        }
        else
        {
            bufferCurrentPosition -= howManyBeforeLast;
        }
    }
    private void OnBuffersRestore(float seconds)
    {
        MoveLastBufferPosition(seconds);
    }

}
