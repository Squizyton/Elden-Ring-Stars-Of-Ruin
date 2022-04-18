using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsSpawnerr : MonoBehaviour
{
    [SerializeField] public GameObject starPrefab;
    [SerializeField] public Transform target;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Camera cam;
    public int spawnAmount;
    public float spawnInterval;


    private float timer;


    private void Start()
    {
        timer = spawnInterval;


        //TODO: Shoot raycast from camera to target
    }


    private void Update()
    {
        if (spawnAmount <= 0) return;
        
        if (timer <= 0)
        {
            SpawnStar();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    void SpawnStar()
    {
        timer = spawnInterval;
        spawnAmount--;
        
        var star = Instantiate(starPrefab, transform.position, transform.rotation);
        
        star.GetComponent<StarBehaviour>().SetTarget(target);
    }
}
