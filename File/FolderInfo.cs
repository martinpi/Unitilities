/*
The MIT License

Copyright (c) 2015 Martin Pichlmair

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

using UnityEngine;
using System.Collections;
using System.IO;

namespace Utils {
	public class FolderInfo {

		/* based on http://forum.unity3d.com/threads/how-to-get-list-of-assets-at-asset-path.18898/ */
		/* usage: Object[] objects = Utils.GetObjectsAtPath<AudioClip>("/Path/To/Music"); */
		public static T[] GetObjectsAtPath<T>(string path) {
			
			ArrayList al = new ArrayList();
			string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
			
			foreach (string fileName in fileEntries)
			{
				int assetPathIndex = fileName.IndexOf("Resources");
				string localPath = fileName.Substring(assetPathIndex);

				Object t = UnityEditor.AssetDatabase.LoadAssetAtPath(localPath, typeof(T));
				
				if (t != null) {
					al.Add(t);
				}
			}
			T[] result = new T[al.Count];
			for (int i = 0; i < al.Count; i++)
				result[i] = (T)al[i];
			
			return result;
		}

		public static string[] ListObjectsAtPath(string path, string extension) {
			ArrayList al = new ArrayList();
			string fullPath = Application.dataPath + "/" + path;
			string[] fileEntries = Directory.GetFiles(fullPath);
			
			foreach (string fileName in fileEntries) {
				string localPath = fileName.Substring(fullPath.Length+1);
				if (localPath.EndsWith(extension))
					al.Add(localPath);
			}
			string[] result = new string[al.Count];
			for (int i = 0; i < al.Count; i++)
				result[i] = (string)al[i];
			
			return result;
		}
	}
}

