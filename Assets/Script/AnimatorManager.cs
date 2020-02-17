using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("InputY", Input.GetAxis("Vertical"));
        animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
        animator.SetFloat("InputC", System.Convert.ToSingle(Input.GetButton("Jump")));
    }
}
