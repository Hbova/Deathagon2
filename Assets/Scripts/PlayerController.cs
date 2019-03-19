using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform playerCamera;

    public Transform target;

    public Transform appearance;

    public float speed = 10;

    Vector3 lastSyncedPos;

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
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
        else
        {
            // interpolate from the renderer's current position to its ideal position
            appearance.position = Vector3.Lerp(appearance.position, target.position, speed * Time.deltaTime);

            // appearance.position = target.position; // for jerky but accurate movement
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            // don't send redundant data, like an unchanged position, over the network
            if (lastSyncedPos != target.position)
            {
                lastSyncedPos = target.position;

                // since there is new position data, serialize it to the data stream
                stream.SendNext(target.position);
            }
        }
        else
        {
            // receive data from the stream in *the same order* in which it was originally serialized
            target.position = (Vector3)stream.ReceiveNext();
        }
    }
}
