using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class DonorList
    {
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
        public string ListName { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public string Order { get; set; }
        public List<string> Fields { get; set; }
        public DrapiFilter Filter { get; set; }
        public List<Donor> Data { get; set; }
    }
}
