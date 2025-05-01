//using UnityEngine;

//public GameObject healthPickupPrefab;
//public Transform[] spawnPoints;

//private float timer;
//private float interval;

//void Start()
//{
//    interval = Random.Range(240f, 300f);
//}

//void Update()
//{
//    timer += Time.deltaTime;
//    if (timer >= interval)
//    {
//        SpawnPickup();
//        timer = 0f;
//        interval = Random.Range(240f, 300f);
//    }
//}

//void SpawnPickup()
//{
//    int index = Random.Range(0, spawnPoints.Length);
//    Instantiate(healthPickupPrefab, spawnPoints[index].position, Quaternion.identity);
//}
