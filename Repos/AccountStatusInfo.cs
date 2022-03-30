
namespace DataBaseTest.Repos
{
    public readonly struct AccountStatusInfo
    {
        public AccountStatusInfo(AccountStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public readonly AccountStatus Status { get; }
        public readonly string Message { get; }
    }
}
