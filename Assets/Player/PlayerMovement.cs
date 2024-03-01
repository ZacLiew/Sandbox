using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Map map;

    private const float groundCheckLength = 0.8f;
    private Rigidbody2D rb2d;
    private bool isFacingRight = true;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private TilesSheet tileSheet;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

    }

    public void Move(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb2d.velocity.y);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref velocity, movementSmoothing);

        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();
    }

    public void Jump()
    {
        if (CheckIsGrounded())
            rb2d.AddForce(new Vector2(0f, jumpForce));
    }

    public void Mine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;

        if (map.RayCheckInRange(transform.position, direction, 3, whatIsGround, mousePos))
            map.ChangeTile("Main", mousePos, null);
    }

    public void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        RaycastHit2D attackCheck = Physics2D.Raycast(transform.position, direction, 3, whatIsEnemy);

        Debug.Log(attackCheck.collider);

        if (attackCheck.collider != null)
        {
            attackCheck.collider.GetComponent<EnemyAI>().takeDamage(3);
            Debug.Log("attacked");
        }
    }

    public bool Build()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float distance = Vector3.Distance(transform.position, mousePos);
        if (map.CheckEmpty(mousePos) && distance < 3)
        {
            map.ChangeTile("Main", mousePos, tileSheet.tiles[0].tile);
            InventoryManager.instance.decreaseInventory();
            return (true);
        }
        return (false);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private bool CheckIsGrounded()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, whatIsGround);
        if (groundCheck.collider != null)
            return (true);
        return (false);
    }
}
