using Microsoft.AspNetCore.Http;
using MyCar.Shared.Abstractions.Entities;

namespace MyCar.Shared.Abstractions.Services;
public interface IStoredFileRepository
{
	Task<Guid> AddAsync(IFormFile file, CancellationToken cancellationToken);
	Task<StoredFile> GetAsync(Guid id, CancellationToken cancellationToken);
	Task DeleteAsync(StoredFile file, CancellationToken cancellationToken);
	Task<(byte[], string, string)> GetFileAsync(Guid id, CancellationToken cancellationToken);
}
