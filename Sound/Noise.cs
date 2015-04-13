using UnityEngine;
using System;

public class Noise : MonoBehaviour
{
  private System.Random RandomNumber = new System.Random();
  public float offset = 0.3f;

  void OnAudioFilterRead(float[] data, int channels)
  {
    for (int i = 0; i < data.Length; i++)
    {
      data[i] =  offset -1.0f + (float)RandomNumber.NextDouble()*2.0f;
    }
  }
}