using Dapper;
using Orleans.Storage.Application.Grains.Dealership.States;
using Orleans.Storage.Persistence.StateHandler.Storage;
using System.Data;
using System.Collections.Generic;

namespace Orleans.Storage.Application.StateHandler.States;

public class DealershipStateHandler(StateHandlerContext context, IDbConnection dbConnection) : StateHandlerBase<DealershipState>(context)
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public override async Task ReadAsync(string grainType, GrainId grainId, IGrainState<DealershipState> grainState)
    {
        try
        {
            var graindIdKey = grainId.Key.ToString();


            const string query = @"
                SELECT id, name, location, city, version
                FROM dealership
                WHERE id = @Id";

            const string carsQuery = @"
                    SELECT id, make, model, year
                    FROM dealership_car
                    WHERE dealership_id = @DealershipId";

            var dealershipDb = await _dbConnection.QueryFirstOrDefaultAsync<(string Id, string Name, string Location, string City, long Version)>(
                query,
                new { Id = graindIdKey }
            );            
           
            if (dealershipDb.Id == graindIdKey)
            {

                var dealershipCarsDb = await _dbConnection.QueryAsync<(string Id, string Make, string Model, int Year)>(
                    carsQuery,
                    new { DealershipId = graindIdKey }
                );

                var (id, name, location, city, version) = dealershipDb;
                var state = new DealershipState
                {
                    Name = name,
                    Location = location,
                    City = city,
                    Cars = []
                };                
                
                foreach (var (Id, Make, Model, Year) in dealershipCarsDb)
                {
                    state.Cars[Id] = new DealershipCarState
                    {
                        Make = Make,
                        Model = Model,
                        Year = Year
                    };
                }

                grainState.State = state;
                grainState.State.Cars.Snapshot();
                grainState.RecordExists = true;
                grainState.ETag = version.ToString();
                return;
            }

            grainState.State = new DealershipState { Cars = [] };
            grainState.RecordExists = false;
            grainState.ETag = null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading dealership state from database", ex);
        }
    }

    public override async Task WriteAsync(string grainType, GrainId grainId, IGrainState<DealershipState> grainState)
    {
        const string UpdateQuery = @"
            UPDATE dealership
            SET name = @Name,
                location = @Location,
                city = @City,
                version = @NewVersion
            WHERE id = @Id AND version = @CurrentVersion";

        const string InsertQuery = @"
            INSERT INTO dealership (id, name, location, city, version)
            VALUES (@Id, @Name, @Location, @City, @NewVersion)";
        try
        {
            var currentVersion = long.TryParse(grainState.ETag, out var v) ? v : 0;
            var newVersion = currentVersion + 1;
            bool isUpdate = grainState.RecordExists;
            string query = isUpdate ? UpdateQuery : InsertQuery;
            string newEtag = newVersion.ToString();

            var parameters = new Dictionary<string, object?>
            {
                ["Id"] = grainId.Key.ToString(),
                ["Name"] = grainState.State.Name,
                ["Location"] = grainState.State.Location,
                ["City"] = grainState.State.City,
                ["NewVersion"] = newVersion
            };
            if (isUpdate)
            {
                parameters["CurrentVersion"] = currentVersion;
            }

            var rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);
            if (rowsAffected == 0)
                throw new StateHandlerInconsistentException("Concurrency conflict: state has been modified by another process.");
            grainState.ETag = newEtag;
        }
        catch (Exception ex)
        {
            throw new Exception("Error writing dealership state to database", ex);
        }
    }

    public override async Task ClearAsync(string grainType, GrainId grainId, IGrainState<DealershipState> grainState)
    {
        try
        {
            const string query = "DELETE FROM dealership WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = grainId.Key.ToString() });
            grainState.State = new DealershipState { Cars = new() };
            grainState.ETag = null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error clearing dealership state from database", ex);
        }
    }
}
