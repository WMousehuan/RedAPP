using UnityEngine;

public class BannerManager : MonoSingleton<BannerManager>
{
	private BannerList m_BannerList;

	public Transform ParentPopupGroup;

	[HideInInspector]
	public int OpenedBannerCount;

	[SerializeField]
	private GameObject PrefabBannerList;

	private void Start()
	{
		GameObject gameObject = Object.Instantiate(PrefabBannerList);
		gameObject.transform.SetParent(base.transform);
		m_BannerList = gameObject.GetComponent<BannerList>();
	}

	//public Banner Open(BannerType type)
	//{
	//	OpenedBannerCount++;
	//	GameObject banner = m_BannerList.GetBanner(type);
	//	GameObject gameObject = Object.Instantiate(banner);
	//	gameObject.transform.SetParent(ParentPopupGroup, worldPositionStays: false);
	//	gameObject.transform.localPosition = Vector3.zero;
	//	return gameObject.GetComponent<Banner>();
	//}

	public void Close()
	{
		OpenedBannerCount--;
	}
}
