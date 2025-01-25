using Microsoft.AspNetCore.Http;
using MyCar.Shared.Infrastructure.Entities;

namespace MyCar.Shared.Infrastructure.Repositories;
internal interface IStoredFileRepository
{
	Task<Guid> AddAsync(IFormFile file, CancellationToken cancellationToken);
	Task DeleteAsync(StoredFile file, CancellationToken cancellationToken);
	Task<StoredFile> GetAsync(Guid id, CancellationToken cancellationToken);
}
