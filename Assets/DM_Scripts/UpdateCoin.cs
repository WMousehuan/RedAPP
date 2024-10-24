using UnityEngine;
using UnityEngine.UI;

public class UpdateCoin : MonoBehaviour
{
	private Text text;

	private void Start()
	{
		text = GetComponent<Text>();
		EventManager.Instance.Regist(GameType.CoinUpdate.ToString(), this.GetInstanceID(), (objects) => {
            text.text = RedPackageAuthor.Instance.userBalance.ToString();
        });
	}
    private void OnDestroy()
    {
		EventManager.Instance.UnRegist(GameType.CoinUpdate.ToString(), this.GetInstanceID());
    }
    private void LateUpdate()
	{
		//if ((bool)text && !UIManager.holdOnUpdateCoin)
		//{
		//	text.text = RedPackageAuthor.Instance.userBalance.ToString();// Utils.GetCurrencyNumberString(MonoSingleton<PlayerDataManager>.Instance.Coin);
  //          //text.text =  Utils.GetCurrencyNumberString(MonoSingleton<PlayerDataManager>.Instance.Coin);
  //      }
	}
}
