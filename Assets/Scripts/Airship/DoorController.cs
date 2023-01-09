using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform doorPlank;
    public Transform lDoor;
    public Transform rDoor;
    public GameObject doorCollider;

    [Space]
    public float moveSpeed = 3f;
    public Vector3 doorMovement = Vector3.right * 3;

    [Space]
    public float rotSpeed = 90;
    public float lDoorRotAmount = -90;
    public float rDoorRotAmount = 90;

    [Space]
    public Mesh doorPlankMesh;
    public Mesh lDoorMesh;
    public Mesh rDoorMesh;

    Vector3 plankStartPos;

    private void Start()
    {
        plankStartPos = doorPlank.localPosition;
    }

    void Update()
    {
        if (Airship.Docked)
        {
            doorCollider.SetActive(false);
            doorPlank.localPosition = Vector3.MoveTowards(doorPlank.localPosition, plankStartPos + doorMovement, Time.deltaTime * moveSpeed);
            lDoor.localRotation = Quaternion.RotateTowards(lDoor.localRotation, Quaternion.Euler(0, lDoorRotAmount, 0), Time.deltaTime * rotSpeed);
            rDoor.localRotation = Quaternion.RotateTowards(rDoor.localRotation, Quaternion.Euler(0, rDoorRotAmount, 0), Time.deltaTime * rotSpeed);
        }
        else
        {
            doorCollider.SetActive(true);
            doorPlank.localPosition = Vector3.MoveTowards(doorPlank.localPosition, plankStartPos, Time.deltaTime * moveSpeed);
            lDoor.localRotation = Quaternion.RotateTowards(lDoor.localRotation, Quaternion.identity, Time.deltaTime * rotSpeed);
            rDoor.localRotation = Quaternion.RotateTowards(rDoor.localRotation, Quaternion.identity, Time.deltaTime * rotSpeed);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!doorPlank || !doorPlank) return;

        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(door.position, 0.1f);
        Gizmos.DrawMesh(doorPlankMesh, doorPlank.position, doorPlank.rotation);
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(doorPlankMesh, doorPlank.position + doorPlank.TransformDirection(doorMovement), doorPlank.rotation);

        if (!lDoorMesh || !rDoorMesh || !lDoor || !rDoor) return;

        Gizmos.color = Color.red;
        Gizmos.DrawMesh(lDoorMesh, lDoor.position, lDoor.rotation);
        Gizmos.DrawMesh(rDoorMesh, rDoor.position, rDoor.rotation);

        Gizmos.color = Color.green;
        Gizmos.DrawMesh(lDoorMesh, lDoor.position, lDoor.rotation * Quaternion.Euler(0, lDoorRotAmount, 0));
        Gizmos.DrawMesh(rDoorMesh, rDoor.position, rDoor.rotation * Quaternion.Euler(0, rDoorRotAmount, 0));
    }
}
