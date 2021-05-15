using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smooth = 1.25f;

    public Transform target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 2f * Time.deltaTime);
        transform.LookAt(target);
    }
}
