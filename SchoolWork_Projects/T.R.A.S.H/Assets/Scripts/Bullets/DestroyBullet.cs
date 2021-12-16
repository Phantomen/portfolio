using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour {
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
