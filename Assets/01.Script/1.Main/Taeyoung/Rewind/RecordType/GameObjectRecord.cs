using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRecord : TransformRecord
{
    List<bool> activeList;

    public override void ApplyData(int index, int nextIndexDiff)
    {
        base.ApplyData(index, nextIndexDiff);

        gameObject.SetActive(activeList[index]);
    }

    public override void Recorde(int index)
    {
        base.Recorde(index);

        activeList[index] = gameObject.activeSelf;
    }

    public override void Register()
    {
        base.Register();

        GenerateList<bool>(ref activeList, gameObject.activeSelf);
    }
}