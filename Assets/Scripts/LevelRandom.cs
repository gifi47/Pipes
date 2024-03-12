using System.Collections.Generic;
using UnityEngine;

public class LevelRandom : Level
{
    public int sizeX = 5;
    public int sizeY = 5;

    [SerializeField]
    private GameObject prefabBarrelStart;

    [SerializeField]
    private GameObject prefabValve;

    [SerializeField]
    private GameObject prefabBarrelEnd;

    [SerializeField]
    private GameObject prefabPipeS;

    [SerializeField]
    private GameObject prefabPipeC;

    [SerializeField]
    private GameObject prefabPipeT;

    [SerializeField]
    private GameObject prefabPipeCross;

    // Start is called before the first frame update
    void Start()
    {
        bool isFirstStart = true;

        List<LiquidTankStart> listStart = new List<LiquidTankStart>();
        List<LiquidTankEnd> listEnd = new List<LiquidTankEnd>();

        for (int i = -sizeX; i < sizeX; i++)
        {
            for (int j = -sizeY; j < sizeY; j++)
            {
                int item = Random.Range(0, 6);
                if (item == 0 || item == 1 || item == 6) item = Random.Range(0, 6);
                if ((item == 0 || item == 1 || item == 6) && (Random.Range(0, 2) == 0)) item = Random.Range(0, 6);

                if (item == 5 && i == -sizeX && i == (sizeX - 1)) item = Random.Range(0, 5);

                if (j == (sizeY - 1) && item == 1)
                {
                    item = Random.Range(0, 4);
                    if (item == 1) item = 4;
                }
                else if (j == -sizeY && item == 0) item = Random.Range(1, 5);

                GameObject instance;
                int liquidTypeRand;
                int maxRot;
                switch (item)
                {
                    case 0:
                        instance = Instantiate(prefabBarrelStart, gameObject.transform);
                        instance.transform.position = new Vector3(i, j + 0.16f, 0);
                        LiquidTankStart lt = instance.GetComponent<LiquidTankStart>();
                        liquidTypeRand = Random.Range(1, 5);
                        if (liquidTypeRand == 3)
                        {
                            liquidTypeRand = Random.Range(1, 8);
                        }
                        lt.liquidType = ((LiquidType)liquidTypeRand);
                        lt.StartLiquidType = ((LiquidType)liquidTypeRand);

                        listStart.Add(lt);
                        if (isFirstStart)
                        {
                            instance = Instantiate(prefabValve, gameObject.transform);
                            instance.transform.position = new Vector3(i, j - 0.75f, 0);
                            instance.transform.localScale = new Vector3(0.63395f, 0.63395f, 0.63395f);
                            instance.GetComponent<BoxCollider>().center = new Vector3(0, 0, -3);
                            isFirstStart = false;
                            gameStartTrigger = instance.GetComponent<Valve>();
                            if (shoudSetInteraction)
                            {
                                gameStartTrigger.OnInteract = actionOnInteraction;
                            }
                        }
                        
                        break;
                    case 1:
                        instance = Instantiate(prefabBarrelEnd, gameObject.transform);
                        LiquidTankEnd lte = instance.GetComponent<LiquidTankEnd>();
                        liquidTypeRand = Random.Range(1, 5);
                        if (liquidTypeRand == 3)
                        {
                            liquidTypeRand = Random.Range(1, 8);
                        }
                        lte.RequiredLiquidType = ((LiquidType)liquidTypeRand);

                        listEnd.Add(lte);
                        instance.transform.position = new Vector3(i - 0.17f, j - 0.157f, 0);
                        break;
                    case 2:
                        instance = Instantiate(prefabPipeS, gameObject.transform);
                        instance.transform.position = new Vector3(i, j, 0);
                        maxRot = Random.Range(0, 4);
                        instance.transform.Rotate(new Vector3(0, 0, 90 * maxRot));
                        break;
                    case 3:
                        instance = Instantiate(prefabPipeC, gameObject.transform);
                        instance.transform.position = new Vector3(i, j, 0);
                        maxRot = Random.Range(0, 4);
                        instance.transform.Rotate(new Vector3(0, 0, 90 * maxRot));
                        break;
                    case 4:
                        instance = Instantiate(prefabPipeT, gameObject.transform);
                        instance.transform.position = new Vector3(i, j, 0);
                        maxRot = Random.Range(0, 4);
                        instance.transform.Rotate(new Vector3(0, 0, 90 * maxRot));
                        break;
                    case 5:
                        instance = Instantiate(prefabPipeCross, gameObject.transform);
                        instance.transform.position = new Vector3(i, j, 0);
                        maxRot = Random.Range(0, 4);
                        instance.transform.Rotate(new Vector3(0, 0, 90 * maxRot));
                        break;
                }
            }
        }
        start = listStart.ToArray();
        end = listEnd.ToArray();

        if (levelBounds == null)
        {
            levelBounds = gameObject.AddComponent<LevelBounds>();
            levelBounds.topLeft = new Vector2(-(sizeX + 0.5f), (sizeY + 0.5f));
            levelBounds.bottomRight = new Vector2((sizeX + 0.5f), -(sizeY + 0.5f));
        }
    }

    bool shoudSetInteraction = false;
    System.Action actionOnInteraction;

    public override void SetInteraction(System.Action action)
    {
        if (gameStartTrigger != null)
        {
            gameStartTrigger.OnInteract += action;
        } else
        {
            actionOnInteraction = action;
            shoudSetInteraction = true;
        }
    }
}
