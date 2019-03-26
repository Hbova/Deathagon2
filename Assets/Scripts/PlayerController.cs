using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public Text playerNumberText;
    public Text incomeText;
    public Text walletText;
    public Text waveTimer;

    public Button LevelOne;
    
    public UnityEngine.AI.NavMeshAgent agent;

    public Camera playerCamera;

    public Transform target;

    public Transform appearance;

    public Transform NavMeshTarget;

    public float speed = 10;

    Vector3 lastSyncedPos;

    public int playerNumber;

    public GameObject Bullet;
    private GameObject bullet;
    

    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<PlayerProperties>().currentIncome = 200;
            transform.GetComponent<PlayerProperties>().currentWallet = 200;
            playerNumberText.text = "Player: " + playerNumber + 1;
            UnitSpawner.find.player = transform;
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsOne);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsTwo);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsThree);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsFour);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsFive);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsSix);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsSeven);
            UnitSpawner.find.arenaSpawns.Add(UnitSpawner.find.unitSpawnsEight);
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
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
            if (Input.GetMouseButtonDown(0))
            {
                bullet = Instantiate(Bullet);
                bullet.transform.GetComponent<Bullet>().bulletDamage = GetComponent<PlayerProperties>().playerBulletDamage;
                bullet.transform.position = transform.position + Vector3.up;
                bullet.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x - 3, transform.eulerAngles.y, 0f);
            }
            if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                playerCamera.transform.eulerAngles -= new Vector3(mouseMovement.y * mouseSensitivityFactor, 0f, 0f);
                transform.eulerAngles += new Vector3(0f, mouseMovement.x * mouseSensitivityFactor, 0f);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (LevelOne.gameObject.activeInHierarchy == false) LevelOne.gameObject.SetActive(true);
                else LevelOne.gameObject.SetActive(false);
            }
            incomeText.text = "Income: " + transform.GetComponent<PlayerProperties>().currentIncome;
            walletText.text = "Wallet: " + transform.GetComponent<PlayerProperties>().currentWallet;
            waveTimer.text = System.Math.Round(NetworkedObjectsH.find.waveTimer,2) + " seconds until next wave";
        }
        else
        {
            // interpolate from the renderer's current position to its ideal position
            appearance.position = Vector3.Lerp(appearance.position, target.position, speed * Time.deltaTime);
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

    private void OnDestroy()
    {
        NetworkedObjectsH.find.RemoveMe(playerNumber);
    }

    [PunRPC]
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        agent.Warp(position);
    }

    [PunRPC]
    public void SpawnCreeps(List<int> creeplist, int playerNumb)
    {
        UnitSpawner.find.SpawnCreeps(creeplist, playerNumb);
    }

    [PunRPC]
    public void AddToMasterCreepList(int playerNumb, int creep)
    {
        NetworkedObjectsH.find.AddToMasterCreepList(playerNumb, creep);
    }

    [PunRPC]
    public void SyncWaveTimer(int timer)
    {
        NetworkedObjectsH.find.SyncWaveTimer(timer);
    }
}
