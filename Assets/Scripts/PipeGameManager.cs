using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float moveSpeed = 1f;

    public float minimumZoom = 1.55f;
    public float maximumZoom = 13.35f;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private LevelMenuManager levelMenuManager;

    [SerializeField]
    private TutorialScript tutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (levelManager == null)
        {
            gameObject.GetComponent<LevelManager>();
        }
        levelManager.LoadedLevel.SetInteraction(StartGame);

        zoomSpeed = StaticData.settings.camZoomSpeed;
        moveSpeed = StaticData.settings.camMoveSpeed;

        if (!StaticData.data.isTutorialComplete && LevelManager.selectedLevel == 0)
        {
            tutorial.Init(this);
        } else
        {
            tutorial.Init(null);
        }

        if (LevelManager.selectedLevel == 5) 
        { 
            maximumZoom = 18f; 
        } else
        {
            maximumZoom = 13.35f;
        }
    }

    // ZOOM & TWO FINGER CAM MOVE
    bool zoom = false;
    Vector2 prevPos1 = Vector2.zero;
    Vector2 prevPos2 = Vector2.zero;

    // NEW INPUT 
    bool moved = false;
    bool camMove = false;
    Vector2 initialPos;

    public float errorBeforeMove = 2f;

    float holdTime = 0;

    public float holdTimeThresholdSF = 0.24f;
    public float holdTimeThreshold = 0.34f;
    public float tapTime = 0.24f;
    public float holdTimeDelay = 0.66f;

    float holdTimePrev = 0;

    public bool cameraMovement = true;
    public bool canInteract = true;

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        switch (Input.touchCount)
        {
            case 1:
                zoom = false;
                foreach (Touch touch in Input.touches)
                {
                    if (StaticData.settings.inputMode == SettingsStruct.InputMode.SingleFinger)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            camMove = false;
                            moved = true;
                            holdTimePrev = holdTimeThreshold;
                            holdTime = Time.deltaTime;
                            initialPos = touch.position;
                        }
                        else if (moved)
                        {
                            if (camMove)
                            {
                                Vector3 newCameraPos = Camera.main.transform.position +
                                    (new Vector3(-touch.deltaPosition.x, -touch.deltaPosition.y, 0)
                                    * Time.deltaTime * moveSpeed * Camera.main.orthographicSize * 0.2f);

                                if (cameraMovement)
                                {
                                    if (levelManager.LoadedLevel.InBounds(newCameraPos))
                                        Camera.main.transform.position = newCameraPos;
                                }
                            }
                            else
                            {
                                if (Vector2.Distance(touch.position, initialPos) 
                                    > errorBeforeMove * (holdTime < tapTime ? 3 * Mathf.Pow(tapTime / holdTime, 1.75f) : 1))
                                {
                                    camMove = true;
                                }
                                else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                                {
                                    if (holdTime > holdTimePrev)
                                    {
                                        holdTimePrev += holdTimeDelay;
                                        ClickOnScreen(touch.position);
                                    }
                                }
                                else
                                {
                                    if (holdTime < holdTimeThreshold)
                                    {
                                        ClickOnScreen(touch.position);
                                    }
                                    moved = false;
                                }
                                holdTime += Time.deltaTime;
                            }
                        }
                    } else
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            moved = true;
                            holdTime = Time.deltaTime;
                            initialPos = touch.position;
                        } 
                        else if (moved)
                        {
                            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                            {
                                if (holdTime > holdTimeThreshold)
                                {
                                    ClickOnScreen(initialPos);
                                    moved = false;
                                }
                            }
                            else
                            {
                                ClickOnScreen(initialPos);
                                moved = false;
                            }
                            holdTime += Time.deltaTime;
                        }
                    }
                }
                break;
            case 2:
                Vector2 pos1 = Input.touches[0].position;
                Vector2 pos2 = Input.touches[1].position;
                if (zoom)
                {
                    float currentSizeModifire = Camera.main.orthographicSize * 0.2f;

                    float delta = Vector2.Distance(prevPos1, prevPos2) - Vector2.Distance(pos1, pos2);
                    float zoomUpdate = delta * Time.deltaTime * zoomSpeed * currentSizeModifire;

                    if (cameraMovement)
                    {
                        if (zoomUpdate > 0 && Camera.main.orthographicSize < maximumZoom)
                        {
                            Camera.main.orthographicSize += zoomUpdate;
                        }
                        else if (zoomUpdate < 0 && Camera.main.orthographicSize > minimumZoom)
                        {
                            Camera.main.orthographicSize += zoomUpdate;
                        }
                    }

                    if (StaticData.settings.inputMode == SettingsStruct.InputMode.TwoFingers)
                    {
                        Vector2 prevCenter = prevPos1 + ((prevPos2 - prevPos1) * 0.5f);
                        Vector2 center = pos1 + ((pos2 - pos1) * 0.5f);

                        Vector3 delta2 = prevCenter - center;

                        if (cameraMovement)
                        {

                            Vector3 newCameraPos = Camera.main.transform.position + (delta2 * Time.deltaTime * moveSpeed * currentSizeModifire);

                            if (levelManager.LoadedLevel.InBounds(newCameraPos))
                                Camera.main.transform.position = newCameraPos;
                        }
                    }
                }
                else
                {
                    zoom = true;
                }
                prevPos1 = pos1;
                prevPos2 = pos2;

                break;
            default:
                zoom = false;
                break;
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            ClickOnScreen(Input.mousePosition);
        }
