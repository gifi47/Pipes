using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialScript : MonoBehaviour
{
    //[SerializeField]
    //private RectTransform circle;

    //[SerializeField]
    //private RectTransform arrow;

    [SerializeField]
    private Transform circle;

    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private TMPro.TMP_Text mascotTxt;

    State state = State.None;

    public float delay = 2f;

    private bool click = false;
    private bool clickOnPipe = false;
    private bool start = true;

    State nextState = State.None;
    float time = 0;

    PipeGameManager pipeGameManager;

    public void Init(PipeGameManager _pipeGameManager)
    {
        if (_pipeGameManager == null)
        {
            Destroy(circle.gameObject);
            Destroy(arrow.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            pipeGameManager = _pipeGameManager;
            state = State.Start;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Wait:
                if (time > delay || (click && !clickOnPipe && start))
                {
                    click = false;
                    time = 0;
                    if (nextState != State.None)
                    {
                        state = nextState;
                        nextState = State.None;
                    }
                } else
                {
                    time += Time.deltaTime;
                }
                break;

            case State.Start:
                circle.gameObject.SetActive(false);
                arrow.gameObject.SetActive(false);
                mascotTxt.text = "Помоги наполнить бочку водой!";
                pipeGameManager.cameraMovement = false;
                NextState(State.ShowEndBarrel);
                break;

            case State.ShowStartBarrel:
                //circle.anchoredPosition = new Vector3();
                mascotTxt.text = "Отсюда потечёт жидкость!";

                //circle.anchoredPosition = new Vector3(309, 162, 0);
                circle.transform.position = new Vector3(0f, 4f, 0f);
                circle.localScale = new Vector3(1f, 1f, 1f);

                //arrow.anchoredPosition = new Vector3(592, 162, 0);
                arrow.transform.position = new Vector3(0.89f, 3.31f, 0f);
                arrow.rotation = Quaternion.Euler(0, 0, -45f);
                //arrow.localScale = new Vector3(-2.25f, 2f, 1f);
                arrow.localScale = new Vector3(-1f, 1f, 1f);

                NextState(State.ShowPipes);
                break;

            case State.ShowAim:
                mascotTxt.text = "На бочке указан требуемый тип жидкости";

                //circle.anchoredPosition = new Vector3(309, 162, 0);
                circle.transform.position = new Vector3(-1.16f, -4.14f, 0f);
                circle.localScale = new Vector3(0.8f, 0.8f, 1f);

                //arrow.anchoredPosition = new Vector3(592, 162, 0);
                arrow.transform.position = new Vector3(0.24f, -4.18f, 0f);
                arrow.rotation = Quaternion.Euler(0, 0, 0f);
                //arrow.localScale = new Vector3(-2.25f, 2f, 1f);
                arrow.localScale = new Vector3(-1f, 1f, 1f);

                NextState(State.ShowStartBarrel);
                break;

            case State.ShowEndBarrel:
                mascotTxt.text = "Вот эту бочку нужно наполнить!";
                circle.gameObject.SetActive(true);
                arrow.gameObject.SetActive(true);

                //circle.anchoredPosition = new Vector3(309, 162, 0);
                circle.transform.position = new Vector3(-1.16f, -4.14f, 0f);
                circle.localScale = new Vector3(0.9f, 1.2f, 1f);

                //arrow.anchoredPosition = new Vector3(592, 162, 0);
                arrow.transform.position = new Vector3(0.45f, -4.16f, 0f);
                arrow.rotation = Quaternion.Euler(0, 0, 0f);
                //arrow.localScale = new Vector3(-2.25f, 2f, 1f);
                arrow.localScale = new Vector3(-1f, 1f, 1f);

                NextState(State.ShowAim);
                break;

            case State.ShowPipes:
                mascotTxt.text = "Нажми на трубу, чтобы повернуть её!";

                //circle.anchoredPosition = new Vector3(540, 1344, 0);
                circle.transform.position = new Vector3(0f, 2f, 0f);
                circle.localScale = new Vector3(1f, 1f, 1f);

                //arrow.anchoredPosition = new Vector3(706, 1289, 0);
                arrow.transform.position = new Vector3(1f, 1.65f, 0f);
                arrow.rotation = Quaternion.Euler(0, 0, -22.5f);
                //arrow.localScale = new Vector3(-2.25f, 2f, 1f);
                arrow.localScale = new Vector3(-1f, 1f, 1f);

                state = State.WaitForPipeClick;
                break;

            case State.ShowValve:
                mascotTxt.text = "Когда закончишь поворачивать трубы, нажми на вентиль, чтобы вода потекла!";

                //circle.anchoredPosition = new Vector3(309, 162, 0);
                circle.transform.position = new Vector3(-1.47f, 4.05f, 0f);
                circle.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                //arrow.anchoredPosition = new Vector3(592, 162, 0);
                arrow.transform.position = new Vector3(-1.89f, 2.95f, 0f);
                arrow.rotation = Quaternion.Euler(0, 0, -112.5f);
                //arrow.localScale = new Vector3(-2.25f, 2f, 1f);
                arrow.localScale = new Vector3(-1f, 1f, 1f);

                NextState(State.CameraZoom);
                break;

            case State.EndOfTutorial:
                StaticData.data.isTutorialComplete = true;
                StaticData.SaveData();
                Destroy(this.gameObject);
                break;

            case State.CameraZoom:
                start = false;
                mascotTxt.text = "Сводите и разводите два пальца, чтобы увеличивать и уменьшать масштаб";
                circle.gameObject.SetActive(false);
                arrow.gameObject.SetActive(false);
                pipeGameManager.cameraMovement = true;

                NextState(State.CameraMovement);
                break;

            case State.CameraMovement:
                mascotTxt.text = "Для передвежения камеры двигайте одним пальцем по экрану";

                NextState(State.EndOfTutorial);
                break;

            case State.WaitForPipeClick:
                if (clickOnPipe)
                {
                    clickOnPipe = false;
                    StartCoroutine(SetNewState(State.ShowValve, delay * 0.25f));
                }
                if (click) click = false;
                    
                break;
            case State.None:

                break;
        }

        if (click) click = false;
        if (clickOnPipe) clickOnPipe = false;
    }

    private enum State
    {
        None,
        Wait,
        Start,
        ShowEndBarrel,
        ShowStartBarrel,
        ShowAim,
        ShowPipes,
        ShowValve,
        EndOfTutorial,
        CameraZoom,
        CameraMovement,

        WaitForPipeClick
    }

    public void Click(bool OnPipe)
    {
        if (OnPipe)
        {
            clickOnPipe = true;
        }
        click = true;
    }

    IEnumerator SetNewState(State newState, float delay)
    {
        state = State.None;
        yield return new WaitForSeconds(delay);
        state = newState;
    }

    void NextState(State newState)
    {
        nextState = newState;
        state = State.Wait;
    }
}

