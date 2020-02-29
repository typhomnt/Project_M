using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    float posture_in = 0.0f;
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
        posture_in = System.Convert.ToSingle(Input.GetButton("Jump"));
        animator.SetFloat("InputC", posture_in);

        animator.SetLayerWeight(1, posture_in);
    }
}
