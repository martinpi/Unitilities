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

[ExecuteInEditMode]
public class FlowGrid : MonoBehaviour {

	public int Width;
	public int Height;
	public bool FourWay = false;
	public Vector2Int Target = Vector2Int.zero;
	public bool Dirty = true;

	private Vector2[,] _flowField;
	private int[,] _djikstraField;

	void OnEnable() {
		_flowField = new Vector2[Width, Height];
		_djikstraField = new int[Width, Height];
		Reset();
	
		if (Target!=Vector2Int.zero) Recalculate();
	}

	public void Randomize() {
		Reset();
		addRandomWalls((Width + Height)*2 );

		Target = new Vector2Int(
			1+(int)(Random.value*((float)Width-2)), 
			1+(int)(Random.value*((float)Height-2)));

		generateFlowField(Target);
	}

	public void Recalculate() {
		// TODO: we can make updating asyncronous later
		Dirty = true;
		generateFlowField(Target);
		Dirty = false;
	}

	public void Reset() {
		for (int x=0; x<Width; ++x) {
			for (int y=0; y<Height; ++y) {
				_flowField[x,y] = Vector2.zero;
				_djikstraField[x,y] = -1;
			}
		}
	}

	void OnDrawGizmosSelected() {
    	Gizmos.color = Color.white;
		for (int x=0; x<Width; ++x) {
			for (int y=0; y<Height; ++y) {

				if (_djikstraField[x, y] >= 0) {
					
			    	Gizmos.color = new Color(1f, 1f, 1f, 0.7f);
            		Gizmos.DrawLine( 
						transform.position + new Vector3(x,y,0f), 
						transform.position + new Vector3(
						(((float)x)+_flowField[x,y].x*0.5f),
						(((float)y)+_flowField[x,y].y*0.5f),0f) );

			    	Gizmos.color = new Color(1f/(((float)_djikstraField[x, y]/5f)+1f), 1f/((float)_djikstraField[x, y]+1f), 1f, 0.7f);
					Gizmos.DrawSphere( new Vector3(x,y,0f), 0.2f );


				}
			}
        }
    }
	
	private List<Vector2Int> allNeighboursOf(Vector2Int pos, bool noDiagonals = false) {
		List<Vector2Int> neighbours = new List<Vector2Int>();
		int x = pos.x;
		int y = pos.y;

		if (x>0 && y>0 && !noDiagonals) neighbours.Add(new Vector2Int(x-1, y-1));
		if (y>0) neighbours.Add(new Vector2Int(x, y-1));
		if (x<Width-1 && y>0 && !noDiagonals) neighbours.Add(new Vector2Int(x+1, y-1));
		if (x<Width-1) neighbours.Add(new Vector2Int(x+1, y));
		if (x<Width-1 && y<Height-1 && !noDiagonals) neighbours.Add(new Vector2Int(x+1, y+1));
		if (y<Height-1) neighbours.Add(new Vector2Int(x, y+1));
		if (x>0 && y<Height-1 && !noDiagonals) neighbours.Add(new Vector2Int(x-1, y+1));
		if (x>0) neighbours.Add(new Vector2Int(x-1, y));

		return neighbours;
	}

	void calcDistance(Vector2Int pos, int distance) {
		var neighbours = allNeighboursOf(pos, true);

		for (var j = 0; j < neighbours.Count; j++) {
			var n = neighbours[j];

			if (_djikstraField[n.x, n.y] == -1 || _djikstraField[n.x, n.y] > distance) {
				_djikstraField[n.x, n.y] = distance;
 				calcDistance(n, distance + 1);
			}
		}
	}

	void setWall(int x, int y, int value) {
		_djikstraField[ x, y ] = value;
	}

	public Vector3Int getGridPosition( Vector3 worldPosition ) {
		Vector3 localPosition = transform.worldToLocalMatrix * worldPosition;
		Vector3Int gridPosition = new Vector3Int((int)localPosition.x, (int)localPosition.y, (int)localPosition.z);
		gridPosition.x = Mathi.Clamp( gridPosition.x, 0, Width-1 );
		gridPosition.y = Mathi.Clamp( gridPosition.y, 0, Height-1 );
		return gridPosition;
	}

	public Vector3 getWorldPosition( Vector3Int gridPosition ) {
		gridPosition.x = Mathi.Clamp( gridPosition.x, 0, Width-1 );
		gridPosition.y = Mathi.Clamp( gridPosition.y, 0, Height-1 );		
		return transform.localToWorldMatrix * new Vector3( gridPosition.x, gridPosition.y, gridPosition.z );		
	}

