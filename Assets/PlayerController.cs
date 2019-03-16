using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform playerCamera;

    public Transform target;

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target.position = transform.position;
        RotateDestination(GetDestination());
        agent.destination = target.position;
        if (Input.GetMouseButton(1))
        {
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            playerCamera.eulerAngles -= new Vector3(mouseMovement.y * mouseSensitivityFactor, 0f, 0f);
            transform.eulerAngles += new Vector3(0f, mouseMovement.x * mouseSensitivityFactor, 0f);
        }
    }
    public Vector3 GetDestination()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal") * 10,0f, Input.GetAxis("Vertical") * 10);
        float run;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = 2f;
        }
        else run = 1;
        return direction * Time.deltaTime * 8 * run;
    }

    public void RotateDestination(Vector3 direction)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * direction;

        target.transform.position += rotatedTranslation;
    }
}
