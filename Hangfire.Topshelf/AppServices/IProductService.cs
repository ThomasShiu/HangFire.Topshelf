using Hangfire.Samples.Framework;

namespace Hangfire.Topshelf.AppServices
{
	public interface IProductService : IAppService
	{
		bool Exists(int productId);
	}
}
