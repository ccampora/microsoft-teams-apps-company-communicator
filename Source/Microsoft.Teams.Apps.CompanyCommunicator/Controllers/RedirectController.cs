namespace Microsoft.Teams.Apps.CompanyCommunicator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.URLTrackingData;
    using Moq;

    [Route("redirect")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        //private readonly Mock<IUrlTrackingDataRepository> urlTrackingDataRepository = new Mock<IUrlTrackingDataRepository>();
        private readonly IUrlTrackingDataRepository urlTrackingDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectController"/> class.
        /// </summary>
        /// <param name="urlTrackingDataRepository">urlTracling data repository</param>
        public RedirectController(
            IUrlTrackingDataRepository urlTrackingDataRepository)
        {
            this.urlTrackingDataRepository = urlTrackingDataRepository;
        }


        /// <summary>
        /// Add one to the url counter and then redirect to url.
        /// </summary>
        /// <param name="url">The url to redirect to.</param>
        /// <param name="rowkey">The rowkey from the notificationdataEntity</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string url, string rowkey)
        {
            if (rowkey == null & url != null)
            {
                return this.Redirect(url);
            }

            // Check notification status for the recipient.
            var urlTrackingData = await this.urlTrackingDataRepository.GetAsync(
                partitionKey: UrlTrackingDataTableNames.DefaultPartition,
                rowKey: rowkey);

            if (urlTrackingData == null)
            {
                // Create a sent notification based on the draft notification.
                var createurlTrackingData = new UrlTrackingDataEntity
                {
                    PartitionKey = UrlTrackingDataTableNames.DefaultPartition,
                    RowKey = rowkey,
                    Id = rowkey,
                    ClickedUrlCounter = 0,
                };
                await this.urlTrackingDataRepository.InsertOrMergeAsync(createurlTrackingData);
                await this.urlTrackingDataRepository.CreateOrUpdateAsync(createurlTrackingData);
            }
            else
            {
                urlTrackingData.ClickedUrlCounter += 1;
                await this.urlTrackingDataRepository.InsertOrMergeAsync(urlTrackingData);
            }

            // TODO: Log url here
            return this.Redirect(url);
        }
    }
}
