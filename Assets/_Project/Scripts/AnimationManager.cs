using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager _instance;
    public static AnimationManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void IdleAnimation(Animator animator)
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Run", false);
    }

    public void RunAnimation(Animator animator)
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Run", true);
    }

    public void DeathAnimation(Animator animator)
    {
        animator.SetBool("Run", false);
        animator.SetBool("Death", true);
    }

    public void WinAnimation(Animator animator)
    {
        animator.SetBool("Run", false);
        animator.SetBool("Win", true);
    }
}