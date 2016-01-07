using UnityEngine;
using System.Collections;

public class VoxelChunk
{
	float[,,] m_voxels;
	Vector3[,,] m_normals;
	Vector3 m_posGrid;
	Vector3 m_pos;
	GameObject m_mesh;
	float m_surfaceLevel;
	uint m_biome = 0;
	uint[] m_neighbourBiomes;

	int[,] m_sampler = new int[,] 
	{
		{1,-1,0}, {1,-1,1}, {0,-1,1}, {-1,-1,1}, {-1,-1,0}, {-1,-1,-1}, {0,-1,-1}, {1,-1,-1}, {0,-1,0},
		{1,0,0}, {1,0,1}, {0,0,1}, {-1,0,1}, {-1,0,0}, {-1,0,-1}, {0,0,-1}, {1,0,-1}, {0,0,0},
		{1,1,0}, {1,1,1}, {0,1,1}, {-1,1,1}, {-1,1,0}, {-1,1,-1}, {0,1,-1}, {1,1,-1}, {0,1,0}
	};

//	float[] dists = new float[3];
//	System.Func<float[], float> combinerFunc = i => i[2] - i[0];

	public VoxelChunk(Vector3 pos, Vector3 offset, int width, int height, int length, float surfaceLevel, uint biome)
	{
		Init(pos, offset, width, height, length, surfaceLevel, biome);
	}

	public void Init(Vector3 pos, Vector3 offset, int width, int height, int length, float surfaceLevel, uint biome)
	{
		m_biome = biome;
		m_surfaceLevel = surfaceLevel;
		//As we need some extra data to smooth the voxels and create the normals we need a extra 5 voxels
		//+1 one to create a seamless mesh. +2 to create smoothed normals and +2 to smooth the voxels
		//This is a little unoptimsed as it means some data is being generated that has alread been generated in other voxel chunks
		//but it is simpler as we dont need to access the data in the other voxels. You could try and copy the data
		//needed in for the other voxel chunks as a optimisation step
		m_voxels = new float[width+5, height+5, length+5];
		//As the extra data is basically a border of voxels around the chunk we need to offset the position
		//It does not need to be done but it means that te voxel position (once translated) matches its world position
		m_pos = pos - new Vector3(2,2,2) + offset;
	}

	public void SetNeighbourBiomes(uint top, uint right, uint bottom, uint left) {
		m_neighbourBiomes = new uint[4];
		m_neighbourBiomes[0] = top;
		m_neighbourBiomes[1] = right;
		m_neighbourBiomes[2] = bottom;
		m_neighbourBiomes[3] = left;
	}
	
	//All of these sample functions create the noise needed for a certain effect, ie caves, moutains etc.
	//There is no reason I have choosen the technique and values for anyone of the noise functions
	//Its just what I have decided looks nice. There is no wrong or right way to do it.
	//The noise functions are one of the slowest to call so I tried to keep it simple while still having nice looking terrain
	
	//The last three values in the noise functions are octaves, frequency and amplitude.
	//More ocatves will create more detail but is slower.
	//Higher/lower frquency will 'strech/shrink' out the noise.
	//Amplitude defines roughly the range of the noise, ie amp = 1.0 means roughly -1.0 to +1.0 * range of noise
	//The range of noise is 0.5 for 1D, 0.75 for 2D and 1.5 for 3D
	
	float SampleMountains(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the mountains. It used something called 
		//domain warping. Domain warping is basically offseting the position used for the noise by
		//another noise value. It tends to create a warped effect that looks nice.
		float w = perlin.FractalNoise2D(x, z, 3, 15.0f, 32.0f);

		//Clamp noise to 0 so mountains only occur where there is a positive value
		//The last value (32.0f) is the amp that defines (roughly) the maximum mountaion height
		//Change this to create high/lower mountains

		return Mathf.Min(0.0f, perlin.FractalNoise2D(x+w, z+w, 6, 120.0f, 32.0f) );
	}
	
