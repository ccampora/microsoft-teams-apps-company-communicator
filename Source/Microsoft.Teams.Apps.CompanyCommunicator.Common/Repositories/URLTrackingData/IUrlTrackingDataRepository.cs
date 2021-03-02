// <copyright file="IUrlTrackingDataRepository.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.URLTrackingData
{
    using System.Threading.Tasks;

    /// <summary>
    /// interface for Url Tracking Data Repository.
    /// </summary>
    public interface IUrlTrackingDataRepository : IRepository<UrlTrackingDataEntity>
    {
        /// <summary>
        /// This method ensures the UrlTracking table is created in the storage.
        /// This method should be called before kicking off an Azure function that uses the SentNotificationData table.
        /// Otherwise the app will crash.
        /// By design, Azure functions (in this app) do not create a table if it's absent.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public Task EnsureUrlTrackingTableExistsAsync();
    }
}
