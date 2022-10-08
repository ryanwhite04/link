using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    // private Animator animator;

    [SerializeField] private LayerMask ground;

    private float dirX = 0f;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float thrust = 14f;

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        // animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            body.velocity = new Vector2(body.velocity.x, thrust);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (body.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (body.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        // animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        print("Checking if grounded");
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, ground);
    }
}
