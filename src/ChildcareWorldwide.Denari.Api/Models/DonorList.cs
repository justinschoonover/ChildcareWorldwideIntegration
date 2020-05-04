using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class DonorList
    {
        public string ResultCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string ErrorNumber { get; set; } = default!;
        public string ErrorMessage { get; set; } = default!;
        public string ListName { get; set; } = default!;
        public int PageSize { get; set; } = default!;
        public int PageCount { get; set; } = default!;
        public int CurrentPage { get; set; } = default!;
        public string Order { get; set; } = default!;
        public List<string> Fields { get; } = new List<string>();
        public DrapiFilter Filter { get; set; } = default!;
        public List<Donor> Data { get; } = new List<Donor>();
    }
}
