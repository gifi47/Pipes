using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascotTitleScreen : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float delay = 0;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { animator = GetComponent<Animator>(); }
        delay = Random.Range(7.0f, 20.0f);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > delay)
        {
            time = 0;
            delay = Random.Range(27.0f, 49.0f);
            animator.Play("AppearFromBottom");
            StartCoroutine(Hide());
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        animator.Play("HideInTheBottom");
    } 
}

