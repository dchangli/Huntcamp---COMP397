using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(DestroyBulletAfter());
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    private IEnumerator DestroyBulletAfter()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
