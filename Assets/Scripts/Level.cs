using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    protected LiquidTankStart[] start;

    [SerializeField]
    protected LiquidTankEnd[] end;

    [SerializeField]
    protected Valve gameStartTrigger;

    [SerializeField]
    protected LevelBounds levelBounds;

    private void Start()
    {
        if (levelBounds == null)
        {
            levelBounds = gameObject.AddComponent<LevelBounds>();
            levelBounds.topLeft = new Vector2(-5, 6);
            levelBounds.bottomRight = new Vector2(5, -6);
        }
    }

    public Queue<KeyValuePair<GameObject, int>> Compose()
    {
        Queue<KeyValuePair<GameObject, int>> que = new Queue<KeyValuePair<GameObject, int>>();
        for (int i = 0; i < start.Length; i++)
        {
            que.Enqueue(new KeyValuePair<GameObject, int>(start[i].gameObject, 0));
        }

        return que;
    }

    public int CheckLiquid()
    {
        int rezult = 0;
        for (int i = 0; i < end.Length ; i++)
        {
            if (end[i].liquidType == end[i].RequiredLiquidType) rezult++;
        }
        return (int)((rezult / ((float)end.Length)) * 3);
    }

    public virtual void SetInteraction(Action action)
    {
        gameStartTrigger.OnInteract += action;
    }

    public bool InBounds(Vector3 position)
    {
        return !(position.x < levelBounds.topLeft.x || position.x > levelBounds.bottomRight.x
            || position.y > levelBounds.topLeft.y || position.y < levelBounds.bottomRight.y);
    }
}

