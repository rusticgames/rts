using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour {
    public Vector2 moveDirection;
    public float moveForce;
    public Vector2 velocity;
    public ForceMode2D forceMode;


	// Use this for initialization
	void Start () {
        StartCoroutine(moveRoutine());
	}


    IEnumerator moveRoutine()
    {
        while (true)
        {
            if (this.transform.position.x > 30)
            {
                Vector3 newPosition = this.transform.position;
                newPosition.x -= 20;
                this.transform.position = newPosition;
            }
            this.rigidbody2D.AddForce(moveDirection * moveForce, forceMode);
            yield return new WaitForFixedUpdate();
        }
    }

    void OnDrawGizmos()
    {
        velocity = this.rigidbody2D.velocity;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.rigidbody2D.position, moveDirection);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(this.rigidbody2D.position, velocity);
    }
    /** /
    bool IsGrounded()
    {
        groundedOrigin = this.rigidbody2D.position;
        groundedOrigin.y -= (0.5f * this.GetComponent<CircleCollider2D>().size.y) + 0.01f;
        RaycastHit2D hit = Physics2D.Raycast(groundedOrigin, -Vector2.up, 0.01f);
        if (hit.collider != null)
        {
            Debug.Log("BOY YOU GROUNDED" + hit.collider);
        }
        else
        {
            Debug.Log("AIN'T GROUNDED: " + groundedOrigin + ", orig: " + this.rigidbody2D.position);
        }
        return (hit.collider != null) ? true : false;      
    }/**/
}
