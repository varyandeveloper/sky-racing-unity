using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerController : Skier
{
    public Swipe toucControls;

    private Transform target;

    private Vector3 custom = Vector3.zero;

    private void Awake() {
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Transform>();
    }

    private void Update() {
        gameManager.ShowPoint(Time.deltaTime * pathFollower.speed, false);
        ListenToInput();
        target.position = custom != Vector3.zero ? custom : transform.position;

        if (custom != Vector3.zero) {
            custom.z += pathFollower.speed * Time.deltaTime;
        }
    }

    protected override void Dead()
    {
        base.Dead();
    }

    private void ListenToInput()
    {
        if (pathFollower.pathCreator) {
            if (toucControls.SwipeLeft) {
                StartCoroutine(UpdateCustom(0.6f, -1.6f));
                Left();
            }
            else if (toucControls.SwipeRight) {
                StartCoroutine(UpdateCustom(0.6f, 1.6f));
                Right();
            }
            else if (toucControls.SwipeUp){
                Up();
            }
            else if (toucControls.SwipeDown) {
                Down();
            }
        }
    }

    IEnumerator UpdateCustom(float time, float value) {
        custom = new Vector3(target.position.x + value, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(time);
        custom = Vector3.zero;
    }
}
