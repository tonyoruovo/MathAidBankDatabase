using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public class AccountPostResult<TResult> : IResult<TResult>
    {
        public int Id { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public TResult Value { get; set; }

        public ApplicationException FailCause { get; set; }
    }
}
