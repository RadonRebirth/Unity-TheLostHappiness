using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public GameObject player;

    public Collider2D col;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical);
        AnimateMovement(direction);

        transform.position += direction * speed * Time.fixedDeltaTime;

        if (direction.x < 0)
        {
           spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 5;
        }
        else 
        { 
            speed = 3;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    }

    void AnimateMovement(Vector3 direction)
    {
        if(animator != null)
        {
            if(direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);

                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
                
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            
        }
    }
}
