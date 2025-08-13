using FluentValidation.Results;
using Salon.Domain.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Salon.Application.Helpers
{
    public static class ResultHelper
    {
        private const string ERROR_HAS_OCCURRED = "Validation Failed";
        public static Result GetErrorResult(string message) => new Result(null).ValidationError(message);
        public static Result GetErrorResult(List<ValidationFailure> validationResults)
        {
            var errorList = validationResults.Select(x => x.ErrorMessage).ToList();
            var result = new Result(errorList);
            result.Error = true;
            result.Message = ERROR_HAS_OCCURRED;
            result.StatusCode = ((int)HttpStatusCode.BadRequest);

            return result;
        }
    }
}
