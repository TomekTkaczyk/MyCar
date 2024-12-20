﻿namespace MyCar.Shared.Abstractions.Auth;
public interface IPasswordManager
{
	string Secure(string password);
	bool Validate(string password, string securePassword);
}
