using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControl : MonoBehaviour
{
    public Tilemap MyTileMap;
    public GameObject follow;
    public GameObject map;
    public GameObject player;

    public GameObject CanvasPlayer;
    public GameObject CanvasMiniMap;
    public Camera MainCamera;

    private Vector2 minX;
    private Vector2 maxY;

    private int size;

    private Camera cam;
    private test Test;
    private Texture2D myTexture;
    private Dictionary<string, Color> MapTexture;

    private int[,] tiles;
    private List<Tile> myTiles = new List<Tile>();

    void Start()
    {
        Test = GameObject.Find("simulation").GetComponent<test>();

        size = Test.size;

        cam = GetComponent<Camera>();

        myTexture = new Texture2D(size, size);
        myTexture.filterMode = FilterMode.Point;
        myTexture.Apply();
        map.GetComponent<Renderer>().material.mainTexture = myTexture;

        map.transform.position = new Vector3(size / 2 - 10000, size / 2, 100);
        map.transform.localScale = new Vector3(size, size, 1);

        MapTexture = new Dictionary<string, Color>
        {
            {"end", new Color(0, 173f / 255f, 166f / 255f)},
            {"evil", new Color(136f / 255f, 9f / 255f, 27f / 255f)},
            {"green", Color.green},
            {"g", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"magic", new Color(184f / 255f, 61f / 255f, 186f / 255f)},
            {"neutral", Color.grey},
            {"sand", new Color(0, 52f / 255f, 0)},
            {"water", new Color(63f / 255f, 72f / 255f, 204f / 255f)},

            {"CleanGreen", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"GreenGrass", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest65", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest66", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest67", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest68", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest69", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest70", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest71", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest72", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest73", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest74", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest75", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
            {"forest76", new Color(127f / 255f, 145f / 255f, 69f / 255f)},
        };

        for (int x = 0; x < size; x++)
        {

            for (int y = 0; y < size; y++)
            {
                myTexture.SetPixel(x, y, new Color32(0, 0, 0, 0));

            }
        }
        myTexture.Apply();
    }

    void Update()
    {

        if (InMap == false)
        {
            MiniMapTransform();
            MainCameraTransform();
        }
        else
        {

        }
    }

    private void MainCameraTransform()
    {
        MainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        MainCamera.transform.position = new Vector3(Mathf.Clamp(MainCamera.transform.position.x, MainCamera.transform.position.x - MainCamera.ViewportToWorldPoint(new Vector2(0, 0)).x + 2, size - (MainCamera.transform.position.x - MainCamera.ViewportToWorldPoint(new Vector2(0, 0)).x)) - 1, Mathf.Clamp(MainCamera.transform.position.y, MainCamera.transform.position.y - MainCamera.ViewportToWorldPoint(new Vector2(0, 0)).y + 1, size - (MainCamera.transform.position.y - MainCamera.ViewportToWorldPoint(new Vector2(0, 0)).y) - 1), -10);
    }

    private void MiniMapTransform()
    {
        follow.transform.position = new Vector3(player.transform.position.x - 10000, player.transform.position.y, 0);
        transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, -10);
        Borders();
        float border = size - ((maxY.y - minX.y) / 2) - 10000;
        transform.position = new Vector3(Mathf.Clamp(follow.transform.position.x, (maxY.y - minX.y) / 2 - 10000, border), Mathf.Clamp(follow.transform.position.y, (maxY.y - minX.y) / 2, size - ((maxY.y - minX.y) / 2)), -10);
    }

    public void startPaint()
    {
        StartCoroutine(PaintTexture());
    }

    public void StartChunks()
    {
        tiles = Test.tiles;
        myTiles = Test.myTiles;
        StartCoroutine(Chunk());
    }

    IEnumerator Chunk()
    {
        yield return new WaitForSeconds(0.1f);

        int maxX = Mathf.RoundToInt(player.transform.position.x) + 30;
        int mxY = Mathf.RoundToInt(player.transform.position.y) + 30;
        int X = Mathf.RoundToInt(player.transform.position.x) - 30;
        int Y = Mathf.RoundToInt(player.transform.position.y) - 30;

        for (int x = X; x < maxX; x++)
        {
            for (int y = Y; y < mxY; y++)
            {
                if (x >= 0 && x < size && y >= 0 && y < size && MyTileMap.GetTile(new Vector3Int(x,y,0)) == null)
                {
                    MyTileMap.SetTile(new Vector3Int(x, y, 0), myTiles[tiles[x, y]]);
                }

            }
        }

        StartCoroutine(Chunk());
    }

    IEnumerator PaintTexture()
    {

        yield return new WaitForSeconds(0.1f);

        int maxX = Mathf.RoundToInt(player.transform.position.x) + 14;
        int mxY = Mathf.RoundToInt(player.transform.position.y) + 14;
        int X = Mathf.RoundToInt(player.transform.position.x) - 14;
        int Y = Mathf.RoundToInt(player.transform.position.y) - 14;
        TileBase tile;

        for (int x = X; x < maxX; x++)
        {
             for(int y = Y; y < mxY; y++)
             {
                if (x >= 0 && x < size && y >= 0 && y < size && myTexture.GetPixel(x, y) == new Color32(0, 0, 0, 0))
                {
                    tile = MyTileMap.GetTile(new Vector3Int(x, y, 0));
                    myTexture.SetPixel(x, y, MapTexture[tile.name]);
                }
             }
        }
        myTexture.Apply();
        StartCoroutine(PaintTexture());
    }

    private void Borders()
    {
         minX = cam.ViewportToWorldPoint(new Vector2(0, 0)); // bottom-left corner
         maxY = cam.ViewportToWorldPoint(new Vector2(0, 1)); // top-right corner
    }

    private bool InMap = false;

    public void ButtonOpenMap()
    {
        InMap = true;
        CanvasPlayer.SetActive(false);
        CanvasMiniMap.SetActive(true);
        MainCamera.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, -10);
        MainCamera.orthographicSize = 100;
        MainCamera.GetComponent<ZoomCamera>().enabled = true;
    }

    public void ButtonCloseMap()
    {
        InMap = false;
        CanvasPlayer.SetActive(true);
        CanvasMiniMap.SetActive(false);
        MainCamera.orthographicSize = 8;
        MainCamera.GetComponent<ZoomCamera>().enabled = false;
    }
}
