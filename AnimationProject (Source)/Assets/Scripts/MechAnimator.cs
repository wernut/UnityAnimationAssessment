/*=============================================================================
 * Game:        Animation Controller Assessment
 * Version:     1.0
 * 
 * Class:       MechAnimator.cs
 * Purpose:     To control the animation of the mech.
 * 
 * Author:      Lachlan Wernert
 *===========================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechAnimator : MonoBehaviour
{
    // References:
    public Animator animator;
    public Image[] buttonImages;
    public ParticleSystem deathEffect;
    public Transform hipTransform;
    public Slider walkSpeedSlider;

    // Private flags:
    private bool isWalking = false;
    private bool isJumping = false;
    private bool isDead = false;

    private void Start()
    {
        // Making the death particle effect a child of the mech's hip transform:
        deathEffect.transform.parent = hipTransform;
    }

    // Toggle walk function:
    public void ToggleWalk()
    {
        // Making the isWalking flag equal to it's opposite for toggling:
        isWalking = !isWalking;
        // Toggling the walk parameter in the animation controller:
        animator.SetBool("Walk", isWalking);
        // Setting the isDead flag to false, if true:
        if (isDead)
            isDead = false;
    }

    // Start jump sequence function:
    public void Jump()
    {
        // Setting the isDead flag to false, if true:
        if (isDead)
            isDead = false;

        // Starting to jumpSequence coroutine:
        StartCoroutine(jumpSequence());
    }

    // Jump sequence coroutine:
    IEnumerator jumpSequence()
    {
        // Setting the jump triggers:
        animator.SetTrigger("Jump");
        animator.SetBool("Grounded", false);
        isJumping = true;

        // Waiting 1.0f seconds before triggering the ground impact flags:
        yield return new WaitForSeconds(1.0f);

        // Setting the impact triggers and resetting the flag to 0:
        animator.SetTrigger("GroundImpact");
        animator.SetBool("Grounded", true);
        isJumping = false;
    }

    public void Die()
    {
        // Setting the nessecary flags to false:
        isWalking = false;
        animator.SetBool("Walk", false);

        // Playing the particle effect and setting the death flags:
        isDead = true;
        deathEffect.Play();
        animator.SetTrigger("Death");

        // Starting the death sequence coroutine:
        StartCoroutine(deathSequence());
    }

    IEnumerator deathSequence()
    {
        // Waiting 3 seconds before setting the isDead flag back to false:
        yield return new WaitForSeconds(3f);
        isDead = false;
    }

    private void Update()
    {
        // Setting the walkSpeed to the value of the UI slider:
        animator.SetFloat("WalkSpeed", walkSpeedSlider.value);

        // Bad hardcoded button colour changes:
        if (isWalking)
            buttonImages[0].color = Color.green;
        else
            buttonImages[0].color = Color.white;

        if (isJumping)
            buttonImages[1].color = Color.green;
        else
            buttonImages[1].color = Color.white;

        if (isDead)
            buttonImages[2].color = Color.green;
        else
            buttonImages[2].color = Color.white;
    }
}
