using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeResizer_Oprimized : MonoBehaviour
{
    [Range(1, 100)]
    public int Cubes = 1;
    int oldCubes = 1;

    public float Gap = 1f;

    public GameObject CubeReference;

    public List<GameObject> spawnedCubes = new List<GameObject>();
    // Start is called before the first frame update
    void Update()
    {
        if (oldCubes == Cubes)
            return;

        spawnedCubes.ForEach(e => Destroy(e));
        spawnedCubes.Clear();

        oldCubes = Cubes;
        for (int i = 0; i < Cubes; i++)
        {
            var newCube = Instantiate(CubeReference);
            spawnedCubes.Add(newCube);
            newCube.transform.position += Vector3.left * i * Gap;
            for (int ii = 0; ii < Cubes; ii++)
            {
                if (!spawnedCubes.Any(e => e.transform.position == newCube.transform.position + Vector3.forward * ii * Gap))
                {
                    var newCube1 = Instantiate(CubeReference);
                    spawnedCubes.Add(newCube1);
                    newCube1.transform.position = newCube.transform.position + Vector3.forward * ii * Gap;
                }
                for (int iii = 0; iii < Cubes; iii++)
                {
                    if (!spawnedCubes.Any(e => e.transform.position == Vector3.left * i * Gap + Vector3.forward * ii * Gap + Vector3.up * iii * Gap))
                    {
                        var newCube2 = Instantiate(CubeReference);
                        spawnedCubes.Add(newCube2);
                        newCube2.transform.position = Vector3.left * i * Gap + Vector3.forward * ii * Gap + Vector3.up * iii * Gap;
                    }
                }
            }
        }
    }
}