#endif
    }

    private void ClickOnScreen(Vector2 clickPos)
    {
        if (canInteract)
        {
            bool onPipe = false;
            Ray ray = Camera.main.ScreenPointToRay(clickPos);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Pipe"))
                {
                    raycastHit.collider.GetComponent<Pipe>().Rotate();
                    onPipe = true;
                    audioManager.PlayClickSound();
                    //raycastHit.transform.Rotate(new Vector3(0, 0, 90));
                }
                else if (raycastHit.collider.CompareTag("Interactable"))
                {
                    raycastHit.collider.GetComponent<IInteractable>().Interact();
                    audioManager.PlayValveSound();
                }
            }
            tutorial?.Click(onPipe);
        }
    }

    public IEnumerator GameAlgorithm(Queue<KeyValuePair<GameObject, int>> que, float timeDelay)
    {
        int depth = 0;
        while (que.Count > 0)
        {
            KeyValuePair<GameObject, int> next = que.Dequeue();
            if (depth < next.Value)
            {
                depth = next.Value;
                yield return new WaitForSeconds(timeDelay);
            }

            GameObject pipe = next.Key;

            LiquidTank liquidTankComponent = pipe.GetComponent<LiquidTank>();
            //if (liquidTankComponent == null) liquidTankComponent = pipe.GetComponent<LiquidTankEnd>();

            if (liquidTankComponent != null)
            {
                //liquidTankComponent.Paint(GetColor(liquidTankComponent.liquidType), 1f);
                liquidTankComponent.Paint(GetColor(liquidTankComponent.liquidType));
            }
            else continue;

            foreach (Collider2D collider in pipe.GetComponentsInChildren<Collider2D>())
            {
                Collider2D[] colliders = new Collider2D[5];
                int rezults = collider.OverlapCollider(new ContactFilter2D() { }, colliders);
                if (rezults > 0)
                {
                    GameObject childPipe = colliders[0].gameObject.transform.parent.gameObject;
                    LiquidTank childPipeLT = childPipe.GetComponent<LiquidTank>();
                    //if (childPipeLT == null) childPipeLT = childPipe.GetComponent<LiquidTankEnd>();
                    if (childPipeLT == null) continue;
                    if (childPipeLT.liquidType != liquidTankComponent.liquidType)
                    {
                        childPipeLT.liquidType = childPipeLT.liquidType | liquidTankComponent.liquidType;
                        childPipeLT.paintModes.Enqueue(colliders[0].gameObject.name);
                        que.Enqueue(new KeyValuePair<GameObject, int>(childPipe, next.Value + 1));
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
        Results();
    }

    public static Color GetColor(LiquidType liquidType)
    {
        switch (liquidType)
        {
            case LiquidType.Water:
                //return new Color(0.056f, 0.09f, 0.82f);
                return new Color(0.3619f, 0.5869f, 0.9565f);

            case LiquidType.Oil:
                return Color.black;

            case LiquidType.Gas:
                return Color.yellow;

            case LiquidType.WaterOil:
                return GetColor(LiquidType.Oil) * 0.5f + GetColor(LiquidType.Water) * 0.5f;

            case LiquidType.WaterGas:
                return GetColor(LiquidType.Gas) * 0.5f + GetColor(LiquidType.Water) * 0.5f;

            case LiquidType.OilGas:
                return GetColor(LiquidType.Oil) * 0.5f + GetColor(LiquidType.Gas) * 0.5f;

            case LiquidType.WaterOilGas:
                return GetColor(LiquidType.Oil) * 0.33f + GetColor(LiquidType.Water) * 0.33f + GetColor(LiquidType.Gas) * 0.33f;

            default: 
                return new Color(0, 0, 0, 0);
        }
    }

    public void Results()
    {
        audioManager.StopWaterFlow();
        int rezult = levelManager.LoadedLevel.CheckLiquid();
        if (rezult > 0)
        {
            audioManager.PlayWinSound();
        } else
        {
            audioManager.PlayLoseSound();
        }
        StaticData.CompleteLevel(LevelManager.selectedLevel, rezult);
        levelMenuManager.LevelCompleted(rezult);
    }

    public void StartGame()
    {
        audioManager.PlayWaterFlow();
        canInteract = false;
        Queue<KeyValuePair<GameObject, int>> que = levelManager.LoadedLevel.Compose();
        StartCoroutine(GameAlgorithm(que, 0.6f));
    }

    public void Randomize()
    {
        LiquidTank[] pipes = levelManager.LoadedLevel.GetComponentsInChildren<LiquidTank>();
        foreach (LiquidTank pipe in pipes)
        {
            BoxCollider boxCollider = pipe.GetComponent<BoxCollider>();
            if (boxCollider != null && boxCollider.enabled == true)
            {
                int max_Rot = Random.Range(0, 4);
                for (int i = 0; i < max_Rot; i++)
                {
                    pipe.gameObject.GetComponent<Pipe>().Rotate();
                }
            }
        }
    }
}
