using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("RifleFire", true);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("RifleFire", false);
        }
    }
}
