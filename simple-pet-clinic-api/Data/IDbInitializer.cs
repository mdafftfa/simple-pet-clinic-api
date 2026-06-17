using System.Threading.Tasks;

namespace simple_pet_clinic_api.Data;

public interface IDbInitializer
{
    Task InitializeAsync();
}