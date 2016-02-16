using UnityEngine;
using System.Collections;

namespace Unitilities {
	public class FileUploader {

		public IEnumerator UploadFileCo(string localFileName, string uploadURL, string mimeType = "text/plain")
		{
			WWW localFile = new WWW("file:///" + localFileName);
			yield return localFile;
			if (localFile.error == null)
				Debug.Log("Loaded file successfully");
			else
			{
				Debug.Log("Open file error: "+localFile.error);
				yield break; // stop the coroutine here
			}
			WWWForm postForm = new WWWForm();
			// version 1
			//postForm.AddBinaryData("theFile",localFile.bytes);
			// version 2
			postForm.AddBinaryData("theFile",localFile.bytes, localFileName, mimeType);
			WWW upload = new WWW(uploadURL,postForm);        
			yield return upload;
			if (upload.error == null)
				Debug.Log("upload done :" + upload.text);
			else
				Debug.Log("Error during upload: " + upload.error);
		}


	}
}

public class UploadFile : MonoBehaviour {
	readonly Unitilities.FileUploader _uploader = new Unitilities.FileUploader();

	public void Upload(string localFileName, string uploadURL, string mimeType) {
		StartCoroutine(_uploader.UploadFileCo(localFileName, uploadURL, mimeType));
	}
	public void UploadPNG(string localFileName, string uploadURL) {
		StartCoroutine(_uploader.UploadFileCo(localFileName, uploadURL, "image/png"));
	}
	public void UploadText(string localFileName, string uploadURL) {
		StartCoroutine(_uploader.UploadFileCo(localFileName, uploadURL));
	}
}


