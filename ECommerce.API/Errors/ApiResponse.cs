﻿
namespace ECommerce.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int StatusCode,string? Message= null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side. Errors lead to anger. anger leads to hate",
                _ => null
            };
        }
    }
}
