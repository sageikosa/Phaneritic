namespace Phaneritic.Implementations.Database;


[Serializable]
public class DbConflictException : Exception
{
	public DbConflictException(int errorCode) { ErrorCode = errorCode; }
	public DbConflictException(int errorCode,string message) : base(message) { ErrorCode = errorCode; }
	public DbConflictException(int errorCode,string message, Exception inner) : base(message, inner) { ErrorCode = errorCode; }

	public readonly int ErrorCode;
}