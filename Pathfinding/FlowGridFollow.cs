﻿/*
The MIT License

Copyright (c) 2018 Martin Pichlmair

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowGridFollow : MonoBehaviour {

	public FlowGrid FlowGrid;
	public float Force = 1f;
	public bool RandomStartPosition = true;

	private bool _active = false;
	private Rigidbody2D _body2D;

	void Start () {
		_body2D = GetComponent<Rigidbody2D>();
		StartCoroutine(StartMoving(2f));
		if (RandomStartPosition) {
			Vector2 pos = Vector2.zero;
			pos.x = ((float)FlowGrid.Width) * Random.value;
			pos.y = ((float)FlowGrid.Height) * Random.value;
			// _body2D.MovePosition(pos);
			transform.position = pos.Vector3XY();
		}
	}
	
	void Update () {
		if (!_active) return;

		Vector3 dir = FlowGrid.getInterpolatedForces(transform.position);
		_body2D.AddForce(Force * dir.Vector2XY());
	}

	IEnumerator StartMoving(float delay) {
		yield return new WaitForSeconds(delay);
		_active = true;

	}

}
