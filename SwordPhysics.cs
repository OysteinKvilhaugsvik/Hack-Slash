using UnityEngine;

public class SwordPhysics : MonoBehaviour
{
    public float minSpeed = 3.0f;
    private Rigidbody rb;
    private Vector3 previousPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        rb.velocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;
        previousPosition = currentPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float speed = rb.velocity.magnitude;
            Debug.Log(speed);
            if (speed > minSpeed)
            {
                Debug.Log("Sword is moving fast!");
                EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
                enemyController.TakeDamage();
            }
        }
    }
}
