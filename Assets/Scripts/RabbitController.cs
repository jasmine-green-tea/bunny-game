using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RabbitController : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 movementDirection;
    [SerializeField]
    private float directionUpdatePeriodSeconds;
    public bool pausedForInteraction = false;
    public float movementSpeed = 2.0f;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float animatorSpeed = 1f;

    public NeedSystem needSystem;
    public bool isEating = false;
    public bool isPlaying = false;

    public Rigidbody2D rb;

    private void Start()
    {
        StartCoroutine(DirectionPickerCoroutine());
        StartCoroutine(WaitToConsume());
    }

    private void Update()
    {
        animator.speed = animatorSpeed;
        //transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        rb.velocity = movementDirection * movementSpeed;
    }


    IEnumerator DirectionPickerCoroutine()
    {
        while (true)
        {
            while (pausedForInteraction)
                yield return null;


            int x = UnityEngine.Random.Range(-1, 2);
            int y = UnityEngine.Random.Range(-1, 2);

            if (x == 0 && y != 0)
                y = 0;

            bool isSad = needSystem.GetIsSad();



            if (isSad)
            {
                animator.Play("sad");
                x = 0;
                y = 0;
            }
            else
            {

                if ((x == 0) && (y == 0))
                    animator.Play("idle");
                else
                    animator.Play("walkRight");

                if (x < 0)
                    spriteRenderer.flipX = true;
                else if (x > 0)
                    spriteRenderer.flipX = false;
            }

            movementDirection = new Vector2(x, y);



            float multiplier = 1;
            if (movementDirection.magnitude == 0)
                multiplier = 4;
            if (isEating)
                multiplier = 1.5f;
            yield return new WaitForSeconds(directionUpdatePeriodSeconds * multiplier);
        }


        yield return null;
    }

    IEnumerator WaitToConsume()
    {
        while (true)
        {
            yield return new WaitUntil(() => (isEating || isPlaying));

            if (isEating)
                animator.Play("eat");
            //else if (isPlaying)
                //animator.Play("play")


            movementDirection = Vector2.zero;

            yield return new WaitForSeconds(directionUpdatePeriodSeconds * 2.5f);

            pausedForInteraction = false;
            isEating = false;
            isPlaying = false;

        }


        yield return null;
    }


}
