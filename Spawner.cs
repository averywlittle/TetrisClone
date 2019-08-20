using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    // Groups
    public GameObject[] groups;

    public void spawnNext() {
        // Random index
        int i = Random.Range(0, groups.Length);

        // Spawn group at current position
        Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Spawn initial group
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
