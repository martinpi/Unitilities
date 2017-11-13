// Copyright (c) 2016 robosoup
// www.robosoup.com

using UnityEngine;
using System.Collections.Generic;
using Unitilities;

namespace Unitilities.Boids
{
    public class Swarm
    {
		public float sight = 200f;
		public float speed = 12f;
		public float flockDistance = 3f;
		public float huntDistance = 10f;
		public float collisionSize = 1f;
		public float agility = 0.7f;
		public Rect cameraArea;
		public Rect swarmArea;
		public int killed = 0;

        public List<Boid> Boids = new List<Boid>();
		public static List<Boid> AllBoids = new List<Boid>();

		public Vector2i GetSector(Vector2 coordinates) {
			// sectors begin in the lower left corner. the whole swarm area is a 9-patch (3x3) - with rim it's 5x5
			return new Vector2i(
				(int) ((coordinates.x - swarmArea.xMin) / swarmArea.width/3f),
				(int) ((coordinates.y - swarmArea.yMin) / swarmArea.height/3f)
			);
		}

		public List<Boid> BoidsAroundSector(Vector2i sector) {
			List<Boid> boids = new List<Boid>();

			Vector2i boidSector;
			for (var i = 0; i < AllBoids.Count; i++) {
				AllBoids[i].sector = boidSector = GetSector(AllBoids[i].pos);
				if (boidSector.x >= (sector.x-1 + 3) % 3 && boidSector.x <= (sector.x+1) % 3 && 
					boidSector.y >= (sector.y-1 + 3) % 3 && boidSector.y <= (sector.y+1) % 3 ) {

					boids.Add(AllBoids[i]);
				}
			}

			return boids;
		}

        public Swarm(float hunterPercentage, Rect area, int number, int type)
        {
			swarmArea = area;

			for (var i = 0; i < number; i++) {
				Boid b = new Boid(this, i<(number * (int)(100f * hunterPercentage)/100), area, type);
				Boids.Add(b);
				AllBoids.Add(b);
			}
        }

		public void RegisterSwarm() {
			for (var i = 0; i < Boids.Count; i++) {
				AllBoids.Add(Boids[i]);
			}
		}

		public static void MoveAllBoids(Vector2 target, float deltaTime, float cameraSize)
		{
			foreach (var boid in AllBoids) {
				boid.Move(AllBoids,target,deltaTime,cameraSize);
//				boid.Respawn();
			}
		}

    }
}
