using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Data
{
    public class ServiceResult<T> : ServiceResult
    {
        public T Entity { get; set; }

        public ServiceResult(ServiceStatus status, string message = "") : base(status, message) { }
        public ServiceResult(ServiceStatus status, T entity, string message = "") : base(status, message)
        {
            Entity = entity;
        }
    }

    public class ServiceResult
    {
        public ServiceStatus Status { get; set; }
        public string Message { get; set; }

        public ServiceResult(ServiceStatus status, string message = "")
        {
            Status = status;
            Message = message;
        }
    }

    public enum ServiceStatus
    {
        Ok,
        Created,
        CreatedWithErrors,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        Error,
        Found,
        NotEnoughStock,
        NotCreated,
        Valid,
        InValid,
        FoundWithErrors
    }
}
