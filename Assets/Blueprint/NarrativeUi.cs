using Futilef;

#if FBP
public static unsafe partial class NarrativeUi {
    static Pool *nodePool;
    
    public static void Init() {
        nodePool = Pool.New();

        InitPlayerList();
        InitGal();
        InitBranching();
        InitMisc();
        InitFunctionMenu();
    }
}
#endif