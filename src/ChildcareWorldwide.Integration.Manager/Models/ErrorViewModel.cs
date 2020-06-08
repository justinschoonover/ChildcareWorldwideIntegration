namespace ChildcareWorldwide.Integration.Manager.Models
{
    public class ErrorViewModel : ViewModelBase
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
