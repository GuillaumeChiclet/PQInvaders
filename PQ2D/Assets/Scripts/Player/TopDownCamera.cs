using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform aimCursor;

    [SerializeField] private float followSpeed = 5.0f;
    [SerializeField] private float distanceBeforeAiming = 5.0f;
    [Range(0, 1)][SerializeField] private float aimingOffsetPercent = 0.3f;
    
    float zDistance = 0.0f;
    float distanceBeforeAimingSqr = 0.0f;

    private void Awake()
    {
        zDistance = transform.position.z;
        distanceBeforeAimingSqr = distanceBeforeAiming * distanceBeforeAiming;
    }

    private void Update()
    {
        Vector3 mousePos = aimCursor.transform.position;
        mousePos.z = 0.0f;   

        Vector3 playerPos = player.position;
        playerPos.z = 0.0f;

        Vector3 currentPos = transform.position;
        currentPos.z = 0.0f;

        Vector3 toCursor = mousePos - playerPos;

        Vector3 cameraTarget = playerPos;
        if (toCursor.sqrMagnitude > distanceBeforeAimingSqr)
        {
            cameraTarget += toCursor.normalized * aimingOffsetPercent;
        }

        float distance = (playerPos - currentPos).magnitude;

        Vector3 newPos = Vector3.MoveTowards(currentPos, playerPos, followSpeed * distance * Time.deltaTime);
        newPos.z = zDistance;
        transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, distanceBeforeAiming);
    }
}
