using Dapper;
using Orleans.Storage.Application.Grains.Car.States;
using Orleans.Storage.Persistence.StateHandler.Storage;
using System.Data;
using System.Numerics;

namespace Orleans.Storage.Application.StateHandler.States;

public class CarStateHandler(StateHandlerContext context, IDbConnection dbConnection) : StateHandlerBase<CarState>(context)
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public override async Task ReadAsync(string grainType, GrainId grainId, IGrainState<CarState> grainState)
    {        
        try
        {
            var graindIdKey = grainId.Key.ToString();

            const string query = @"
                SELECT id, make, model, year, version
                FROM car 
                WHERE id = @Id";

            var carDb = await _dbConnection.QueryFirstOrDefaultAsync<(string Id, string Make, string Model, int Year, long Version)>(
                query,
                new { Id = graindIdKey }
            );

            if (graindIdKey.Equals(carDb.Id))
            {
                var (id, make, model, year, version) = carDb;
                grainState.State = new CarState
                {
                    Make = make,
                    Model = model,
                    Year = year
                };
                grainState.RecordExists = true;
                grainState.ETag = version.ToString();
                return;
            }

            grainState.State = new CarState();
            grainState.RecordExists = false;
            grainState.ETag = null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading car state from database", ex);
        }
    }

    public override async Task WriteAsync(string grainType, GrainId grainId, IGrainState<CarState> grainState)
    {
        const string UpdateQuery = @"
        UPDATE car
        SET make = @Make,
            model = @Model,
            year = @Year,
            version = @NewVersion
        WHERE id = @Id AND version = @CurrentVersion";

        const string InsertQuery = @"
        INSERT INTO car (id, make, model, year, version)
        VALUES (@Id, @Make, @Model, @Year, @NewVersion)";

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
                ["Make"] = grainState.State.Make,
                ["Model"] = grainState.State.Model,
                ["Year"] = grainState.State.Year,
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
            throw new Exception("Error writing car state to database", ex);
        }
    }

    public override async Task ClearAsync(string grainType, GrainId grainId, IGrainState<CarState> grainState)
    {
        try
        {
            const string query = "DELETE FROM car WHERE license_plate = @LicensePlate";

            await _dbConnection.ExecuteAsync(query, new { LicensePlate = grainId.Key.ToString() });

            grainState.State = new CarState();
            grainState.ETag = null;
        }
        catch (Exception ex)
        {
            throw new Exception("Error clearing car state from database", ex);
        }
    }

    private static string GenerateETag(CarState state)
    {
        var hash = HashCode.Combine(
            state.Make,
            state.Model,
            state.Year
        );

        return hash.ToString();
    }
}
