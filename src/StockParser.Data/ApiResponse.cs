using System;
using System.Collections.Generic;
using System.Text;

namespace StockParser.Common
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ApiResponseSuccess<T> : ApiResponse<T> where T : class
    {
        public ApiResponseSuccess(T model, string message = "")
        {
            IsSuccess = true;
            Message = message;
            Data = model;
            Code = string.Empty;
        }
    }

    public class ApiResponseSuccess : ApiResponse
    {
        public ApiResponseSuccess(string message = "")
        {
            IsSuccess = true;
            Message = message;
            Code = string.Empty;
        }
    }

    public class ApiResponseError<T> : ApiResponse<T> where T : class
    {
        public ApiResponseError(string errorMessage, T data = null, string code = "")
        {
            Message = errorMessage;
            IsSuccess = false;
            Code = code;
        }
    }

    public class ApiResponseError : ApiResponse
    {
        public ApiResponseError(string errorMessage, string code = "")
        {
            Message = errorMessage;
            IsSuccess = false;
            Code = code;
        }
    }

    public class ApiResponseException : ApiResponse
    {
        public ApiResponseException(Exception e, string code = "")
        {
            Message = e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "");
            IsSuccess = false;
            Code = code;
        }
    }

    public class ApiResponseException<T> : ApiResponse<T> where T : class
    {
        public ApiResponseException(Exception e, T data = null, string code = "")
        {
            Message = e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "");
            IsSuccess = false;
            Code = code;
            Data = data;
        }
    }
}
