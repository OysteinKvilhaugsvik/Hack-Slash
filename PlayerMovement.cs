using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f; // Base movement speed of the player
    private float snapTurnRotation = 0f;

    // Update is called once per frame
private void Update()
{
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    Vector2 correctedInput = ApplySnapTurnCorrection(new Vector2(horizontal, vertical));

    Vector3 movement = new Vector3(correctedInput.x, 0f, correctedInput.y);
    movement = movement.normalized * baseSpeed;

    transform.position += movement * Time.deltaTime;
}

private Vector2 ApplySnapTurnCorrection(Vector2 input)
{
    Quaternion rotation = Quaternion.Euler(0f, snapTurnRotation, 0f);
    return rotation * input;
}

public void PerformSnapTurn(float rotationAngle)
{
    snapTurnRotation += rotationAngle;
}
}
