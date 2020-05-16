using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string horizontalInputName = "Horizontal";
    public string verticalInputName = "Vertical";

    public float xInput { get; private set; }
    public float yInput { get; private set; }

    public Vector2 moveInput { get; private set; }

    void Update()
    {
        xInput = Input.GetAxisRaw(horizontalInputName) * Time.deltaTime;
        yInput = Input.GetAxisRaw(verticalInputName) * Time.deltaTime;

        moveInput = new Vector2(xInput, yInput);
    }
}
