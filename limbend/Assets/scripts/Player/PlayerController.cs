using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private test g;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;


    public Joystick joystick;
    public Camera GameCamera;

    public float speed;
    public float drag = 5f;

    private Vector2 moveVec;

    private Rigidbody2D rb;
    private Renderer rd;

    void Start()
    {
        StartCoroutine(getBiom());

        rb = GetComponent<Rigidbody2D>();
        rd = GetComponent<Renderer>();
        g = GameObject.Find("simulation").GetComponent<test>();
    }

    private void FixedUpdate()
    {
        moveVec = new Vector2(joystick.Horizontal * speed, joystick.Vertical * speed);
        rb.AddForce(moveVec, ForceMode2D.Impulse);
        rb.drag = drag;
    }

    void Update()
    {
        moveBorders();
        var newPosX = Mathf.Clamp(transform.position.x, xMin, xMax);
        var newPosY = Mathf.Clamp(transform.position.y, yMin, yMax);

        transform.position = new Vector3(newPosX, newPosY, 0);
    }

    private void moveBorders()
    {
        xMin = GameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + rd.bounds.extents.x;

        xMax = GameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - rd.bounds.extents.x;

        yMin = GameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + rd.bounds.extents.y;

        yMax = GameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - rd.bounds.extents.y;
    }

    IEnumerator getBiom()
    {
        yield return new WaitForSeconds(1);

        try 
        { 
            Debug.Log(g.GetTile(new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0)));
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
        
        //StartCoroutine(getBiom());
    }
}
