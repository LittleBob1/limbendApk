using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System.Collections;

public class test : MonoBehaviour
{
    public Tilemap myTileMap;

    public TMP_Text TextLoading;
    public GameObject CanvasLoading;
    public GameObject player;

    public int size;

    public int regionAmount;
    public int regionTileAmount;

    public float scaleS;
    public int octavesS;
    public float persistS;
    public float lacunarS;

    public List<Tile> myTiles = new List<Tile>();
    private int[,] tiles;

    private void Start()
    {
        StartCoroutine(CreateVoronoi());
    }

    private void Update()
    {
       
    }

    IEnumerator CreateVoronoi()
    {

        TextLoading.text = "Preparation...";

        Vector2[] points = new Vector2[regionAmount];

        tiles = new int[size, size];

        for (int i = 0; i < regionAmount; i++)
        {
            points[i] = new Vector2(Random.Range(0, size), Random.Range(0, size));
        }

        int af = 2 + 7 * Random.Range(1, regionAmount / 7);

        player.transform.position = new Vector2(points[af].x, points[af].y);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float distance = float.MaxValue;
                int value = 0;

                float perlinValue = PerlinNoise.GenerateBiomsNoise(x, y, scaleS, octavesS, persistS, lacunarS);

                for (int i = 0; i < regionAmount; i++)
                {
                    if (Vector2.Distance(new Vector2(x, y), points[i]) < distance)
                    {
                        distance = Vector2.Distance(new Vector2(x, y), points[i]);
                        value = i;
                    }
                }

                int closesRegionIndex = 0;
                float distanceRegion = float.MaxValue;
                for (int i = 0; i < regionAmount; i++)
                {
                    if (i != value)
                    {
                        if (Vector2.Distance(new Vector2(x, y), points[i]) < distanceRegion)
                        {
                            distanceRegion = Vector2.Distance(new Vector2(x, y), points[i]);
                            closesRegionIndex = i;
                        }
                    }
                }

                if (distanceRegion - distance < value)
                {
                    if (perlinValue < 0.5f)
                    {
                        tiles[x, y] = value % regionTileAmount;
                    }

                    else
                    {
                        tiles[x, y] = closesRegionIndex % regionTileAmount;
                    }
                }
                else
                {
                    tiles[x, y] = value % regionTileAmount;
                }
            }
            if (x % 2 == 0)
            {
                yield return null;
                float a = x;
                TextLoading.text = "Generation... " + Mathf.RoundToInt(a / size * 100f) + "%";
            }
        }

        
        for (int i = 0; i < regionAmount; i++)
        {
        
            for (int amount = 1; amount <= 2; amount++)
            {
                for (int y = amount; y < size - amount; y++)
                {
                    for (int x = amount; x < size - amount; x++)
                    {
                        
                        if(tiles[x + amount, y] != tiles[x, y] && tiles[x - amount, y] != tiles[x, y] || tiles[x, y + amount] != tiles[x, y] && tiles[x, y - amount] != tiles[x, y])
                        {
                            tiles[x, y] = tiles[x + amount, y];
                        }
                        
                    }
                }
            }
            
            yield return null;
            float a = i;
            TextLoading.text = "Cleaning... " + Mathf.RoundToInt(a / regionAmount * 100f) + "%";
        }
        

        for (int y = 1; y < size - 1; y++)
        {
            for (int x = 1; x < size - 1; x++)
            {
                
                if (tiles[x + 1, y] != tiles[x, y] && tiles[x - 1, y] != tiles[x, y] && tiles[x, y + 1] != tiles[x, y] && tiles[x, y - 1] != tiles[x, y])
                {
                    tiles[x, y] = tiles[x + 1, y];
                }

            }
            yield return null;
            float a = y;
            TextLoading.text = "Completion of generation... " + Mathf.RoundToInt(a / size * 100f) + "%";
        }

        
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
               myTileMap.SetTile(new Vector3Int(x,y,0), myTiles[tiles[x, y]]);
            }
            if (x % 10 == 0)
            {
                yield return null;
                float a = x;
                TextLoading.text = "Create a map... " + Mathf.RoundToInt(a / size * 100f) + "%";
            }
        
        }
        
        CameraControl cont = GameObject.Find("MiniMapCamera").GetComponent<CameraControl>();
        cont.startPaint();
        Destroy(CanvasLoading);
    }
    

    public class PerlinNoise
    {
        public static float GenerateBiomsNoise(int x, int y, float scale, int octaves, float persistance, float lacunarity)
        {
            float perlinValue = new float();
            float ampitude = 1;
            float frequency = 1;
            float noiseHeight = 1;

            for(int i = 0; i < octaves; i++)
            {
                float posX = x / scale * frequency;
                float posY = y / scale * frequency;

                perlinValue = Mathf.PerlinNoise(posX, posY) * 2 - 1;
                noiseHeight += perlinValue * ampitude;
                ampitude *= persistance;
                frequency *= lacunarity;
            }

            perlinValue = noiseHeight;
            perlinValue = Mathf.InverseLerp(-0.5f, 2f, perlinValue);

            return perlinValue;
        }
    }

    public TileBase GetTile(Vector3Int pos)
    {
        TileBase tile = myTileMap.GetTile(pos);
        return tile;
    }

}
