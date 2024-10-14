using UnityEngine;

[RequireComponent(typeof(InternetReachabilityVerifier))]
public class SkeletonIRVExample : MonoBehaviour
{
	private InternetReachabilityVerifier internetReachabilityVerifier;

	private bool isNetVerified()
	{
		return internetReachabilityVerifier.status == InternetReachabilityVerifier.Status.NetVerified;
	}

	private void forceReverification()
	{
		internetReachabilityVerifier.status = InternetReachabilityVerifier.Status.PendingVerification;
	}

	private void netStatusChanged(InternetReachabilityVerifier.Status newStatus)
	{
	}

	private void Start()
	{
		internetReachabilityVerifier = GetComponent<InternetReachabilityVerifier>();
		internetReachabilityVerifier.statusChangedDelegate += netStatusChanged;
	}
}
