using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public enum BunnyColor
{
    Orange = 0,
    Gray = 1,
    MaxColors
}

public class RabbitController : MonoBehaviour
{
    // Start is called before the first frame update

    private enum RabbitAnimationState
    {
        Idle,
        Moving,
        Eating,
        Playing,
        Sad
    }


    public Vector2 movementDirection;
    [SerializeField]
    private float directionUpdatePeriodSeconds;
    public bool pausedForInteraction = false;
    public float movementSpeed = 2.0f;

    public Animator animator;
    public RuntimeAnimatorController[] colorControllers;
    public SpriteRenderer spriteRenderer;
    public float animatorSpeed = 1f;

    public NeedSystem needSystem;
    public bool isEating = false;
    public bool isPlaying = false;
    public bool isSad = false;

    public Rigidbody2D rb;

    private RabbitAnimationState currentState = RabbitAnimationState.Idle;

    private float animationEndTime = 0f;
    private bool isAnimationForced = false;

    public UnityEvent forcedAnimationEvent;
    public UnityEvent releaseAnimationEvent;


    private void Start()
    {
        StartCoroutine(DirectionPickerCoroutine());
    }

    private void Update()
    {
        animator.speed = animatorSpeed;
        //transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        rb.velocity = movementDirection * movementSpeed;

        isSad = needSystem.GetIsSad();

        UpdateAnimationState();
        UpdateFlipOrientation();
    }

    public void SetBunnyColor(int colorIndex)
    {
        animator.runtimeAnimatorController = colorControllers[colorIndex];
    }

    private void UpdateFlipOrientation()
    {

        if (rb.velocity.x < 0 && movementDirection.magnitude > 0)
            spriteRenderer.flipX = true;
        if (rb.velocity.x > 0 && movementDirection.magnitude > 0)
            spriteRenderer.flipX = false;

    }
    private void UpdateAnimationState()
    {
        if (isAnimationForced && Time.time < animationEndTime)
            return;
        else if (isAnimationForced && Time.time >= animationEndTime)
        {
            releaseAnimationEvent.Invoke();
            isAnimationForced = false;
            isEating = false;
            isPlaying = false;
        }


            RabbitAnimationState newState = DetermineAnimationState();

        if (newState != currentState)
        {
            currentState = newState;
            PlayStateAnimation(newState);
        }
    }

    private RabbitAnimationState DetermineAnimationState()
    {
        // Define clear priorities (highest to lowest)
        if (isEating) return RabbitAnimationState.Eating;
        if (isPlaying) return RabbitAnimationState.Playing;
        if (isSad) return RabbitAnimationState.Sad;
        if (movementDirection != Vector2.zero) return RabbitAnimationState.Moving;
        return RabbitAnimationState.Idle;
    }

    private void PlayStateAnimation(RabbitAnimationState state)
    {
        switch (state)
        {
            case RabbitAnimationState.Sad:
                animator.Play("sad");
                movementDirection = Vector2.zero;
                break;
            case RabbitAnimationState.Eating:
                animator.Play("eat");
                movementDirection = Vector2.zero;
                StartForcedAnimation(3f);
                break;
            case RabbitAnimationState.Playing:
                // animator.Play("play");
                movementDirection = Vector2.zero;
                StartForcedAnimation(3f);
                break;
            case RabbitAnimationState.Moving:
                animator.Play("walkRight");
                // Handle sprite flipping
                //if (movementDirection.x < -0.1f) spriteRenderer.flipX = true;
                //else if (movementDirection.x > 0.1f) spriteRenderer.flipX = false;
                break;
            case RabbitAnimationState.Idle:
                animator.Play("idle");
                break;
        }
    }

    private void StartForcedAnimation(float duration)
    {

        forcedAnimationEvent.Invoke();

        isAnimationForced = true;
        animationEndTime = Time.time + duration;
    }

    IEnumerator DirectionPickerCoroutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => ShouldPauseMovement());

            // Only pick direction if we're allowed to move
            if (!ShouldPauseMovement())
            {
                int x = UnityEngine.Random.Range(-1, 2);
                int y = UnityEngine.Random.Range(-1, 2);

                if (x == 0)
                    y = 0;
                movementDirection = new Vector2(x, y);
            }

            float multiplier = (movementDirection.magnitude == 0) ? 2 : 1;
            yield return new WaitForSeconds(directionUpdatePeriodSeconds * multiplier);
        }
    }

    private bool ShouldPauseMovement()
    {
        return isSad || isEating || isPlaying || pausedForInteraction || isAnimationForced;
    }


}
