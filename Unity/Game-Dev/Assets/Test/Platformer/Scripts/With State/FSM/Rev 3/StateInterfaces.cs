namespace PlatformerFSM
{
    public interface ICoreState
    {
        string AnimName { get; }
        bool CollisionsEnabled { get; }
    }
}
