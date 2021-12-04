using MyPageBlog.Shared.Utilities.Results.Abstract;
using MyPageBlog.Shared.Utilities.Results.Abstract.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Shared.Utilities.Results.Concrete
{
    public class DataResult<T> : IDataResult<T>
    {
        public ResultStatus ResultStatus { get; }
        public T Data { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public ResultStatus Success { get; }
        public DataResult(ResultStatus success)
        {
            Success = success;
        }

        public DataResult(ResultStatus resultStatus,T data)
        {
            ResultStatus = resultStatus;
            Data = data;
        }
        public DataResult(ResultStatus resultStatus, T data,string message)
        {
            ResultStatus = resultStatus;
            Data = data;
            Message = message;
        }
        public DataResult(ResultStatus resultStatus, T data, string message,Exception exception)
        {
            ResultStatus = resultStatus;
            Data = data;
            Message = message;
            Exception = exception;
        }
      
    }
}
