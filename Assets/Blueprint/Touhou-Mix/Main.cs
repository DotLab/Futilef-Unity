#if FBP_THMIX
/**
 * welcome -> song_select -> main_game -> game_result
 * welcome: */
public unsafe struct Main {
	const int dotlabIconMetaId = 0;
	
	const float dotlabIconSize = 200;

	TpSprite dotlabIconSprite;

	public static void Init(Main *self) {
		float scr2World = Scr.Scr2World;

		Node.Init(dotlabIconSprite);
		Node.SetMeta(dotlabIconSprite, Res.GetTpSpriteMeta(dotlabIconMetaId));
		Node.SetPos(dotlabIconSprite, Rel.Center, 0, 0);
		Node.SetSize(dotlabIconSprite, 0, 0);
		Node.SetVisible(dotlabIconSprite, true);

		
	}

	public static void StartWelcomeScene(Main *self) {
		
	}
	
	public static void Draw(Main *self) {

	}
}
#endif