namespace SpinWheel.Scripts.Utility.Event
{
    public abstract class GameEvent : IEvent
    {
        public void Raise()
        {
            EventBroker.Instance.RaiseEvent(this);
        }
    }
}