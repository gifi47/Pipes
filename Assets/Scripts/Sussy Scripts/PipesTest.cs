using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PipesTest : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float moveSpeed = 1f;

    [SerializeField]
    private GameObject start;

    [SerializeField]
    private GameObject[] starts;

    [SerializeField]
    private GameObject level;

    // Start is called before the first frame update
    void Start()
    {
        Compose();
    }

    bool zoom = false;
    Vector2 prevPos1 = Vector2.zero;
    Vector2 prevPos2 = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
#if EE
        switch (Input.touchCount)
        {
            case 1:
                zoom = false;
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        ClickOnScreen(touch.position);
                    }
                }
                break;
            case 2:
                Vector2 pos1 = Input.touches[0].position;
                Vector2 pos2 = Input.touches[1].position;
                if (zoom)
                {
                    float delta = Vector2.Distance(prevPos1, prevPos2) - Vector2.Distance(pos1, pos2);
                    Camera.main.orthographicSize += delta * Time.deltaTime * zoomSpeed;

                    Vector2 prevCenter = prevPos1 + ((prevPos2 - prevPos1) * 0.5f);
                    Vector2 center = pos1 + ((pos2 - pos1) * 0.5f);

                    Vector3 delta2 = prevCenter - center;
                    Camera.main.transform.position += delta2 * Time.deltaTime * moveSpeed;
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

    private static void ClickOnScreen(Vector2 clickPos)
    {

        Ray ray = Camera.main.ScreenPointToRay(clickPos);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Pipe"))
            {
                raycastHit.collider.GetComponent<Pipe>().Rotate();
                //raycastHit.transform.Rotate(new Vector3(0, 0, 90));
            }
            else if (raycastHit.collider.CompareTag("Interactable"))
            {
                raycastHit.collider.GetComponent<IInteractable>().Interact();
            }
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
    }

    public static Color GetColor(LiquidType liquidType)
    {
        switch (liquidType)
        {
            case LiquidType.Water:
                return Color.blue;
            case LiquidType.Oil:
                return Color.black;
            case LiquidType.Gas:
                return Color.yellow;
            case LiquidType.WaterOil:
                return new Color(0.05f, 0.1f, 0.5f);
            case LiquidType.WaterGas:
                return new Color(0.7f, 0.2f, 0.8f);
            case LiquidType.OilGas:
                return new Color(0.3f, 0.075f, 0f);
            case LiquidType.WaterOilGas:
                return new Color(0.3f, 0.1f, 0.3f);
            default:
                return Color.white;
        }
    }

    private LiquidTank[] levelPipes;

    public void Compose()
    {
        levelPipes = level.GetComponentsInChildren<LiquidTank>();
    }

    public void ClearWater()
    {
        foreach (var _pipe in levelPipes) 
        { 
            _pipe.liquidType = LiquidType.None;
            _pipe.Paint(Color.white, 0.01f);
        }
        start.GetComponent<LiquidTank>().liquidType = LiquidType.Water;
        start.GetComponent<LiquidTank>().Paint(Color.white, 0.5f);
        for (int i = 0; i < starts.Length; i++)
        {
            var a = starts[i].GetComponent<LiquidTank>();
            a.liquidType = ((LiquidType)(1 << ((i+1) % 3)));
            a.Paint(Color.white, 0.01f);
        }
    }

    public void StartGame()
    {
        Queue<KeyValuePair<GameObject, int>> que = new Queue<KeyValuePair<GameObject, int>>();
        que.Enqueue(new KeyValuePair<GameObject, int>(start, 0));
        foreach (GameObject _start in starts) que.Enqueue(new KeyValuePair<GameObject, int>(_start, 0));
        StartCoroutine(GameAlgorithm(que, 0.6f));
    }

    private void BFS(ref Queue<GameObject> que, ref HashSet<GameObject> pipes)
    {
        while (que.Count > 0)
        {

            GameObject pipe = que.Dequeue();

            LiquidTank liquidTankComponent = pipe.GetComponent<LiquidTank>();
            if (liquidTankComponent != null)
            {
                liquidTankComponent.Paint(Color.blue, 1f);
            }

            foreach (Collider2D collider in pipe.GetComponentsInChildren<Collider2D>())
            {
                Collider2D[] colliders = new Collider2D[5];
                int rezults = collider.OverlapCollider(new ContactFilter2D() { }, colliders);
                if (rezults > 0)
                {
                    GameObject childPipe = colliders[0].gameObject.transform.parent.gameObject;
                    if (!pipes.Contains(childPipe))
                    {
                        pipes.Add(childPipe);
                        que.Enqueue(childPipe);
                    }
                }
            }
        }
    }

    private IEnumerator BFSCoroutine(Queue<GameObject> que, HashSet<GameObject> pipes, float timeDelay)
    {
        while (que.Count > 0)
        {

            GameObject pipe = que.Dequeue();

            LiquidTank liquidTankComponent = pipe.GetComponent<LiquidTank>();
            if (liquidTankComponent != null)
            {
                liquidTankComponent.Paint(Color.blue, 1f);
            }

            foreach (Collider2D collider in pipe.GetComponentsInChildren<Collider2D>())
            {
                Collider2D[] colliders = new Collider2D[5];
                int rezults = collider.OverlapCollider(new ContactFilter2D() { }, colliders);
                if (rezults > 0)
                {
                    GameObject childPipe = colliders[0].gameObject.transform.parent.gameObject;
                    if (!pipes.Contains(childPipe))
                    {
                        pipes.Add(childPipe);
                        que.Enqueue(childPipe);
                    }
                }
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }

    public void Check()
    {
        Queue<GameObject> que = new Queue<GameObject>();
        HashSet<GameObject> pipes = new HashSet<GameObject>() { start };
        que.Enqueue(start);
        BFS(ref que, ref pipes);
    }

    public void CheckWhithAnimation()
    {
        Queue<GameObject> que = new Queue<GameObject>();
        HashSet<GameObject> pipes = new HashSet<GameObject>() { start };
        que.Enqueue(start);
        StartCoroutine(BFSCoroutine(que, pipes, 0.1f));
    }
}
