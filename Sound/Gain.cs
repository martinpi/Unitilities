using UnityEngine;
using System;

public class Gain : MonoBehaviour {
 	public float Level = 0.3f;

 	void OnAudioFilterRead(float[] data, int channels) {
    for (int i = 0; i < data.Length; i++)
		data[i] =  data[i] * Level;
	}
}