using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Test : MonoBehaviour
{
    public int m_score;


    public void Start()
    {
        Debug.Log("Test script started");
    }
    public void Update()
    {
        m_score++;
        Debug.Log("Score: " + m_score);
    }
}