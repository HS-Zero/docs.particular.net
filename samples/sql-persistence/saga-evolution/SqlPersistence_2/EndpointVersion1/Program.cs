﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Warn);

        Console.Title = "Samples.EvolveSaga.Version1";

        var endpointConfiguration = new EndpointConfiguration("Samples.EvolveSaga");
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=SqlPersistenceSample;Integrated Security=True";
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        SharedConfiguration.Apply(endpointConfiguration);

        endpointConfiguration.PurgeOnStartup(true);
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Saga starting. Will exit in 5 seconds. After exist start Phase 2 Endpoint.");

        #region startSaga
        var startTimeoutSaga = new StartSaga
        {
            TheId = Guid.NewGuid()
        };
        await endpointInstance.SendLocal(startTimeoutSaga)
            .ConfigureAwait(false);
        #endregion

        await Task.Delay(TimeSpan.FromSeconds(5))
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}