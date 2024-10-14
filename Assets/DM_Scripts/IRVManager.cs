public class IRVManager : MonoSingleton<IRVManager>
{
	public InternetReachabilityVerifier.Status CurrentNetStatus => InternetReachabilityVerifier.Status.NetVerified;

	private void Start()
	{
	}

	public void CheckCurrentNetStatus()
	{
	}
}
