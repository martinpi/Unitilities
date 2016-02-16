using UnityEngine;
using System.IO;

namespace Unitilities {
	namespace Screenshot {
		public static class Screenshooter {
			public static Texture2D TakeScreenshot() {
				return TakeScreenshot(Screen.width, Screen.height, Camera.main);
			}
			public static Texture2D SaveScreenshot(int number) {
				return TakeScreenshot(Screen.width, Screen.height, Camera.main, "Screenshot"+number+".png");
			}

			public static Texture2D TakeScreenshot(int width, int height, 
				Camera screenshotCamera) {
				if(width<=0 || height<=0) return null;
				if(screenshotCamera == null) screenshotCamera = Camera.main;

				Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
				RenderTexture renderTex = new RenderTexture(width, height, 24);
				screenshotCamera.targetTexture = renderTex;
				screenshotCamera.Render();
				RenderTexture.active = renderTex;
				screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
				screenshot.Apply(false);
				screenshotCamera.targetTexture = null;
				RenderTexture.active = null;
				renderTex.Release();
				Object.Destroy(renderTex);
				return screenshot;
			}	

			public static Texture2D TakeScreenshot(int width, int height, 
				Camera screenshotCamera, string saveToFileName) {
				Texture2D screenshot = TakeScreenshot(width, height, screenshotCamera);
				if(screenshot != null && saveToFileName!=null) {
					if(Application.platform==RuntimePlatform.OSXPlayer || 
						Application.platform==RuntimePlatform.WindowsPlayer && 
						Application.platform!=RuntimePlatform.LinuxPlayer 
						|| Application.isEditor) {
						byte[] bytes;
						if(saveToFileName.ToLower().EndsWith(".jpg"))
							bytes = screenshot.EncodeToJPG();
						else 
							bytes = screenshot.EncodeToPNG();

						FileStream fs = new FileStream(saveToFileName, FileMode.OpenOrCreate);
						BinaryWriter w = new BinaryWriter(fs);
						w.Write(bytes);
						w.Close();
						fs.Close();
					}
				}
				return screenshot;
			}
		}
	}
}

