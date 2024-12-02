using Microsoft.AspNetCore.Mvc;

namespace MyCar.Shared.Infrastructure.Api;
public class ProducesDefaultContentTypeAttribute(params string[] additionalContentTypes) 
	: ProducesAttribute("application/json", additionalContentTypes)
{
}
