using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{


    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
        }
    }

    private void Attack()
    {

    }
}
