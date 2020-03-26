using System;

namespace ChildcareWorldwide.Integration.Manager.Models
{
    public class ViewModelBase
    {
        public MetaDataViewModel MetaData { get; set; } = default!;
        public string? FullName { get; set; }
        public Uri? AvatarUri { get; set; }
    }

    public class ViewModelBase<T> : ViewModelBase
        where T : IViewModel
    {
        public T Data { get; set; } = default!;
    }
}
