using System;
using UnityEngine;

public class BoxTest : MonoBehaviour
{
    [SerializeField] private LayerMask _playerAttackLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Debug.Log("Hit");
        }
    }
}
