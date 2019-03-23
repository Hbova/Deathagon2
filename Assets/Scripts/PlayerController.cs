using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Camera playerCamera;

    public Transform target;

    public Transform appearance;

    public Transform NavMeshTarget;

    public float speed = 10;

    Vector3 lastSyncedPos;

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //playerCamera = GetComponentInChildren<Camera>();
            //playerCamera.targetDisplay = NetworkedObjectsH.find.players.Count - 1;
        }
        else
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (photonView.IsMine)
        {
            NavMeshTarget.position = target.position;
            RotateDestination(GetDestination());
            agent.destination = NavMeshTarget.position;
            if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                playerCamera.transform.eulerAngles -= new Vector3(mouseMovement.y * mouseSensitivityFactor, 0f, 0f);
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
        if (Input.GetKey(KeyCode.LeftShift)) agent.speed = 7f;
        
        else agent.speed = 3.5f;
        return direction * Time.deltaTime * 8;
    }

    public void RotateDestination(Vector3 direction)
    {
        Vector3 rotatedTranslation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * direction;

        NavMeshTarget.transform.position += rotatedTranslation;
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
    [PunRPC]
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        agent.Warp(position);
    }
}
