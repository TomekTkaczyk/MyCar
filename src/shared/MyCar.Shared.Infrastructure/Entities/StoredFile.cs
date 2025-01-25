namespace MyCar.Shared.Infrastructure.Entities;
internal class StoredFile : EntityBase
{
	public string FileName { get; set; }
	public string FileDescription { get; set; }
	public string FileHash { get; set; }
	public string FileStoragePath { get; set; }
	public string FileStorageName { get; set; }
}
