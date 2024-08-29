namespace Engine
{
    public interface IObserver
    {
        void OnNotify(EventType eventType, object data);
    }

    public enum EventType
    {
        CardMoved,
        CardRemoved,
        CardPointsChanged,
        PlayerPointsChanged
    }
    
}
