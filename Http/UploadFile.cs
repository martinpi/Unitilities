using UnityEngine;
using System.Collections;

/*
 * EXAMPLE CODE – Uploading a screenshot
 * 
	int _screenshotNumber = 0;
	IEnumerator TakeRandomScreenshot(float minDelay) {
		float delay = minDelay + Random.value * minDelay;
		yield return new WaitForSeconds(delay);

		string devicename = SystemInfo.deviceModel + "_" + SystemInfo.deviceName;
		devicename = devicename.Replace(',','_').Replace('/','_').Replace('\\','_').Replace('.','_').Replace(' ','_');

		string filename = "SZ_"+devicename+"_"+_score+".png";
		string filepath = filename;
		#if UNITY_EDITOR
		filepath = Application.persistentDataPath+"/"+filepath;
		#endif

		Application.CaptureScreenshot(filepath);

		// wait for screenshot to be stored
		yield return new WaitForSeconds(5f);
		if (UploadScreenshots)
			GetComponent<UploadFile>().UploadPNG(Application.persistentDataPath+"/"+filename,"http://brokenrul.es/pi/SUMZERO/upload.php");
		
//		Debug.Log("Screenshot "+_screenshotNumber+" taken");

		_screenshotNumber++;
		StartCoroutine(TakeRandomScreenshot(minDelay));
	}
}
*/

namespace Unitilities {
	public class FileUploader {

		public IEnumerator UploadFileCo(string localFileName, string uploadURL, string mimeType = "text/plain")
		{
			WWW localFile = new WWW("file:///" + localFileName);
			yield return localFile;
			if (localFile.error != null) {
				Debug.Log("Open file error: "+localFile.error);
				yield break; // stop the coroutine here
			}
			WWWForm postForm = new WWWForm();
			postForm.AddBinaryData("theFile",localFile.bytes, localFileName, mimeType);
			WWW upload = new WWW(uploadURL,postForm);        
			yield return upload;
			if (upload.error != null)
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


