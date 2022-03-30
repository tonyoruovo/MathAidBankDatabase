using System;

namespace DataBaseTest.Repos 
{
    public interface IException<T>
    {
        IResult<T> Detail { get; }
    }

    public class ApplicationNullReferenceException : ApplicationException, IException<object>
    {
        public ApplicationNullReferenceException(string msg, int id): base(msg)
        {
            Detail = new PostResult<object>
            {
                FailCause = this,
                Id = id,
                Message = base.Message
            };
        }

        public ApplicationNullReferenceException(int id) : this("Null", id)
        {
        }

        public ApplicationNullReferenceException() : this(0)
        {
        }

        public IResult<object> Detail { get; }
    }

    public class ApplicationArgumentException : ApplicationException, IException<object>
    {
        public ApplicationArgumentException(string msg, int id, object details) : base(msg)
        {
            Detail = new PostResult<object>
            {
                FailCause = this,
                Id = id,
                Message = base.Message,
                Value = details
            };
        }

        public ApplicationArgumentException(string msg, object details) : this(msg, 0, details)
        {
        }

        public ApplicationArgumentException(object details) : this(null, 0, details)
        {
        }

        public IResult<object> Detail { get; }
    }

    public class ApplicationStateException : ApplicationException, IException<object>
    {
        public ApplicationStateException(string msg, int id, object details) : base(msg)
        {
            Detail = new PostResult<object>
            {
                FailCause = this,
                Id = id,
                Message = base.Message,
                Value = details
            };
        }

        public ApplicationStateException(string msg, object details) : this(msg, 0, details)
        {
        }

        public ApplicationStateException(object details) : this(null, 0, details)
        {
        }

        public ApplicationStateException() : this(null)
        {
        }

        public IResult<object> Detail { get; }
    }
}
