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

        public Swarm(float hunterPercentage, Rect area, int number, int type)
        {
			swarmArea = area;

			for (var i = 0; i < number; i++) {
				Boid b = new Boid(this, i<(number * (int)(100f * hunterPercentage)/100), area, type);
				Boids.Add(b);
				AllBoids.Add(b);
			}
        }

//        public void MoveBoids(Vector2 target, float deltaTime)
//        {
//			foreach (var boid in Boids) {
//				boid.Respawn();
//				boid.Move(Boids,target,deltaTime);
//			}
//        }

		public static void MoveAllBoids(Vector2 target, float deltaTime, float cameraSize)
		{
			foreach (var boid in AllBoids) {
				boid.Respawn();
				boid.Move(AllBoids,target,deltaTime,cameraSize);
			}
		}

    }
}
