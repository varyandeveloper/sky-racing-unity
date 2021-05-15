using UnityEngine;
using System.Collections;

public class EnemyController : Skier
{
    float timer;

    int[] dividers = {2, 3, 5};

    private float actionTimer = 0;

    private Transform player;

    private bool isHitting = false;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();
        jumpTime = 0.65f;
    }

    protected override void Dead()
    {
        base.Dead();
        StartCoroutine(Respown(2));
    }

    private void Update()
    {
        CheckToRide();
        CheckPossibleCollision();
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.tag == "Avoid") {
            isHitting = true;
        }
        base.OnTriggerEnter(other);
    }

    private void CheckToRide()
    {
        timer += Time.deltaTime;
        if ((int)(timer % pathFollower.speed) == 0) {
            Ride();
            timer = 1f;
        }
    }

    private void CheckPossibleCollision()
    {
        actionTimer -= Time.deltaTime;
        if (pathFollower.pathCreator && isHitting && actionTimer < 0) {
            int action;
            isHitting = false;
    
            if (pathFollower.pathCreator.name == "PathRighter") {
                action = 1;
            } else if (pathFollower.pathCreator.name == "PathLefter") {
                action = 2;
            } else {
                action = Random.Range(1, 3);
            }

            DoRandomAction(action);
        }
    }

    protected override void Ride()
    {
        base.Ride();
        pathFollower.speed += Random.Range(0.1f, 0.5f);
    }

    protected override bool isGrounded()
    {
        return pathFollower.pathCreator == null;
    }

    private void DoRandomAction(int key)
    {
        Action(key); 
        actionTimer = 1;
    }

    private void Action(int key)
    {
        switch (key) {
            case 1:
                Left();
                break;
            case 2:
                Right();
                break;
            default:
                Up();
                break;
        }
    }

    protected IEnumerator Respown(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("Dead", false);
        string[] pathNames = {"PathCenter", "PathRight", "PathRighter", "PathLeft", "PathLefter"};
        transform.position = new Vector3(player.position.x, player.position.y, player.position.x - 5);
        pathFollower.pathCreator = gameManager.GetPath(pathNames[Random.Range(0, pathNames.Length)]);
        pathFollower.UseRandomSpeed();
        rb.isKinematic = false;
    }
}
