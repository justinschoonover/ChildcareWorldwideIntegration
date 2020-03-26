namespace ChildcareWorldwide.Integration.Manager.Models
{
    public class ErrorViewModel : MetaDataViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
