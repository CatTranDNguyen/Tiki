using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int length_bomb = 2;
    public float timeWait = 3;
    public Rigidbody explose;
    [HideInInspector] public Map map;
    [HideInInspector] public KnightController knight;

    private float tmptime = 0;
    private void Start()
    {

    }
    bool play = false;
    bool isexplose = false;
    // Update is called once per frame
    void Update()
    {

        tmptime += Time.deltaTime;
        if (tmptime >= 4f)
        {
            Destroy(this.gameObject);
        }
        else  if (!isexplose && tmptime >= timeWait)
        {
            isexplose = true;
            int x = (int)Math.Round(transform.position.x);
            int z = (int)Math.Round(transform.position.z);
            Explose();
            map.ExploseBomb(x, z);
            knight.count_bomb--;
            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].enabled = false;
            }
        }
        else if (tmptime >= 1.0f && tmptime <= 1.5f)
        {
            int x = (int)Math.Round(transform.position.x);
            int z = (int)Math.Round(transform.position.z);
            map.SpawnBomb(x, z);
        }
        else if (!play && tmptime >= 2.6f)
        {
            play = true;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
    }

    private void SpawnExplose(int x, int z)
    {
        Vector3 pos = new Vector3(x, 0, z);
        Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0));
        Rigidbody shellInstance =
            Instantiate(explose, pos, rot) as Rigidbody;
    }

    private void Explose()
    {
        bool L, T, B, R;
        L = T = B = R = true;
        int x = (int)Math.Round(transform.position.x);
        int z = (int)Math.Round(transform.position.z);

        for (int k = 0; k < length_bomb; k++)
        {
            if (R && map.isOkToSpawnExplose(x + k, z))
                SpawnExplose(x + k, z);
            else
                R = false;
            if (L && map.isOkToSpawnExplose(x - k, z))
                SpawnExplose(x - k, z);
            else
                L = false;
            if (B && map.isOkToSpawnExplose(x, z + k))
                SpawnExplose(x, z + k);
            else
                B = false;
            if (T && map.isOkToSpawnExplose(x, z - k))
                SpawnExplose(x, z - k);
            else
                T = false;
        }
    }
}
