using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class FooClass : MonoBehaviour {
	private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
	GUITexture BackgroundTexture;

	// Use this for initialization
	void Start ()
    {
        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        WebCamDevice[] devices = WebCamTexture.devices;
		webCamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webCamTexture;
		webCamTexture.Play();
		Debug.Log("Initialize");
		BackgroundTexture = gameObject.AddComponent<GUITexture>();
		BackgroundTexture.pixelInset = new Rect(0,0,Screen.width,Screen.height);
		//set up camera
		string backCamName="";
		for( int i = 0 ; i < devices.Length ; i++ ) {
			Debug.Log("Device:"+devices[i].name+ "IS FRONT FACING:"+devices[i].isFrontFacing);

			if (!devices[i].isFrontFacing) {
				backCamName = devices[i].name;
			}
		}

		webCamTexture = new WebCamTexture(backCamName,10000,10000,30);
		webCamTexture.Play();
		BackgroundTexture.texture = webCamTexture;
		//GetComponent<RawImage> ().texture = webCam;
	}
	
	// Update is called once per frame
	void Update () {
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.
		var cube = GameObject.Find ("Cube") as GameObject;


		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.touches [0];
                
			//Check if the phase of that touch equals Began
			if (myTouch.phase == TouchPhase.Began) {
				//If so, set touchOrigin to the position of that touch
				touchOrigin = myTouch.position;
			}
                
                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
			else if ((myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary)) {
				//Set touchEnd to equal the position of this touch
				Vector2 touchEnd = myTouch.position;
                    
				//Calculate the difference between the beginning and end of the touch on the x axis.
				float x = touchEnd.x - touchOrigin.x;
                    
				//Calculate the difference between the beginning and end of the touch on the y axis.
				float y = touchEnd.y - touchOrigin.y;
                    
				//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
				//touchOrigin.x = -1;
                    
				//Check if the difference along the x axis is greater than the difference along the y axis.
				if (Mathf.Abs (x) > Mathf.Abs (y))
                        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                        horizontal = x > 0 ? 1 : -1;
				else
                        //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                        vertical = y > 0 ? 1 : -1;
			} else if(myTouch.phase == TouchPhase.Ended) {
				if (myTouch.tapCount > 2) {
					webCamTexture.Stop ();
					SceneManager.LoadScene ("Edit");
				} else {
					SaveImage ();
				}
			}
		}
            
		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			if (vertical != 0) {
				cube.transform.Translate (Vector3.back * vertical * Time.deltaTime);
			}
			if (horizontal != 0) {
				cube.transform.Translate (Vector3.left * horizontal * Time.deltaTime);
			}

			//AttemptMove<Wall> (horizontal, vertical);
		}
	}

	WebCamTexture webCam;
	WebCamTexture webCamTexture;
	void SaveImage()
	{
		Texture2D destTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);

		Color[] textureData = webCamTexture.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();
		byte[] pngData = destTexture.EncodeToPNG();
		string absolutePath = Application.persistentDataPath;
		//if(File.Exists(Application.persistentDataPath+"/capturedPic2.png"))
		//if(File.Exists("WebcamSnaps" + "photo.png"))
		if(File.Exists(absolutePath + "surveillanceCapture01.png"))
		{
			//File.Delete(Application.persistentDataPath+"/capturedPic2.png");
			//File.Delete("WebcamSnaps" + "photo.png");
			File.Delete(absolutePath + "surveillanceCapture01.png");
		}
		//File.WriteAllBytes(Application.persistentDataPath+"/capturedPic2.png",pngData);
		//File.WriteAllBytes("WebcamSnaps" + "photo.png",pngData);
		Debug.Log("File Saved to " + absolutePath + "surveillanceCapture01.png");
		File.WriteAllBytes(absolutePath + "surveillanceCapture01.png", pngData);
		//Debug.Log("pic saved to"+Application.persistentDataPath);

		//Debug.Log("WebcamSnaps");
		Debug.Log("File Saved to Desktop/Surveillance/CamCapture/");

	}

	void LoadImage()
	{
		/*
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture.
		Texture2D texture = null;
		texture.SetPixels*/
	}
}
