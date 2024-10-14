using dotmob.sr.maptool;

public class SceneMapTool : SceneClass
{
	private void Start()
	{
		MonoSingleton<SceneControlManager>.Instance.SetFirstMapToolSet(this);
        //@TODO ENABLE_SRDEBUG
#if ENABLE_SRDEBUG
        SRDebug.Init();
#endif
        MonoSingleton<MapToolManager>.Instance.topMenu.RefreshSaveAndPlayButtonInteractive();
		MonoSingleton<UIManager>.Instance.HideLoading();
	}
}
