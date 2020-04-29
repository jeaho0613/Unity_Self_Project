using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // 실린더
    public Gradient gradient; // 체력 상황에 따른 색 변경
    public Image fill; // 배경 이미지

    // 처음 전체 체력을 초기화 하는 변수
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health; // 체력값을 최대벨류로 초기화
        slider.value = health; // 현제 벨류의 값도 초기화

        fill.color = gradient.Evaluate(1f); // 시간 변화에 따른 값 산출
    }

    // 변경되는 체력값 업데이트
    public void SetHealth(int health)
    {
        slider.value = health; // 현재 체력값을 슬라이더 벨류에 넘겨줌

        fill.color = gradient.Evaluate(slider.normalizedValue); // 슬라이더의 색도 변경
    }


}
