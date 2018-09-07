using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalMoveSpeed, stopForce, gravForce;
    Transform transform;
    Rigidbody rb;
    CapsuleCollider coll;

    /// <summary>How much force to give on jumps</summary>
    public float jumpForce = 100;
    /// <summary>How large the normal Y must be (at least) to be able to jump</summary>
    public float minimumJumpNormalY;
    /// <summary>How large the normal must be (at least) to be able to jump</summary>
    public float minimumFlipNormal;

    Animator anim;
    /// <summary>The current collisions of the player</summary>
    List<Collision> currentCollisions;
    /// <summary> The next collisions of the player</summary>
    List<Collision> nextCollisions;
    /// <summary>Whether the player can currently jump</summary>
    bool canJump = true;

    bool canFlip = true;
    /// <summary> Whether the player was on the ground the last update </summary>
    bool wasOnGround, movementControlPrevented, turningAround, flipping, inout, onGround;
    int walkingDir;

    void Awake()
    {
        Physics.gravity = new Vector3(0, -gravForce, 0);
        //Debug.Log(transform.position);
    }

    // Use this for initialization
    void Start()
    {
        walkingDir = 1;
        turningAround = false;
        anim = GetComponentInChildren<Animator>();
        nextCollisions = new List<Collision>();
        currentCollisions = new List<Collision>();
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        coll = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        //Switch collision lists
        currentCollisions = nextCollisions;
        nextCollisions = new List<Collision>();

        if (rb.IsSleeping())
            rb.WakeUp();

        onGround = IsOnGround();

        var hMove = Input.GetAxis("Horizontal");
        if (movementControlPrevented)
            hMove = 0;
        
        //Debug.Log(rb.velocity.x);
        MoveCharacter(hMove);
        CheckInnerFlip(hMove);
        CheckOuterFlip(onGround);

        //Jumping
        if (canJump && !movementControlPrevented && Input.GetButton("Jump"))
        {
            if (onGround && wasOnGround)
            {
                Jump();
            }
        }

        //Flipping
        //if (Input.GetButtonDown("Flip") && !movementControlPrevented)
        //{
        //    TryFlip();
        //}

        wasOnGround = onGround;
    }

    void Update()
    {
        UpdateAnimator();
    }

    void Jump()
    {
        var up = Physics.gravity.normalized * -1;
        rb.velocity = new Vector3(up.x * jumpForce, up.y * jumpForce);
        //rb.AddForce(-jumpForce * Physics.gravity.x, -jumpForce * Physics.gravity.y, 0);
        canJump = false;
        StartCoroutine(ResetCanJump());
    }

    void MoveCharacter(float hMove)
    {
        var grav = Physics.gravity.normalized;
        var right = new Vector3(-grav.y, grav.x);

        float newX, newY;
        bool direction;
        if (grav.x == 0)
        {
            newX = right.x * horizontalMoveSpeed * hMove;
            //if (!Input.GetButtonDown("Jump") && IsOnGround())
            //    newY = 0;
            //else
                newY = rb.velocity.y;
            turningAround = newX != 0 && Math.Sign(hMove) != walkingDir;
        }
        else
        {
            //if (!Input.GetButtonDown("Jump") && IsOnGround())
            //    newX = 0;
            //else
                newX = rb.velocity.x;
            newY = right.y * horizontalMoveSpeed * hMove;
            turningAround = newY != 0 && Math.Sign(hMove) != walkingDir;
        }
        if (turningAround)
        {
            walkingDir *= -1;
        }
        
        rb.velocity = new Vector3(newX, newY);
    }

    void CheckInnerFlip(float hMove)
    {
        //Automatic flip if you walk against a wall
        if (Math.Abs(hMove) > 0.01f && canFlip && IsOnGround())
        {
            if (currentCollisions.Any(c => c.contacts.Any(cp => Physics.gravity.y == 0 ? Math.Abs(cp.normal.y) >= minimumFlipNormal
                : Math.Abs(cp.normal.x) >= minimumFlipNormal)))
            {
                if (hMove < 0)
                {
                    flipping = TryFlip(-1);
                }
                else
                {
                    flipping = TryFlip(1);
                }
                if (flipping)
                {
                    inout = false;
                }
            }
        }
    }

    void CheckOuterFlip(bool onGround)
    {
        if (!canFlip) return;

        var hasFlipVelocity = rb.velocity.x * Physics.gravity.x + rb.velocity.y * Physics.gravity.y >= 0;

        float playerHMove = -rb.velocity.x * Physics.gravity.y + rb.velocity.y * Physics.gravity.x;
        //if (!onGround && wasOnGround)
        //{
        //    //Debug.Log(playerHMove);
        //    Debug.Log(rb.velocity.x * Physics.gravity.x + rb.velocity.y * Physics.gravity.y);
        //}
        if (!onGround && wasOnGround && hasFlipVelocity)// && Math.Abs(playerHMove) >= 0)
        {
            var usedHMove = playerHMove;
            if (usedHMove == 0)
            {
                usedHMove = walkingDir;
            }
            flipping = TryFlip(usedHMove > 0 ? -1 : 1, true);
            if (flipping)
            {
                inout = true;
                StartCoroutine(ResetCanMove());
            }
        }
    }

    /// <summary>
    /// In a few frames, we can jump again.
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetCanJump()
    {
        for (var i = 0; i < 2; i++)
            yield return new WaitForFixedUpdate();
        canJump = true;
    }

    public IEnumerator ResetCanFlip()
    {
        canFlip = false;
        for (var i = 0; i < 3; i++)
            yield return new WaitForFixedUpdate();
        canFlip = true;
    }

    IEnumerator ResetCanMove()
    {
        movementControlPrevented = true;
        yield return new WaitForSeconds(0.4f);
        movementControlPrevented = false;
    }

    /// <summary>
    /// Try to perform a flip
    /// </summary>
    /// <param name="dir">1 if we need to change the gravity counter-clockwise, and -1 if we need to change it clockwise</param>
    /// <param name="isOuter">Whether this is an outer corner</param>
    /// <returns>Whether the flip has been performed</returns>
    bool TryFlip(int dir = 1, bool isOuter = false)
    {
        //Get the new gravity, based on the dir.
        var newGravity = dir == 1 ? new Vector3(-Physics.gravity.y, Physics.gravity.x, 0) :
            new Vector3(Physics.gravity.y, -Physics.gravity.x, 0);

        //The new position starts at the current position
        var newPosition = transform.position;

        //Move the object a little bit up/down (based on downgravity)
        newPosition += new Vector3(dir * (coll.bounds.size.x - coll.bounds.size.y) / 2 * Math.Sign(newGravity.y),
            dir * (coll.bounds.size.x - coll.bounds.size.y) / 2 * Math.Sign(newGravity.x), 0);
        
        if (isOuter)
        {
            var bottomBounds = Math.Min(coll.bounds.size.y, coll.bounds.size.x);
            //Move left/right (based on downgravity)
            newPosition += new Vector3((coll.bounds.extents.x - coll.bounds.extents.y) * Math.Sign(newGravity.x),
                (coll.bounds.extents.y - coll.bounds.extents.x) * Math.Sign(newGravity.y), 0);
            //Move up/down (based on downgravity)
            newPosition += new Vector3(dir * bottomBounds * Math.Sign(newGravity.y), dir * -bottomBounds * Math.Sign(newGravity.x));
        }
        else
            //Move right/left too (based on downgravity)
            newPosition += new Vector3((coll.bounds.extents.x - coll.bounds.extents.y) * Math.Sign(newGravity.x),
                -(coll.bounds.size.x - coll.bounds.size.y) / 2 * Math.Sign(newGravity.y), 0);

        //The extends to use when checking collisions, and the extends afterwards
        var newMegaExtends = new Vector3(coll.bounds.extents.y - 0.01f, coll.bounds.extents.x - 0.01f);
        var newExtends = new Vector3(coll.bounds.extents.y - 0.05f, coll.bounds.extents.x - 0.05f);

        //The collisions we have at our initial new position
        var collisionsAtNewLocation = Physics.OverlapBox(newPosition, newMegaExtends, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore).ToList();
        collisionsAtNewLocation.Remove(coll); //Remove collision with self from it

        //Move a bit left/right (/up/down) if needed, based on the collisions we get
        const float maxMove = 0.5f; //Maximum movement in this way
        var currMove = 0f;
        foreach (var col in collisionsAtNewLocation)
        {
            float thisMove = 0;
            if (newGravity.x > 0)
            {
                thisMove = newPosition.x + newMegaExtends.x - col.bounds.min.x;
            } else if (newGravity.x < 0)
            {
                thisMove = col.bounds.max.x - (newPosition.x - newMegaExtends.x);
            }
            else if (newGravity.y > 0)
            {
                thisMove = newPosition.y + newMegaExtends.y - col.bounds.min.y;
            }
            else if (newGravity.y < 0)
            {
                thisMove = col.bounds.max.y - (newPosition.y - newMegaExtends.y);
            }
            currMove = Math.Min(maxMove, Math.Max(0, Math.Max(currMove, thisMove)));
        }

        //The new position
        newPosition = new Vector3(newPosition.x - Math.Sign(newGravity.x) * currMove,
            newPosition.y - Math.Sign(newGravity.y) * currMove);
        
        //Try an overlap again, at the new position
        collisionsAtNewLocation = Physics.OverlapBox(newPosition, newExtends, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore).ToList();//Physics.OverlapCapsule(newPosition - addVector, newPosition + addVector,
        collisionsAtNewLocation.Remove(coll);

        //If there are no collisons left, flip!
        if (collisionsAtNewLocation.Count == 0) {
            //Flip now!
            Physics.gravity = newGravity;
            transform.position = newPosition;
            transform.Rotate(0, 0, dir * 90);
            rb.velocity = new Vector3(0, 0, 0);
            StartCoroutine(ResetCanFlip());
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// Whether the player is on the ground.
    /// </summary>
    /// <returns>Whether the player is currently on the ground.</returns>
    public bool IsOnGround()
    {
        var grav = Physics.gravity.normalized;
        if (Math.Abs(grav.x) < 0.01f)
        {
            return grav.y < 0
                ? currentCollisions.Any(c => c.contacts.Any(cp => cp.normal.y >= minimumJumpNormalY))
                : currentCollisions.Any(c => c.contacts.Any(cp => cp.normal.y <= -minimumJumpNormalY));
        }
        return grav.x < 0
            ? currentCollisions.Any(c => c.contacts.Any(cp => cp.normal.x >= minimumJumpNormalY))
            : currentCollisions.Any(c => c.contacts.Any(cp => cp.normal.x <= -minimumJumpNormalY));
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        nextCollisions.Add(collisionInfo);
    }

    void UpdateAnimator()
    {
        var grav = Physics.gravity.normalized;
        
        //Turning variables and scale
        transform.localScale = new Vector3(walkingDir, 1, 1);
        //anim.SetBool("TurningAround", turningAround);
        if (turningAround)
            anim.SetTrigger("TurningAround");
        turningAround = false;

        //Jumping variables
        anim.SetBool("OnGround", wasOnGround || onGround || flipping);

        //Movement variables
        float ver = grav.x == 0 ? rb.velocity.y : rb.velocity.x;
        
        anim.SetFloat("HorizontalVelocity", Math.Abs(Input.GetAxis("Horizontal")));
        anim.SetFloat("VerticalVelocity", Math.Abs(ver));

        //Flip variables
        if (flipping)
        {
            if (inout)
            {
                anim.SetTrigger("FlipDown");
                //anim.SetBool("OnGround", true);
                //Debug.Log("flipping down");
            }
            else
            {
                anim.SetTrigger("FlipUp");
                //Debug.Log("flipping up");
            }
            flipping = false;
        }
    }
}
 