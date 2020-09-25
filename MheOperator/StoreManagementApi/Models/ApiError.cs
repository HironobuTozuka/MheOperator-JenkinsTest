using System;
using Common.Exceptions;
namespace MheOperator.StoreManagementApi.Models
{
    public class ApiError {

        public DateTime errorTime;
        public SmHttpControllerException responseDetails;

    }
}