	float SampleSpikes(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the mountains. It used something called 
		//domain warping. Domain warping is basically offseting the position used for the noise by
		//another noise value. It tends to create a warped effect that looks nice.
		float w = perlin.FractalNoise2D(x, z, 3, 4.0f, 32.0f);
		
		//Clamp noise to 0 so mountains only occur where there is a positive value
		//The last value (32.0f) is the amp that defines (roughly) the maximum mountaion height
		//Change this to create high/lower mountains
		
		return Mathf.Min(m_surfaceLevel-1f, perlin.FractalNoise2D(x+w, z+w, 6, 320.0f, 64.0f) );
	}
	
	float SampleGround(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the ground.
		//The last value (8.0f) is the amp that defines (roughly) the maximum 
		//and minimum vaule the ground varies from the surface level
		return perlin.FractalNoise2D(x, z, 4, 100.0f, 32.0f);
	}

	float SampleIslands(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the mountains. It used something called 
		//domain warping. Domain warping is basically offseting the position used for the noise by
		//another noise value. It tends to create a warped effect that looks nice.
		float w = perlin.FractalNoise2D(x, z, 2, 5.0f, 32.0f);
		
		//Clamp noise to 0 so mountains only occur where there is a positive value
		//The last value (32.0f) is the amp that defines (roughly) the maximum mountaion height
		//Change this to create high/lower mountains
		
		// Base the min on surface level!
		return Mathf.Min(m_surfaceLevel+1f, perlin.FractalNoise2D(x+w, z+w, 6, 120.0f, 16.0f) );
	}

	float SampleCaves(float x, float y, float z, PerlinNoise perlin)
	{
		//The creates the noise used for the caves. It uses domain warping like the moiuntains
		//to creat long twisting caves.
		float w = perlin.FractalNoise3D(x, y, z, 1, 40.0f, 32.0f);
		//The last vaule is the cave amp and defines the maximum cave diameter. A larger value will create
		//larger caves (A higher frequency will also create larger caves). It is unitless, 1 != 1m
		return Mathf.Abs(perlin.FractalNoise3D(x+w, y*2.0f+w, z+w, 2, 50.0f, 10.0f));
	}
	
