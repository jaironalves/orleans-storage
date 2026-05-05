using Dapper;
using Orleans.Storage.Application.Grains.Car.States;
using Orleans.Storage.Persistence.StateHandler.Storage;
using System.Data;

namespace Orleans.Storage.Application.StateHandler.States;

public class CarStateHandler(StateHandlerContext context, IDbConnection dbConnection) : StateHandlerBase<CarState>(context)
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public override async Task ReadAsync(string grainType, GrainId grainId, IGrainState<CarState> grainState)
    {
        try
        {
            const string query = @"
                SELECT id, make, model, year
                FROM car 
                WHERE id = @Id";

            var car = await _dbConnection.QueryFirstOrDefaultAsync<CarState>(
                query,
                new { Id = grainId.Key.ToString() }
            );

            if (car is not null)
            {
                grainState.State = car;
                grainState.RecordExists = true;
                grainState.ETag = GenerateETag(car);
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
        try
        {
            const string query = @"
                INSERT INTO car (make, model, year, color, license_plate)
                VALUES (@Make, @Model, @Year, @Color, @LicensePlate)
                ON DUPLICATE KEY UPDATE
                    make = VALUES(make),
                    model = VALUES(model),
                    year = VALUES(year),
                    color = VALUES(color)";

            await _dbConnection.ExecuteAsync(query, new
            {
                grainState.State.Make,
                grainState.State.Model,
                grainState.State.Year,                
                LicensePlate = grainId.Key.ToString()
            });

            grainState.ETag = GenerateETag(grainState.State);
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
