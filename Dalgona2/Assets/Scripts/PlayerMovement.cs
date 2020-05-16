using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [Range(1f, 5f)] public float speed;
    private PlayerInput playerInput;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        transform.Translate(playerInput.moveInput * speed);
    }
}
