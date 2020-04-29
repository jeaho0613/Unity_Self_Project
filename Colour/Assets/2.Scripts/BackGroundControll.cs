using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControll : MonoBehaviour
{
    public float speed; // 움직일 스피드
    Material myMaterial; // 이미지가 변경 될 Material

    private void Awake()
    {
        myMaterial = GetComponent<Renderer>().material; // 초기화
    }

    private void Update()
    {
        // Material의 Y값이 무제한 올라가지 않게 5에서 초기화
        if (myMaterial.mainTextureOffset.y > 5)
        {
            myMaterial.mainTextureOffset = new Vector2(0, myMaterial.mainTextureOffset.y * 0);
        }
        float newOffsetY = myMaterial.mainTextureOffset.y + speed * Time.deltaTime; // offset 변경
        Vector2 newOffset = new Vector2(0, newOffsetY);

        myMaterial.mainTextureOffset = newOffset;
    }
}
