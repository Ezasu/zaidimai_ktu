using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeResizer : MonoBehaviour
{
    [Range(1, 100)]
    public int Cubes = 1;
    int oldCubes = 1;

    public float Gap = 1f;

    public GameObject CubeReference;

    public List<GameObject> spawnedCubes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldCubes == Cubes)
            return;

        spawnedCubes.ForEach(e => Destroy(e));

        oldCubes = Cubes;
        for (int i = 0; i < Cubes; i++)
        {

            var newCube = Instantiate(CubeReference);
            spawnedCubes.Add(newCube);
            newCube.transform.position += Vector3.left * i * Gap; //+ Vector3.up * Gap;

            for (int ii = 0; ii < Cubes; ii++)
            {
                //GameObject added = null;
                //var testList = new List<GameObject>(spawnedCubes);
                //if (!testList.Any(e => e.transform.position == newCube.transform.position + Vector3.forward * ii * Gap))
                //{
                    
                //}
                //if (added != null)
                //    spawnedCubes.Add(added);

                var newCube1 = Instantiate(CubeReference);
                spawnedCubes.Add(newCube1);
                newCube1.transform.position = newCube.transform.position + Vector3.forward * ii * Gap; //+ Vector3.up * Gap;
                //added = newCube1;

                for (int iii = 0; iii < Cubes; iii++)
                {

                    var newCube2 = Instantiate(CubeReference);
                    spawnedCubes.Add(newCube2);
                    //newCube2.transform.position = newCube1.transform.position + Vector3.forward * ii * Gap + Vector3.up * iii * Gap;
                    newCube2.transform.position = newCube1.transform.position + Vector3.up * iii * Gap;
                    //newCube2.transform.position += Vector3.up * iii * Gap; //+ Vector3.up * Gap;
                }
            }
        }


    }
}
