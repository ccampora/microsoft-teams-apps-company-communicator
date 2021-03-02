// <copyright file="UrlTrackingDataRepository.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.URLTrackingData
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Repository of the notification data in the table storage.
    /// </summary>
    public class UrlTrackingDataRepository : BaseRepository<UrlTrackingDataEntity>, IUrlTrackingDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlTrackingDataRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public UrlTrackingDataRepository(
            ILogger<UrlTrackingDataRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: UrlTrackingDataTableNames.TableName,
                  defaultPartitionKey: UrlTrackingDataTableNames.DefaultPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task EnsureUrlTrackingTableExistsAsync()
        {
            var exists = await this.Table.ExistsAsync();
            if (!exists)
            {
                await this.Table.CreateAsync();
            }
        }
    }
}