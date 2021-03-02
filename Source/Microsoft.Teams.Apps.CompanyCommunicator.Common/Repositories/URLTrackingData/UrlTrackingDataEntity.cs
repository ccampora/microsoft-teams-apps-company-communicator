namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.URLTrackingData
{
    using System;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// This entity holds the counter of the clicked Urls that have been share on an adaptive card.
    /// </summary>
    public class UrlTrackingDataEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the id of the notification.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets number of times the Adaptive Card Url has been clicked.
        /// </summary>
        public int ClickedUrlCounter { get; set; }

    }
}
