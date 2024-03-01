using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float    runSpeed = 40f;
    
    private PlayerMovement  controller;
    private float           horizontalMove = 0f;
    private bool            isJump = false;

    private void Start()
    {
        controller = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        bool buildSucess;
        if (Input.GetButtonDown("Jump"))
            isJump = true;
        if (Input.GetMouseButtonDown(0))
        {
            buildSucess = false;
            if (InventoryManager.instance.isPlaceable())
                buildSucess = controller.Build();
            if (buildSucess == false)
            {
                controller.Mine();
                controller.Attack();
            }
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
        if (isJump)
        {
            controller.Jump();
            isJump = false;
        }
    }

    public void MoveToOpenSpace()
    {
        while (Physics2D.OverlapPoint(transform.position) != null)
        {
            Debug.Log("call");
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }
}
