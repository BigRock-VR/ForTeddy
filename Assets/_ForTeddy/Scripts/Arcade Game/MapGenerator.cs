using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> distributions;
    [SerializeField]
    List<GameObject> squareType;
    [SerializeField]
    List<GameObject> rectangleType;
    [SerializeField]
    Transform arcadeRoom;

    [SerializeField]
    bool spawnMaps;

    [SerializeField]
    Transform instanciatedMap;

    [SerializeField]
    float refreshRate = 3;
    float _timer = 3;

    public void GenerateMap()
    {
        instanciatedMap = Instantiate(distributions[Random.Range(0, distributions.Count)], arcadeRoom).transform;
        for (int i = 0; i < instanciatedMap.childCount; i++)
        {
            if (instanciatedMap.GetChild(i).name == "t1")
            {
                var x = Instantiate(squareType[Random.Range(0, squareType.Count)]);
                x.transform.SetParent(instanciatedMap.GetChild(i));
                x.transform.localPosition = Vector3.zero;
                x.transform.Rotate(0, 1 * 90 * Random.Range(1, 4), 0);
            }
            if (instanciatedMap.GetChild(i).name == "t2")
            {
                var z = Instantiate(rectangleType[Random.Range(0, squareType.Count)]);
                z.transform.SetParent(instanciatedMap.GetChild(i));
                z.transform.localPosition = Vector3.zero;
                z.transform.Rotate(0, 1 * 180 * Random.Range(1, 2), 0);
            }
        }
    }

    private void LateUpdate()
    {
        if(spawnMaps)
        {
            _timer -= Time.deltaTime;

            if(_timer <= refreshRate/2)
            {
                if (instanciatedMap != null)
                {
                    instanciatedMap.gameObject.SetActive(false);
                    Destroy(instanciatedMap.gameObject);
                }
            }
            if(_timer <= 0)
            {
                _timer = refreshRate;

                GenerateMap();
            }
         

        }
    }

    private void Start()
    {
        _timer = refreshRate;
    }
}
