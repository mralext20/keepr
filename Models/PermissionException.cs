using System;
using System.Runtime.Serialization;

namespace Keepr.Models
{
  class PermissionException : Exception
  {
    public PermissionException()
    {
    }

    public PermissionException(string message) : base(message)
    {
    }

    public PermissionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public PermissionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}