	public void setWall( Vector3 worldPosition, bool wall ) {
		Vector3Int gridPosition = getGridPosition(worldPosition);
		setWall( gridPosition.x, gridPosition.y, wall ? int.MinValue : -1 );
	}
	public void setWall( Vector3Int gridPosition, bool wall ) {
		setWall( gridPosition.x, gridPosition.y, wall ? int.MinValue : -1 );
	}

	public bool isWall ( Vector3Int gridPosition ) {
		return _djikstraField[ gridPosition.x, gridPosition.y ] == int.MinValue;
	}

	void addRandomWalls(int count) { 
		for (int i=0; i<count; ++i)
			setWall( (int)(Random.value * (float)Width), (int)(Random.value*(float)Height), int.MinValue );
	}

	void generateFlowField(Vector2Int target) {

		//flood fill out from the end point
		_djikstraField[target.x,target.y] = 0;
		calcDistance(target, 1);

		//for each grid square
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {

				//Obstacles have no flow value
				if (_djikstraField[x,y] == int.MinValue) {
					continue;
				}

				var pos = new Vector2Int(x, y);
				var neighbours = allNeighboursOf(pos, FourWay);

				//Go through all neighbours and find the one with the lowest distance
				Vector2Int min = new Vector2Int(-1,-1);
				var minDist = 0;
				for (var i = 0; i < neighbours.Count; i++) {
					var n = neighbours[i];
					var dist = _djikstraField[n.x, n.y] - _djikstraField[pos.x, pos.y];

					if (dist < minDist) {
						min = n;
						minDist = dist;
					}
				}

				//If we found a valid neighbour, point in its direction
				if (min.x >=0 && min.y >= 0) {
					_flowField[x,y] = new Vector2(min.x-pos.x, min.y-pos.y).normalized; //min.minus(pos).normalize();
				}
			}
		}
	}

	public float getInterpolatedDistance(Vector3 worldPosition) {

		Vector3 localPosition = transform.worldToLocalMatrix * worldPosition;
		Vector3Int gridPosition = getGridPosition(worldPosition);

		// we can't interpolate the areas at the borders
		gridPosition.x = Mathi.Clamp( gridPosition.x, 0, Width-2 );
		gridPosition.y = Mathi.Clamp( gridPosition.y, 0, Height-2 );

		//The 4 weights we'll interpolate, see http://en.wikipedia.org/wiki/File:Bilininterp.png for the coordinates
		float f00 = (float)_djikstraField[ gridPosition.x, gridPosition.y ];
		float f01 = (float)_djikstraField[ gridPosition.x, gridPosition.y + 1 ];
		float f10 = (float)_djikstraField[ gridPosition.x + 1, gridPosition.y ];
		float f11 = (float)_djikstraField[ gridPosition.x + 1, gridPosition.y + 1 ];

		//Do the x interpolations
		float xWeight = localPosition.x - (float)gridPosition.x;

		var top = f00 * (1f - xWeight) + f10 * xWeight;
		var bottom = f01 * (1f - xWeight) + f11 * xWeight;

		//Do the y interpolation
		float yWeight = localPosition.y - (float)gridPosition.y;

		//This is now the direction we want to be travelling in (needs to be normalized)
		return (top * (1f - yWeight) + bottom * yWeight);
	}


	public Vector3 getInterpolatedForces(Vector3 worldPosition) {

		Vector3 localPosition = transform.worldToLocalMatrix * worldPosition;
		Vector3Int gridPosition = getGridPosition(worldPosition);

		// we can't interpolate the areas at the borders
		gridPosition.x = Mathi.Clamp( gridPosition.x, 1, Width-2 );
		gridPosition.y = Mathi.Clamp( gridPosition.y, 1, Height-2 );

		//The 4 weights we'll interpolate, see http://en.wikipedia.org/wiki/File:Bilininterp.png for the coordinates
		var f00 = _flowField[ gridPosition.x, gridPosition.y ];
		var f01 = _flowField[ gridPosition.x, gridPosition.y + 1 ];
		var f10 = _flowField[ gridPosition.x + 1, gridPosition.y ];
		var f11 = _flowField[ gridPosition.x + 1, gridPosition.y + 1 ];

		//Do the x interpolations
		float xWeight = localPosition.x - (float)gridPosition.x;

		var top = f00 * (1f - xWeight) + f10 * xWeight;
		var bottom = f01 * (1f - xWeight) + f11 * xWeight;

		//Do the y interpolation
		float yWeight = localPosition.y - (float)gridPosition.y;

		//This is now the direction we want to be travelling in (needs to be normalized)
		return (top * (1f - yWeight) + bottom * yWeight).normalized;
	}

}
