﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCar.Shared.Abstractions.Modules;
public interface IModule
{
	string Name { get; }
	string Path { get; }
	IEnumerable<string> Policies => null;
	void Register(IServiceCollection services, IConfiguration configuration);
	void Use(IApplicationBuilder app);
}
