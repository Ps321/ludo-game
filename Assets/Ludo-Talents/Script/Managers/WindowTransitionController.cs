using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTransitionController : MonoBehaviour
{

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        animator.Play("ShowScreen");
    }

    public void HideScreen()
    {
        animator.Play("HideScreen");
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
