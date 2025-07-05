namespace XazeCustomRoles.Interfaces
{
    public interface ISpawnBroadcast
    {
        string SpawnBroadcast { get; }

        ushort BroadcastDuration { get; }

        bool ClearPrevious { get; }
    }
}
