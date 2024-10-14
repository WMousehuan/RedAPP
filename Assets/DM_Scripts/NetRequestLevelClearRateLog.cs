public class NetRequestLevelClearRateLog : NetRequestBase
{
	public static void RequestLevelStart(int gid)
	{
		ReqPacketLevelClearRateLog reqPacketLevelClearRateLog = new ReqPacketLevelClearRateLog();
		reqPacketLevelClearRateLog.gid = gid;
		MonoSingleton<NetworkManager>.Instance.StartNetwork(WebCommand<ResPacketLanguage>.MakeForm("game_start", reqPacketLevelClearRateLog, null));
	}

	public static void RequestLevelClear(int gid)
	{
		ReqPacketLevelClearRateLog reqPacketLevelClearRateLog = new ReqPacketLevelClearRateLog();
		reqPacketLevelClearRateLog.gid = gid;
		MonoSingleton<NetworkManager>.Instance.StartNetwork(WebCommand<ResPacketLanguage>.MakeForm("game_end", reqPacketLevelClearRateLog, null));
	}
}
