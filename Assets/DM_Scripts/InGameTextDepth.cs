using UnityEngine;

public class InGameTextDepth : MonoBehaviour
{
	public Sprite[] SpritesNumberFont;

	public SpriteRenderer SRCurrentNumber;

	public SpriteRenderer SRMaxNumber;

	public void SetNumber(int currentNumber, int maxNumber)
	{
		SRCurrentNumber.sprite = SpritesNumberFont[currentNumber - 1];
		SRMaxNumber.sprite = SpritesNumberFont[maxNumber - 1];
	}
}
