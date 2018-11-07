using Futilef;

#if FBP
public unsafe struct NarrativeScene {
    public Pool nodePool;
    public PtrLst nodeLst;
    public PtrIntDict nodeDict;

    public static void Init(NarrativeScene *self) {
        var nodePool = self->nodePool;

        Pool.Init(nodePool);
        PtrLst.Init(self->nodeLst);
        Pool.AddDependent(nodePool, self->nodeLst);
        PtrIntDict.Init(self->nodeDict);
        Pool.AddDependent(nodePool, self->nodeDict);
    }

    public static void AddImg(NarrativeScene *self, int imgId, int id) {

    }
}
#endif