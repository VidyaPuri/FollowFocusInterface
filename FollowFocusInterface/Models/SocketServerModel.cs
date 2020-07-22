
namespace FollowFocusInterface.Models
{
    public class SocketServerModel
    {
        public bool IsServerListening { get; set; }
        public int FocusTargetIndex { get; set; }
        public int NoClientsConnected { get; set; }
    }
}
