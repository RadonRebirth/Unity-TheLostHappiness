using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform player; // ссылка на игрока
    public Vector3 offset; // смещение камеры относительно игрока
    
    [SerializeField]
    float uplim;
    [SerializeField]
    float downlim;
    [SerializeField]
    float leftlim;
    [SerializeField]
    float rightlim;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;

        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, leftlim, rightlim),
            Mathf.Clamp(transform.position.y, downlim, uplim),
            -1
            );
    }
}
