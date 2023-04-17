using System;
namespace WorkingGood.Infrastructure.Communication.Entities
{
	public class BaseMessage
	{
        public string? Message { get; set; }
        public object? Object { get; set; }
        public object? Errors { get; set; }
    }
}

