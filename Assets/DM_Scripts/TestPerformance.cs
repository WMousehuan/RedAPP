using UnityEngine;
using UnityEngine.UI;

public class TestPerformance : MonoBehaviour
{
	public Font[] test;

	public string testJson;

	public Text testLabel;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha1))
		{
			testLabel.font = test[0];
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha2))
		{
			testLabel.font = test[1];
		}
	}
}
