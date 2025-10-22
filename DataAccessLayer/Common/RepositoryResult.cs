using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Common
{
    public class RepositoryResult<T>
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }


        public static RepositoryResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static RepositoryResult<T> Fail(string error) => new() { Success = false, Error = error };
    }
}
