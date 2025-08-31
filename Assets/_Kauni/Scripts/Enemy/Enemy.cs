using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        animator.SetTrigger("Death");
    }

    public void Damage()
    {
        int hitReactionId = UnityEngine.Random.Range(0, 2);
        animator.SetInteger("HitReaction", hitReactionId);
        animator.SetTrigger("Damage");
    }
}
