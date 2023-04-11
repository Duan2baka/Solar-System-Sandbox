using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float mass;
    //大行星吞噬小行星的速度阈值
    public float englufVelocityThreshold = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        englufVelocityThreshold = 0.0f;
    }

    private void OnCollisionEnter(Collision collision){
        // Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Celestial")){
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            float newMass = rb.mass + otherRb.mass;
            if (rb.mass > otherRb.mass){
                rb.velocity = (rb.mass * rb.velocity + otherRb.mass * otherRb.velocity) / newMass;
                Vector3 initialScale = transform.localScale;
                Vector3 newScale = initialScale * Mathf.Pow(newMass / rb.mass, 1f / 3f);
                transform.localScale = newScale;
                rb.mass = newMass;
                Destroy(collision.gameObject);
            }
        }
    }
}
