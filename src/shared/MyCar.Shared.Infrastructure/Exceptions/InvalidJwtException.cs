﻿using MyCar.Shared.Abstractions.Exceptions;

namespace MyCar.Shared.Infrastructure.Exceptions;

public sealed class InvalidJwtException() : MyCarException("Invalid JWT") { }