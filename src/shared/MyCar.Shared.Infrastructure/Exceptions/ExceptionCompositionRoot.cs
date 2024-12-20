using Microsoft.Extensions.DependencyInjection;
using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;
internal class ExceptionCompositionRoot(IServiceProvider serviceProvider) : IExceptionCompositionRoot
{
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	public ExceptionResponse Map(Exception exception) {
		using(var scope = _serviceProvider.CreateScope()) {
			var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>();
			var nonDefaultMappers = mappers.Where(x => x is not ExceptionToResponseMapper).ToArray();
			var result = nonDefaultMappers
				.Select(x => x.Map(exception))
				.SingleOrDefault(x => x is not null);

			if(result is not null) {
				return result;
			}

			var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionToResponseMapper);

			return defaultMapper?.Map(exception);
		}
	}
}
