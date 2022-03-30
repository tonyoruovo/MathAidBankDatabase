using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public class PostResult<TReturnValue> : IResult<TReturnValue>
    {
        public int Id { get; set; }

        public TReturnValue Value { get; set; }
        // <summary>
        // Number of properties that was completed or written along with the posting.
        // For example, If an object has 3 properties, and only 2 of those was filled
        // and saved then this value will be 2.
        // </summary>
        //public int NumOfEntriesWrittenToDatabse { get; set; }
        //public bool WasPosted { get; set; }

        public string Message { get; set; }

        public bool Successful { get; set; }

        public ApplicationException FailCause { get; set; }
    }

    public interface IResult<T>
    {
        public bool Successful { get; }

        public string Message { get; }

        public T Value { get; }

        public ApplicationException FailCause { get; }
    }
}
