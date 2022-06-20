using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System.Collections;

public class test : MonoBehaviour
{
    public Tilemap myTileMap;
    public GameObject MainCameraBorder;

    public int size;

    public int regionAmount;
    public int regionTileAmount;

    public float scaleS;
    public int octavesS;
    public float persistS;
    public float lacunarS;

    public List<Tile> myTiles = new List<Tile>();
    public List<Color> myColors = new List<Color>();

    private void Start()
    {
        MainCameraBorder.transform.position = new Vector3(size / 2, size / 2, 0);
        MainCameraBorder.transform.localScale = new Vector3(size, size, 1);

        StartCoroutine(CreateVoronoi());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CreateVoronoi();
        }
    }

    IEnumerator CreateVoronoi()
    {
        Vector2[] points = new Vector2[regionAmount];

        for (int i = 0; i < regionAmount; i++)
        {
            points[i] = new Vector2(Random.Range(0, size), Random.Range(0, size));
        }

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
                        myTileMap.SetTile(new Vector3Int(x, y, 0), myTiles[value % regionTileAmount]);
                    }

                    else
                    {
                        myTileMap.SetTile(new Vector3Int(x, y, 0), myTiles[closesRegionIndex % regionTileAmount]);
                    }
                }
                else
                {
                    myTileMap.SetTile(new Vector3Int(x, y, 0), myTiles[value % regionTileAmount]);
                }
            }
            yield return null;
        }


        for (int i = 0; i < regionAmount; i++)
        {
            for (int amount = 1; amount <= 2; amount++)
            {
                for (int y = amount; y < size - amount; y++)
                {
                    for (int x = amount; x < size - amount; x++)
                    {
                        if (myTileMap.GetTile(new Vector3Int(x + amount, y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) && myTileMap.GetTile(new Vector3Int(x - amount, y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) || myTileMap.GetTile(new Vector3Int(x, amount + y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) && myTileMap.GetTile(new Vector3Int(x, y - amount, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            myTileMap.SetTile(new Vector3Int(x, y, 0), myTileMap.GetTile(new Vector3Int(x + amount, y, 0)));
                        }
                    }
                }
            }
        }

        for (int y = 1; y < size - 1; y++)
        {
            for (int x = 1; x < size - 1; x++)
            {
                if (myTileMap.GetTile(new Vector3Int(x + 1, y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) && myTileMap.GetTile(new Vector3Int(x - 1, y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) && myTileMap.GetTile(new Vector3Int(x, 1 + y, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)) && myTileMap.GetTile(new Vector3Int(x, y - 1, 0)) != myTileMap.GetTile(new Vector3Int(x, y, 0)))
                {
                    myTileMap.SetTile(new Vector3Int(x, y, 0), myTileMap.GetTile(new Vector3Int(x + 1, y, 0)));
                }
            }
        }

        CameraControl cont = GameObject.Find("MiniMapCamera").GetComponent<CameraControl>();
        cont.startPaint();
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
