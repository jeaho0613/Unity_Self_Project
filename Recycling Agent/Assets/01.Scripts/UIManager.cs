using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Text text1;
    public
    void Start()
    {
        text1.DOText("This text will appear from scrambled chars", 2, true, ScrambleMode.All).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }
    void Update()
    {
        
    }
}