	public void CreateVoxels(PerlinNoise surfacePerlin, PerlinNoise cavePerlin) {

		//float startTime = Time.realtimeSinceStartup;
		
		//Creates the data the mesh is created form. Fills m_voxels with values between -1 and 1 where
		//-1 is a soild voxel and 1 is a empty voxel.
		
		int w = m_voxels.GetLength(0);
		int h = m_voxels.GetLength(1);
		int l = m_voxels.GetLength(2);
		
		for(int x = 0; x < w; x++)
		{
			for(int z = 0; z < l; z++)
			{
				//world pos is the voxels position plus the voxel chunks position
				float worldX = x+m_pos.x;
				float worldZ = z+m_pos.z;

				float ht = SampleGround(worldX, worldZ, surfacePerlin);

//					float localBiom = 0f;
//					localBiom += (float)z/h * ((float)m_neighbourBiomes[2] - (float)m_biome);
//					localBiom += (float)x/w * ((float)m_neighbourBiomes[1] - (float)m_biome);
//					localBiom += (1f-(float)z/h) * ((float)m_neighbourBiomes[0]- (float)m_biome);
//					localBiom += (1f-(float)x/w) * ((float)m_neighbourBiomes[3]- (float)m_biome);

//					float factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(x, z, 4, 400.0f, 1.0f) + 0.75f)/1.5f);
//					float it = ht + SampleIslands(worldX, worldZ, surfacePerlin);
//					ht = (ht+m_surfaceLevel+1f) * factor + it * (1f-factor);

//					float factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(x, z, 4, 400.0f, 1.5f) + 0.75f)/1.5f);
//					float it = ht + SampleSpikes(worldX, worldZ, surfacePerlin);
//					ht = it; //(ht-1f) * factor + it * (1f-factor);

			
				/*
				float factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(x, z, 4, 400.0f, 1.0f) + 0.75f)/1.5f);
				float st = SampleSpikes(worldX, worldZ, surfacePerlin);
				ht = (ht+m_surfaceLevel-1f) * factor + (ht + st) * (1f-factor);

				factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(x, z, 4, 150.0f, 1.0f) + 0.75f)/1.5f);
				float it = SampleIslands(worldX, worldZ, surfacePerlin);
				ht = (ht+m_surfaceLevel+1f) * factor + (ht + it) * (1f-factor);

				ht -= m_surfaceLevel;
				ht -= SampleMountains(worldX, worldZ, surfacePerlin);
*/

				VoronoiNoise.SetDistanceToEuclidian();
				VoronoiNoise.SetCombinationTo_D2_D0();
				ht *= VoronoiNoise.FractalNoise2D(worldX, worldZ, 2, 500f, 2f, 6);

				float factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(worldX, worldZ, 4, 400.0f, 1.0f) + 0.9375f)/1.875f);
				float st = SampleSpikes(worldX, worldZ, surfacePerlin);
				ht = (ht+m_surfaceLevel-1f) * factor + (ht + st) * (1f-factor);
				
				factor = Mathf.Clamp01((surfacePerlin.FractalNoise2D(worldX, worldZ, 4, 150.0f, 1.0f) + 0.9375f)/1.875f);
				float it = SampleIslands(worldX, worldZ, surfacePerlin);
				ht = (ht+m_surfaceLevel+1f) * factor + (ht + it) * (1f-factor);

				ht -= m_surfaceLevel*1.5f;
//				ht -= SampleMountains(worldX, worldZ, surfacePerlin);

				for(int y = 0; y < h; y++)
				{
					float worldY = y+m_pos.y-m_surfaceLevel;

					//If we take the heigth value and add the world
					//the voxels will change from positiove to negative where the surface cuts through the voxel chunk
					m_voxels[x,y,z] = Mathf.Clamp(ht + worldY , -1.0f, 1.0f);

					if (m_biome == 15) {
						float caveHt = SampleCaves(worldX, worldY, worldZ, cavePerlin);
						//This fades the voxel value so the caves never appear more than h units from
						//the surface level.
						float fade = 1.0f - Mathf.Clamp01(Mathf.Max(0.0f, worldY)/h);
						
						m_voxels[x,y,z] += caveHt * fade;
						m_voxels[x,y,z] = Mathf.Clamp(m_voxels[x,y,z], -1.0f, 1.0f);
					}
				}
			}
		}
		
		//Debug.Log("Create voxels time = " + (Time.realtimeSinceStartup-startTime).ToString() );
		
	}

	
	public void SmoothVoxels()
	{
		//float startTime = Time.realtimeSinceStartup;
		
		//This averages a voxel with all its neighbours. Its is a optional step
		//but I think it looks nicer. You might what to do a fancier smoothing step
		//like a gaussian blur
		
		int w = m_voxels.GetLength(0);
		int h = m_voxels.GetLength(1);
		int l = m_voxels.GetLength(2);
		
		float[,,] smothedVoxels = new float[w,h,l];
		
		for(int x = 1; x < w-1; x++)
		{
			for(int y = 1; y < h-1; y++)
			{
				for(int z = 1; z < l-1; z++)
				{
					float ht = 0.0f;
					
					for(int i = 0; i < 27; i++)
						ht += m_voxels[x + m_sampler[i,0], y + m_sampler[i,1], z + m_sampler[i,2]];

					smothedVoxels[x,y,z] = ht/27.0f;
				}
			}
		}
		
		m_voxels = smothedVoxels;
		
		//Debug.Log("Smooth voxels time = " + (Time.realtimeSinceStartup-startTime).ToString() );
	}
	
	public void CalculateNormals()
	{
		//float startTime = Time.realtimeSinceStartup;
		
		//This calculates the normal of each voxel. If you have a 3d array of data
		//the normal is the derivitive of the x, y and z axis.
		//Normally you need to flip the normal (*-1) but it is not needed in this case.
		//If you dont call this function the normals that Unity generates for a mesh are used.
		
		int w = m_voxels.GetLength(0);
		int h = m_voxels.GetLength(1);
		int l = m_voxels.GetLength(2);
		
		if(m_normals == null) m_normals = new Vector3[w,h,l];
		
		for(int x = 2; x < w-2; x++)
		{
			for(int y = 2; y < h-2; y++)
			{
				for(int z = 2; z < l-2; z++)
				{
					float dx = m_voxels[x+1,y,z] - m_voxels[x-1,y,z];
					float dy = m_voxels[x,y+1,z] - m_voxels[x,y-1,z];
					float dz = m_voxels[x,y,z+1] - m_voxels[x,y,z-1];
					
					m_normals[x,y,z] = Vector3.Normalize(new Vector3(dx,dy,dz));
				}
			}
		}
		
		//Debug.Log("Calculate normals time = " + (Time.realtimeSinceStartup-startTime).ToString() );
		
	}
	
	Vector3 TriLinearInterpNormal(Vector3 pos)
	{	
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;
		
		float fx = pos.x-x;
		float fy = pos.y-y;
		float fz = pos.z-z;
		
		Vector3 x0 = m_normals[x,y,z] * (1.0f-fx) + m_normals[x+1,y,z] * fx;
		Vector3 x1 = m_normals[x,y,z+1] * (1.0f-fx) + m_normals[x+1,y,z+1] * fx;
		
		Vector3 x2 = m_normals[x,y+1,z] * (1.0f-fx) + m_normals[x+1,y+1,z] * fx;
		Vector3 x3 = m_normals[x,y+1,z+1] * (1.0f-fx) + m_normals[x+1,y+1,z+1] * fx;
		
		Vector3 z0 = x0 * (1.0f-fz) + x1 * fz;
		Vector3 z1 = x2 * (1.0f-fz) + x3 * fz;
		
		return z0 * (1.0f-fy) + z1 * fy;
	}

	public void CreateMesh(Material mat, GameObject obj)
	{
//		float startTime = Time.realtimeSinceStartup;
		
		Mesh mesh = MarchingCubes.CreateMesh(m_voxels,2,2);
		if(mesh == null) return;
		
		int size = mesh.vertices.Length;
		
		if(m_normals != null)
		{
			Vector3[] normals = new Vector3[size];
			Vector3[] verts = mesh.vertices;
			
			//Each verts in the mesh generated is its position in the voxel array
			//and you can use this to find what the normal at this position.
			//The verts are not at whole numbers though so you need to use trilinear interpolation
			//to find the normal for that position
			
			for(int i = 0; i < size; i++)
				normals[i] = TriLinearInterpNormal(verts[i]);
			
			mesh.normals = normals;
		}
		else
		{
			mesh.RecalculateNormals();
		}
		
		Color[] control = new Color[size];
		Vector3[] meshNormals = mesh.normals;
			
		for(int i = 0; i < size; i++)
		{
			//This creates a control map used to texture the mesh based on the slope
			//of the vert. Its very basic and if you modify how this works yoou will
			//you will probably need to modify the shader as well.
			float dpUp = Vector3.Dot(meshNormals[i], Vector3.up);
			
			//Red channel is the sand on flat areas
			float R = (Mathf.Max(0.0f, dpUp) < 0.8f) ? 0.0f : 1.0f;
			//Green channel is the gravel on the sloped areas
			float G = Mathf.Pow(Mathf.Abs(dpUp), 2.0f);
			
			//Whats left end up being the rock face on the vertical areas

			control[i] = new Color(R,G,0,0);
		}
		
		//May as well store in colors 
		mesh.colors = control;
		
//		m_mesh = new GameObject("Voxel Mesh " + m_pos.x.ToString() + " " + m_pos.y.ToString() + " " + m_pos.z.ToString());
//		m_mesh.AddComponent<MeshFilter>();
//		m_mesh.AddComponent<MeshRenderer>();
//		m_mesh.AddComponent<MeshCollider>();

		m_mesh = obj;
		m_mesh.name = "Voxel Mesh " + m_pos.x.ToString() + " " + m_pos.y.ToString() + " " + m_pos.z.ToString();
		m_mesh.GetComponent<Renderer>().material = mat;
		m_mesh.GetComponent<MeshFilter>().mesh = mesh;
		m_mesh.transform.localPosition = m_pos;

//		Debug.Log("Create mesh time = " + (Time.realtimeSinceStartup-startTime).ToString() );
	}

	public void CreateCollider() {
		m_mesh.GetComponent<MeshCollider>().sharedMesh = m_mesh.GetComponent<MeshFilter>().mesh;
	}

	public GameObject GetMesh() {
		return m_mesh;
	}
}
