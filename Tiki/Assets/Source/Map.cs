using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map : MonoBehaviour
{
    private const int N = 15;
    private const int SOFT_ID = 1;
    private const int TOUGHT_ID = 2;
    private const int LIVE_ITEM_ID = 1;
    private const int BOMB_ITEM_ID = 2;
    private const int LENGTH_ITEM_ID = 3;
    private const int BOMB_ID = 4;
    private const int SPEED_ITEM_ID = 4;
    [HideInInspector] public int[,] matrix = new int[N, N];
    [HideInInspector] public int[,] matrixitem = new int[N, N];



    [HideInInspector] public Rigidbody[,] matrixRigid = new Rigidbody[N, N];

    public Rigidbody prefabTypeSoft;  //booom khi trung bom
    public Rigidbody prefabTypeTought;

    public Rigidbody prefabLives;
    public Rigidbody prefabBomb;
    public Rigidbody prefabLength;
    public Rigidbody prefabSpeed;

    public TextAsset Map1;
    public TextAsset Map2;

    private int numSoftObj = 0;

    int[] tointarray(string value)
    {
        char[] sep = new char[3] { '\t', '\r', '\n' };
        string[] sa = value.Split();
        int[] ia = new int[sa.Length];
        for (int i = 0; i < ia.Length; ++i)
        {
            int j;
            string s = sa[i];
            if (int.TryParse(s, out j))
            {
                ia[i] = j;
            }
            else
                ia[i] = -1;
        }
        return ia;
    }


    void Start()
    {
        createMap();
    }

    void createMap()
    {
        TextAsset Map = Map1;
        if (LevelMenu.levelChoose == 2)
            Map = Map2;
        int[] tmp = tointarray(Map.text);  //this is the content as string
        int z = 0; int x = 0;
        for (int k = 0; k < tmp.Length; k++)
        {
            if (tmp[k] != -1)
            {
                matrix[z, x] = tmp[k];
                x++;
                z += x / N;
                x %= N;
            }
        }

        for (z = 0; z < N; z++)
        {
            for (x = 0; x < N; x++)
            {
                if (matrix[z, x] == SOFT_ID)
                {
                    Rigidbody shellInstance =
                         Instantiate(prefabTypeSoft, new Vector3(x - N / 2, 0, z - N / 2), Quaternion.Euler(new Vector3(0, 180, 0))) as Rigidbody;
                    matrixRigid[z, x] = shellInstance;
                    numSoftObj++;

                }
                else if (matrix[z, x] == TOUGHT_ID)
                {
                    Rigidbody shellInstance =
                         Instantiate(prefabTypeTought, new Vector3(x - N / 2, 0, z - N / 2), Quaternion.Euler(new Vector3(0, 180, 0))) as Rigidbody;
                    matrixRigid[z, x] = shellInstance;

                }
                else
                    matrixRigid[z, x] = null;
            }
        }

        CreateMatrixItem();
    }

    public void ExploseBomb(int x, int z)
    {
        matrix[z + N / 2, x + N / 2] = 0;
    }

    public void SpawnBomb(int x, int z)
    {
        matrix[z + N / 2, x + N / 2] = BOMB_ID;
    }

    private void CreateMatrixItem()
    {
        int lives = 2;
        int bombs = 13;
        int lengths = 14;
        int speed = 15;

        int numLives, numBombs, numLength, numSpeed;
        numLives = numBombs = numLength = numSpeed = 0;
        bombs += lives;
        lengths += bombs;
        speed += lengths;

        for (int z = 0; z < N; z++)
        {
            for (int x = 0; x < N; x++)
            {
                if (matrix[z, x] == SOFT_ID)
                {
                    while (true)
                    {
                        int k = UnityEngine.Random.Range(0, 60);
                        if (k < lives)
                        {
                            numLives++;
                            if (numLives <= lives)
                            {
                                matrixitem[z, x] = LIVE_ITEM_ID;
                                break;
                            }
                            else continue;
                        }
                        else if (k < bombs)
                        {
                            numBombs++;
                            if (numBombs <= bombs)
                            {
                                matrixitem[z, x] = BOMB_ITEM_ID;
                                break;
                            }
                            else continue;
                        }
                        else if (k < lengths)
                        {
                            numLength++;
                            if (numLength <= lengths)
                            {
                                matrixitem[z, x] = LENGTH_ITEM_ID;
                                break;
                            }
                            else continue;
                        }
                        else if (k < speed)
                        {
                            numSpeed++;
                            if (numSpeed <= speed)
                            {
                                matrixitem[z, x] = SPEED_ITEM_ID;
                                break;
                            }
                            else continue;
                        }
                        else
                        {
                            matrixitem[z, x] = 0;
                            break;
                        }
                    }
                }
                else
                    matrixitem[z, x] = 0;
            }
        }
    }

    private bool isOutside(int x, int z)
    {
        return !(x >= 0 && x < N && z >= 0 && z < N);
    }

    public bool isEmpty(int x, int z)
    {
        x += N / 2;
        z += N / 2;
        if (isOutside(x, z))
            return false;
        return matrix[z, x] == 0;
    }

    void SpawnItemAt(int x, int z, int ID)
    {
        Rigidbody item;
        switch (ID)
        {
            case LIVE_ITEM_ID: item = prefabLives; break;
            case BOMB_ITEM_ID: item = prefabBomb; break;
            case LENGTH_ITEM_ID: item = prefabLength; break;
            default: item = prefabSpeed; break;
        }
        Rigidbody Instance =
                        Instantiate(item, new Vector3(x - N / 2, 0.5f, z - N / 2), Quaternion.Euler(new Vector3(0, 180, 0))) as Rigidbody;

    }

    internal bool isOKToSpawnBomb(int tmpx, int tmpz)
    {
        tmpx += N / 2;
        tmpz += N / 2;
        return matrix[tmpz, tmpx] == 0;
    }

    void DestroyObject(int x, int z)
    {
        x += N / 2;
        z += N / 2;
        if (matrix[z, x] == SOFT_ID)
        {
            Destroy(matrixRigid[z, x].gameObject);
            matrix[z, x] = 0;
        }
        if (matrixitem[z, x] != 0)
        {
            SpawnItemAt(x, z, matrixitem[z, x]);
            matrixitem[z, x] = 0;
        }
    }

    public bool isOkToSpawnExplose(int x, int z)
    {
        if (isOutside(x + N / 2, z + N / 2))
            return false;
        if (isEmpty(x, z) || matrix[z + N / 2, x + N / 2] == BOMB_ID) return true;
        DestroyObject(x, z);
        return false;
    }

    public Vector2 RandomEmptyPosition()
    {
        while (true)
        {
            int x = UnityEngine.Random.Range(0, N);
            int z = UnityEngine.Random.Range(0, N);
            if (matrix[z,x] == 0)
            {
                return new Vector2(x - N / 2, z - N / 2);
            }
        }
    }
}
