using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private test g;

    private int size;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Joystick joystick;
    public Joystick JoyAttack;

    public Transform AttackPosLeft;
    public Transform AttackPosRight;
    public Transform AttackPosUp;
    public Transform AttackPosDown;

    public Vector2 boxSize;
    public Vector2 boxSizeSide;
    public LayerMask resours;

    public float speed;
    public float drag = 5f;

    private Vector2 moveVec;

    private Rigidbody2D rb;
    private Renderer rd;
    private SpriteRenderer sp;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        rd = GetComponent<Renderer>();
        animator = GetComponent<Animator>();

        g = GameObject.Find("simulation").GetComponent<test>();
        test.StartGetBiomPlayer += StartGetBiom;

        try
        {
            MenuController m = GameObject.Find("MenuContoller").GetComponent<MenuController>();

            if (m.WorldSize == 0)
            {
                size = 300;
            }
            else if (m.WorldSize == 1)
            {
                size = 600;
            }
            else if (m.WorldSize == 2)
            {
                size = 1000;
            }
            else if (m.WorldSize == 3)
            {
                size = 5000;
            }
            else
            {
                size = 300;
            }
        }
        catch
        {
            size = 300;
        }

        moveBorders();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerAttack();
    }

    private void PlayerAttack()
    {
        float angle = Mathf.Atan2(JoyAttack.Vertical, JoyAttack.Horizontal) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle = angle + 360;
        }

        if (JoyAttack.Horizontal > 0)
        {
            sp.flipX = true;
        }
        else if (JoyAttack.Horizontal < 0)
        {
            sp.flipX = false;
        }

        if (JoyAttack.Horizontal != 0 || JoyAttack.Vertical != 0)
        {
            Collider2D[] resourses = new Collider2D[0];
            if (angle >= 45 && angle <= 135)
            {
                animator.SetBool("AttackUp", true);
                animator.SetBool("AttackDown", false);
                animator.SetBool("AttackSide", false);
            }

            resourses = Physics2D.OverlapBoxAll(AttackPosUp.position, boxSize, 0, resours);
            if (angle >= 225 && angle <= 315)
            {
                animator.SetBool("AttackDown", true);
                animator.SetBool("AttackSide", false);
                animator.SetBool("AttackUp", false);

                resourses = Physics2D.OverlapBoxAll(AttackPosDown.position, boxSize,0 , resours);
            }


            if (angle >= 135 && angle <= 225)
            {
                animator.SetBool("AttackSide", true);
                animator.SetBool("AttackUp", false);
                animator.SetBool("AttackDown", false);

                resourses = Physics2D.OverlapBoxAll(AttackPosLeft.position, boxSizeSide, 0, resours);
            }


            if ((angle > 0 && angle <= 45) || (angle >= 315 && angle <= 360))
            {
                animator.SetBool("AttackSide", true);
                animator.SetBool("AttackUp", false);
                animator.SetBool("AttackDown", false);

                resourses = Physics2D.OverlapBoxAll(AttackPosRight.position, boxSizeSide, 0, resours);
            }

            if (timeBtwAttack <= 0)
            {
                
                for (int i = 0; i < resourses.Length; i++)
                {
                    resourses[i].GetComponent<IResourses>().TakeDamage(25);
                }
                timeBtwAttack = startTimeBtwAttack;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
        else
        {
            animator.SetBool("AttackSide", false);
            animator.SetBool("AttackUp", false);
            animator.SetBool("AttackDown", false);
        }

    }

    private void PlayerMove()
    {
        moveVec = new Vector2(joystick.Horizontal * speed, joystick.Vertical * speed);
        rb.AddForce(moveVec, ForceMode2D.Impulse);
        rb.drag = drag;

        if (joystick.Horizontal > 0)
        {
            sp.flipX = true;
        }
        else if (joystick.Horizontal < 0)
        {
            sp.flipX = false;
        }

        float angle = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle = angle + 360;
        }

        if (angle >= 45 && angle <= 135)
        {
            animator.SetBool("MoveUp", true);

            animator.SetBool("MoveSide", false);
            animator.SetBool("MoveDown", false);
        }
        if (angle >= 225 && angle <= 315)
        {
            animator.SetBool("MoveDown", true);

            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveSide", false);
        }

        if (angle >= 135 && angle <= 225)
        {
            animator.SetBool("MoveSide", true);

            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveDown", false);
        }

        if ((angle > 0 && angle <= 45) || (angle >= 315 && angle <= 360))
        {
            animator.SetBool("MoveSide", true);

            animator.SetBool("MoveUp", false);
            animator.SetBool("MoveDown", false);
        }

        animator.SetFloat("Horizontal", moveVec.x);
        animator.SetFloat("Vertical", moveVec.y);
        animator.SetFloat("Speed", moveVec.sqrMagnitude);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        var newPosX = Mathf.Clamp(transform.position.x, xMin, xMax);
        var newPosY = Mathf.Clamp(transform.position.y, yMin, yMax);

        transform.position = new Vector3(newPosX, newPosY, 0);
    }

    private void moveBorders()
    {
        xMin = rd.bounds.extents.x + 1;
        xMax = size - rd.bounds.extents.x - 1;
        yMin = rd.bounds.extents.y;
        yMax = size - rd.bounds.extents.y - 1;
    }

    private void StartGetBiom()
    {
        StartCoroutine(getBiom());
    }

    IEnumerator getBiom()
    {
        yield return new WaitForSeconds(3);
        Debug.Log(g.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)));
        StartCoroutine(getBiom());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(AttackPosDown.position, boxSize);
        Gizmos.DrawWireCube(AttackPosUp.position, boxSize);
        Gizmos.DrawWireCube(AttackPosLeft.position, boxSizeSide);
        Gizmos.DrawWireCube(AttackPosRight.position, boxSizeSide);
    }
}
