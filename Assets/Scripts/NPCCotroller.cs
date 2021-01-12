using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCotroller : MonoBehaviour
{

    private Rigidbody rb;

    private Animator anim;

    private Camera cam;

    [SerializeField]
    private float rotationAngle;

    private bool onLevTool = false;

    private bool isGrounded = false;

    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rotationAngle = Random.Range(0, 360);

        InvokeRepeating("ChangeAngle", 20f, Random.Range(15, 20));
        InvokeRepeating("CheckPos", 5, 3);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", isGrounded);
        GroundCheck();

        if (onLevTool)
        { }
        else
        {
            if (isGrounded)
            {
                rb.velocity = transform.forward;
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }
            else
            {
                rb.velocity = Vector3.down * 5;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Envi"))
        {
            ChangeAngle();
            if (rotationAngle >= 360)
            {
                rotationAngle = 0;
            }
            
        }
        onLevTool = false;
    }

    void ChangeAngle()
    {
        rotationAngle += Random.Range(90, 270);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("levTool"))
        {
            onLevTool = true;
            rb.velocity = Vector2.up * 3;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("UFO"))
        {
            Destroy(this.gameObject);
            cam.GetComponent<NPCSpawner>().count--;
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 0.1f;
        Vector3 dir = Vector3.down;

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CheckPos()
    {
        if (Vector3.Distance(lastPos, transform.position) < 1)
        {
            ChangeAngle();
        }

        lastPos = transform.position;
    }
}
