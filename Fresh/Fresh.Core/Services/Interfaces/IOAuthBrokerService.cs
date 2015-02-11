﻿using System;
using System.Threading.Tasks;

namespace Fresh.Core.Services.Interfaces
{
	public interface IOAuthBrokerService
	{
		Task<string> GetAccessTokenAsync(string clientId, string clientSecret, Uri authorizeUri, Uri redirectUri, Uri accessUri);
	}
}
