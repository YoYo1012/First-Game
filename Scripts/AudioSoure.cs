using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSoure : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource source;


    void Start()
    {
        
    }

    void Step()
    {
        source.Play();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
