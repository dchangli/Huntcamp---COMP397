using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform _player;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z);
    }
}
