namespace Microsoft.Teams.Apps.CompanyCommunicator.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Teams.Apps.CompanyCommunicator.Bot;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.URLTrackingData;
    using Microsoft.Teams.Apps.CompanyCommunicator.Common.Services.MicrosoftGraph;

    /// <summary>
    /// Controller for Redirect endpoint.
    /// </summary>
    [Route("redirect")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlTrackingDataRepository urlTrackingDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectController"/> class.
        /// </summary>
        /// <param name="urlTrackingDataRepository">urlTracling data repository</param>
        public RedirectController(
            IUrlTrackingDataRepository urlTrackingDataRepository)
        {
            this.urlTrackingDataRepository = urlTrackingDataRepository ?? throw new ArgumentNullException(nameof(urlTrackingDataRepository));
        }

        /// <summary>
        /// Add one to the url counter and then redirect to url.
        /// </summary>
        /// <param name="url">The url to redirect to.</param>
        /// <param name="rowkey">The rowkey from the notificationdataEntity</param>
        /// <param name="notificationid"></param>
        /// <param name="recipientid"></param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string url, string rowkey, string notificationid, string recipientid)
        {
            if (rowkey == null & url != null)
            {
                return this.Redirect(url);
            }

            // Check notification status for the recipient.
            var urlTrackingData = await this.urlTrackingDataRepository.GetAsync(
                partitionKey: notificationid,
                rowKey: recipientid);

            if (urlTrackingData == null)
            {
                // Create a sent notification based on the draft notification.
                var createurlTrackingData = new UrlTrackingDataEntity
                {
                    PartitionKey = notificationid,
                    RowKey = recipientid,
                    Id = recipientid,
                    ClickedUrlCounter = 1,
                    Recipientid = recipientid,
                };
                await this.urlTrackingDataRepository.InsertOrMergeAsync(createurlTrackingData);
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
