using UnityEngine;

public class TestAnimator : MonoBehaviour
{
	public Animator[] testAnimators;

	private void Start()
	{
	}

	private void Update()
	{
		Animator[] array = testAnimators;
		foreach (Animator animator in array)
		{
			if (animator != null)
			{
				if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha1))
				{
					SetAnimationClick(animator);
				}
				else if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha2))
				{
					SetAnimationHint(animator, active: true);
				}
				else if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha3))
				{
					SetAnimationBounce(animator);
				}
				else if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha4))
				{
					SetAnimationHint(animator, active: false);
				}
				else if (UnityEngine.Input.GetKeyUp(KeyCode.Alpha5))
				{
					animator.Play("Idle");
				}
			}
		}
	}

	private void SetAnimationClick(Animator ani)
	{
		ani.SetTrigger("setClick");
	}

	private void SetAnimationHint(Animator ani, bool active)
	{
		ani.SetBool("isHint", active);
	}

	private void SetAnimationBounce(Animator ani)
	{
		ani.SetTrigger("setBounce");
	}
}
