using System;
using Common.Exceptions;

namespace MheOperator.StoreManagementApi.Models
{
    public static class ApiErrorFactory
    {
        public static ApiError CreateApiError(SmHttpControllerException exception)
        {
            return new ApiError()
            {
                responseDetails = exception,
                errorTime = DateTime.Now
            };
        }
    }
}