using System;
using Newtonsoft.Json;
using WorkingGood.Infrastructure.Common.Exceptions;
using WorkingGood.Infrastructure.Communication.Entities;

namespace WorkingGood.Infrastructure.Common.Extensions
{
	public static class ExternalMessageDeserializer
	{
		public static T DeserializeExternalMessage<T>(this string content)
		{
			return JsonConvert.DeserializeObject<T>(content)
				?? throw new BadResponseException(); 
		}
	}
}

