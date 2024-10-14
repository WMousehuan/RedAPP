using UnityEngine;

public class RotationCheckGameCamera : MonoBehaviour
{
	private void Update()
	{
		if (Camera.main.orthographicSize != 5.7f && (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown || Input.deviceOrientation == DeviceOrientation.FaceUp))
		{
			Camera.main.orthographicSize = 5.7f;
		}
		else if (Camera.main.orthographicSize == 5.7f && (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight))
		{
			Camera.main.orthographicSize = 3.4f;
		}
	}
}
