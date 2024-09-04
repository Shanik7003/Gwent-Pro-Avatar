namespace Engine
{
    public interface IObserver
    {
        void OnNotify(EventType eventType, object data);
    }

    public enum EventType
    {
        CardMoved,
        CardMovedAndDesapeare,
        CardMovedToRight,
        CardMovedToLeft,
        CardRemoved,
        CardPointsChanged,
        PlayerPointsChanged
    }
    
}
