﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float jumpPower = 10f;
    public string currentColor = null;

    public Color Blue;
    public Color Yellow;
    public Color Red;
    public Color Purple;

    public Transform loadPoint;
    public GameObject[] changeColors;

    private Rigidbody2D playerRigi;
    private SpriteRenderer playerRenderer;


    void Start()
    {
        playerRigi = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        RandomColor();
    }

    void Update()
    {
        if (gameObject.tag != "Player")
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            playerRigi.velocity = Vector2.up * jumpPower;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Change")
        {
            RandomColor();
            other.gameObject.SetActive(false);
            return;
        }

        if (other.tag == "Star")
        {
            GameManager.Instance.Score += 1;
            Destroy(other.gameObject);
        }

        if (currentColor != other.tag)
        {
            foreach (var change in changeColors)
            {
                change.SetActive(true);
            }
            gameObject.transform.position = loadPoint.position;
        }
    }

    public void RandomColor()
    {
        int index = Random.Range(0, 4);

        switch (index)
        {
            case 0:
                currentColor = "Blue";
                playerRenderer.color = Blue;
                break;
            case 1:
                currentColor = "Yellow";
                playerRenderer.color = Yellow;
                break;
            case 2:
                currentColor = "Red";
                playerRenderer.color = Red;
                break;
            case 3:
                currentColor = "Purple";
                playerRenderer.color = Purple;
                break;
        }
    }
}