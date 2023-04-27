using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrailSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem windTrailPrefab;

    private List<ParticleSystem> windTrails = new List<ParticleSystem>();
    private float topEdge;
    private float bottomEdge;
    private float leftEdge;
    private float rightEdge;

    void Start()
    {
        Camera camera = Camera.main;

        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;

        topEdge = camera.transform.position.y + halfHeight;
        bottomEdge = camera.transform.position.y - halfHeight;
        leftEdge = camera.transform.position.x - halfWidth;
        rightEdge = camera.transform.position.x + halfWidth;


        StartCoroutine(SpawnParticle());
    }

    IEnumerator SpawnParticle()
    {
        while (true)
        {
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                windTrailPrefab.transform.position = new Vector2(
                    Random.Range(rightEdge, 0),
                    Random.Range(bottomEdge, topEdge));
                ParticleSystem wind = Instantiate(windTrailPrefab);
                wind.gameObject.transform.SetParent(gameObject.transform);
                windTrails.Add(wind);
                yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 2));
        }
    }

    void Update()
    {
        foreach (var windtrail in  windTrails)
        {
            if (windtrail.isStopped)
                Destroy(windtrail.gameObject);
        }

        windTrails.RemoveAll(x => x.isStopped);
    }
}
