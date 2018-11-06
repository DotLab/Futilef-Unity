using Futilef;

#if FBP
public unsafe partial class NarrativeUi {
    Pool *nodePool;
    PtrLst *chatAvatarLst;
    PtrLst *chatBubbleLst;
    PtrLst *chatBubbleTextLst;
    
    public void Init() {
        nodePool = Pool.New();

        // chat
        chatAvatarLst = PtrLst.New();
        Pool.AddDependent(nodePool, chatAvatarLst);
        chatBubbleLst = PtrLst.New();
        Pool.AddDependent(nodePool, chatBubbleLst);
        chatBubbleTextLst = PtrLst.New();
        Pool.AddDependent(nodePool, chatBubbleLst);
        for (int i = 0; i < 5; i += 1) {
            var chatAvatar = (TpSprite *)Pool.Alloc(nodePool, sizeof(TpSprite));
            TpSprite.Init(chatAvatar);
            TpSprite.SetScrSize(chatAvatar, 115, 115);
            TpSprite.SetScrPos(chatAvatar, Rel.TopLeft, Rel.TopLeft, -37, -27 - i * 147);
            TpSprite.SetZ(chatAvatar, 0);
            PtrLst.Push(chatAvatarLst, chatAvatar);

            var chatBubble = (TpSprite *)Pool.Alloc(nodePool, sizeof(TpSprite));
            TpSprite.Init(chatBubble, Res.GetTpSpriteMeta(1001));
            TpSprite.SetVisible(chatBubble, false);
            PtrLst.Push(chatBubbleLst, chatBubble);

            var chatBubbleText = (TpSprite *)Pool.Alloc(nodePool, sizeof(TpSprite));
            BmText.Init(chatBubbleText);
            PtrLst.Push(chatBubbleTextLst, chatBubbleText);
        }
    }

    public void SetChatAvatars(int[] avatarIds) {
    }

    public void SetChatMessage(int id, string message) {
    }

    public void HideChatMessage() {
    }

    public void ShowChat() {
    }

    public void HideChat() {
    }
}
#endif