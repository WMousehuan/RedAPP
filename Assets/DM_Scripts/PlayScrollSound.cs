using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayScrollSound : MonoBehaviour, IBeginDragHandler, IEventSystemHandler
{
	private Coroutine curCoroutine;

	private ScrollRect scrollRect;

	[Range(10f, 300f)]
	public int speed = 100;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (curCoroutine != null)
		{
			StopCoroutine(curCoroutine);
		}
		curCoroutine = StartCoroutine(PlaySound());
	}

	private void Start()
	{
		scrollRect = GetComponent<ScrollRect>();
	}

	private IEnumerator PlaySound()
	{
		while (true)
		{
			Vector2 velocity = scrollRect.velocity;
			if (Mathf.Abs((int)velocity.y) > 0)
			{
				SoundSFX.Play(SFXIndex.Scroll);
				float num = speed;
				Vector2 velocity2 = scrollRect.velocity;
				yield return new WaitForSeconds(num / Mathf.Abs(velocity2.y));
			}
			else
			{
				yield return null;
			}
		}
	}
}
