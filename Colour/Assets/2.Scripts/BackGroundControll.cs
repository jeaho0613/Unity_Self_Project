﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControll : MonoBehaviour
{
    public float speed;
    public Material material;
    Material myMaterial;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        myMaterial = material;
        myMaterial = GetComponent<Renderer>().material;
    }
    
    void Update()
    {
        if(myMaterial.mainTextureOffset.y > 5)
        {
            myMaterial.mainTextureOffset = new Vector2(0,myMaterial.mainTextureOffset.y * 0);
        }
        float newOffsetY = myMaterial.mainTextureOffset.y + speed * Time.deltaTime;
        Vector2 newOffset = new Vector2(0, newOffsetY);

        myMaterial.mainTextureOffset = newOffset;
    }
}