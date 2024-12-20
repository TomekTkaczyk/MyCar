using MyCar.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCar.Shared.Infrastructure.Exceptions;
public interface IExceptionCompositionRoot
{
	ExceptionResponse Map(Exception exception);
}
