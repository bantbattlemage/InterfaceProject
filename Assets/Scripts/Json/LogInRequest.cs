using System;
using Newtonsoft.Json;

[JsonObject, Serializable]
public class LogInRequest
{
	public string Username;
}