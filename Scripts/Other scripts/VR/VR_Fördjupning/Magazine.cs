using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int magazineCount = 30;
    int currentAmmoCount;

    public GameObject magazineOffset;

    // Start is called before the first frame update
    void Start()
    {
        if(magazineOffset == null)
        {
            magazineOffset = gameObject;
        }

        currentAmmoCount = magazineCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetNextBullet()
    {
        bool bulletLeft = false;
        if (currentAmmoCount > 0)
        {
            bulletLeft = true;
            currentAmmoCount--;
        }

        return bulletLeft;
    }
}
