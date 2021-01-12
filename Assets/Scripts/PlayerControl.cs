using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    [SerializeField]
    private float deadZone = 10f;

    [SerializeField]
    float speed = 0.10f;

    private Rigidbody rb;

    private Vector3 direction;

    private float speedLimit = 150f;

    float damp = 0.15f;

    public GameObject levTool;

    private bool canMove = true;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var CursorPos = Input.mousePosition;

        var x = Screen.width / 2;
        var y = Screen.height / 2;

        var dirToCursor = new Vector2(CursorPos.x - x, CursorPos.y - y);

        if (dirToCursor.x > speedLimit)
        {
            dirToCursor.x = speedLimit;
        }

        if (dirToCursor.x < -speedLimit)
        {
            dirToCursor.x = -speedLimit;
        }

        if (dirToCursor.y > speedLimit)
        {
            dirToCursor.y = speedLimit;
        }

        if (dirToCursor.y < -speedLimit)
        {
            dirToCursor.y = -speedLimit;
        }

        direction = Vector2.zero;

        if (Vector2.Distance(dirToCursor, Vector2.zero) > deadZone && canMove)
        {
            direction = new Vector3(dirToCursor.x, 0, dirToCursor.y) * -1;
        }
        else
        {
            direction = Vector2.zero;
        }

        rb.velocity = Vector3.Lerp(rb.velocity, direction * speed, damp);

        transform.rotation = Quaternion.Euler(rb.velocity.normalized * -0.5f);

        if (Input.GetMouseButton(0))
        {
            levTool.GetComponent<CapsuleCollider>().enabled = true;
            if (levTool.transform.localScale.x < 1.5)
            {
                levTool.transform.localScale = new Vector3 (levTool.transform.localScale.x +0.1f, levTool.transform.localScale.y, levTool.transform.localScale.z+0.1f);
            }
            
        }
        else
        {
            levTool.GetComponent<CapsuleCollider>().enabled = false;
            levTool.transform.localScale = new Vector3(0, levTool.transform.localScale.y, 0);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            direction = new Vector3(other.transform.position.x, 0, other.transform.position.z) * -1;
            rb.velocity = Vector3.Lerp(rb.velocity, direction * speed * 30, damp);
            canMove = false;
            StartCoroutine(WaitForMove());
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, direction * speed, damp);
        }
    }

    private IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

}
