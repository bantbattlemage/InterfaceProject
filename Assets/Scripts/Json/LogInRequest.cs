using Newtonsoft.Json;

[JsonObject]
[System.Serializable]
public class LogInRequest
{
	public string Username;
	public string Password;
	public bool NewRegistration;
}