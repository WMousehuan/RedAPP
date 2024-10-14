using UnityEngine;
using UnityEngine.UI;

public class UpdateCoin : MonoBehaviour
{
	private Text text;

	private void Start()
	{
		text = GetComponent<Text>();
	}

	private void LateUpdate()
	{
		if ((bool)text && !UIManager.holdOnUpdateCoin)
		{
			text.text = RedPackageAuthor.Instance.userBalance.ToString();// Utils.GetCurrencyNumberString(MonoSingleton<PlayerDataManager>.Instance.Coin);
            //text.text =  Utils.GetCurrencyNumberString(MonoSingleton<PlayerDataManager>.Instance.Coin);
        }
	}
}
