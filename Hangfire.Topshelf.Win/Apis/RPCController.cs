using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hangfire.Samples.Framework.Logging;
using Hangfire.Topshelf.Core;

namespace Hangfire.Topshelf.Apis
{
    /// <summary>
    /// Restful Apis to process request and add it to background job from apps/micro-services.
    /// </summary>
    public class RPCController : ApiController
    {
        private static ILog _logger = LogProvider.GetLogger(typeof(RPCController));

      //  public IProductService ProductService { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RPCController"/> class.
        /// </summary>
        /// <param name="productService">The product service.</param>
      //  public RPCController(IProductService productService)
      //  {
       //     ProductService = productService;
      //  }

        /// <summary>
        /// Test apis
        /// </summary>
        /// <returns></returns>
        [Route("api/test")]
        [HttpGet]
        public HttpResponseMessage TestSimpleJob()
        {
         //   BackgroundJob.Enqueue<ISampleService>(x => x.SimpleJob(PerformContextToken.Null));
            return Request.CreateResponse(HttpStatusCode.OK, "SimpleJob enqueued.");
        }

        /// <summary>
		/// Echo 回應
		/// </summary>
		/// <returns>傳入什麼回傳什麼</returns>
		[Route("api/echo/{message}")]
        [HttpGet]
        public HttpResponseMessage Echo(string message)
        {
            return Request.CreateResponse(HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [Route("api/order/create")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(int productId)
        {
            //if (!(ProductService.Exists(productId)))
            //    throw new Exception("Product not exists.");

            //BackgroundJob.Enqueue<IOrderService>(x => x.CreateOrder(productId));

            return Request.CreateResponse(HttpStatusCode.OK, "Order Creating...");
        }



        /// <summary>
        ///  取得週期任務的設定檔
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>HttpResponseMessage.</returns>
        public HttpResponseMessage GetRecurringJobFile(string filename)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Order Creating...");
        }
    }
}