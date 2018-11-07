using Futilef;

#if FBP
public unsafe partial struct NarrativeUi {
	EsWorker esWorker;
	
	public static void Init(NarrativeUi *self) {
		EsWorker.Init(esWorker);

		InitPlayerList(self);
		// InitBranching(self);
		// InitGal(self);
		// InitMiscButtons(self);
		// InitFunctionMenu(self);
	}

	public static void Touch(NarrativeUi *self, Tch *touches, int touchCount) {
		TouchPlayerList(touches, touchCount);
	}

	public static void Update(NarrativeUi *self, float deltaTime) {
		EsWorker.Update(self->esWorker, deltaTime);
	}

	public static void Draw(NarrativeUi *self) {
		var playerAvatarArr = (TpSprite **)playerAvatarLst->arr;
		var playerBubbleArr = (TpSpriteSliced **)playerBubbleLst->arr;
		var playerBubbleTextArr = (BmText **)playerBubbleTextLst->arr;
		
		for (int i = 0; i < playerCount; i += 1) {
			TpSprite.Draw(playerAvatarArr[i]);
		}

		for (int i = 0; i < playerCount; i += 1) {
			TpSprite.Draw(playerBubbleArr[i]);
			BmText.Draw(playerBubbleTextArr[i]);
		}
	}
}
#endif