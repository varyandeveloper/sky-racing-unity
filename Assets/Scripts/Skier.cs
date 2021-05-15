using UnityEngine;
using PathCreation;
using System.Collections;
using PathCreation.Examples;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (PathFollower))]
public abstract class Skier : MonoBehaviour
{
    public float jumpForce = 10f;

    protected float jumpTime = 0.55f;

    protected PathFollower pathFollower;

    protected GameManager gameManager;

    protected Animator animator;

    protected float animatorActualSpeed;

    protected Rigidbody rb;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pathFollower = GetComponent<PathFollower>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animatorActualSpeed = animator.speed;
    }

    protected void Right()
    {
        if (pathFollower.pathCreator && pathFollower.pathCreator.name != "PathRighter") {
            StartCoroutine(MoveAfterTime(
                0.3f,
                "Right",
                gameManager.GetPath(getNextPath(true)))
            );
        }
    }

    protected void Left()
    {
        if (pathFollower.pathCreator && pathFollower.pathCreator.name != "PathLefter") {
            StartCoroutine(MoveAfterTime(
                0.3f,
                "Left",
                gameManager.GetPath(getNextPath(false)))
            );
        }
    }

    protected void Up()
    {
        if (pathFollower.pathCreator && isGrounded()) {
            StartCoroutine(EndAnimationAfterTime("Jump", 1f));
            StartCoroutine(pathFollower.TakePathForTime(0.75f));
            rb.velocity = transform.TransformDirection(new Vector3(0, jumpForce, 0));
        }
    }

    virtual protected bool isGrounded()
    {
        return pathFollower.pathCreator != null;
    }

    protected virtual void Dead()
    {
        animator.SetBool("Dead", true);
        pathFollower.speed = 0;
        gameManager.PlayerDead(gameObject);

    }

    protected void Down()
    {
        Ride();
    }

    virtual protected void Ride()
    {
        StartCoroutine(EndAnimationAfterTime("Start", 0.5f));
        pathFollower.speed += 0.25f; 
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Obstacle") {
            Dead();
        } else if (other.gameObject.GetComponent<Skier>()) {
            Debug.Log("Trigger to Skier");
        } else if (other.tag == "Stopper") {
            pathFollower.speed /= 1.3f;
            Destroy(other.gameObject);
        } else if (other.tag == "Suprize") {
            gameManager.ShowPoint(12);
            Destroy(other.gameObject);
            pathFollower.speed *= 2;
        } 
    }

    protected virtual IEnumerator MoveAfterTime(float time, string animationName, PathCreator path)
    {
        animator.speed *= 2;
        animator.SetBool(animationName, true);
        yield return new WaitForSeconds(time);
        animator.SetBool(animationName, false);
        pathFollower.UsePath(path);
        animator.speed = animatorActualSpeed;
    }

    protected IEnumerator EndAnimationAfterTime(string name, float time = 0.4f) {
        animator.speed *= 2;
        animator.SetBool(name, true);
        yield return new WaitForSeconds(0.4f);
        animator.speed = animatorActualSpeed;
        animator.SetBool(name, false);
    }

    protected string getNextPath(bool right)
    {
        string current = pathFollower.pathCreator.name;

        if (right) {
            if (current == "PathLefter") {
                return "PathLeft";
            }

            if (current == "PathLeft") {
                return "PathCenter";
            }

            if (current == "PathCenter") {
                return "PathRight";
            }

            return "PathRighter";
        } else {
            if (current == "PathRighter") {
                return "PathRight";
            }

            if (current == "PathRight") {
                return "PathCenter";
            }

            if (current == "PathCenter") {
                return "PathLeft";
            }

            return "PathLefter";
        }
    }
}
