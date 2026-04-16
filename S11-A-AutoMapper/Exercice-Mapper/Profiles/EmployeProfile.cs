using Exercice_Mapper.Models;
using Exercice_Mapper.ViewModels;
using Mapster;

namespace Exercice_Mapper.Profiles
{
    public class EmployeProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<Employe, EmployeVM>.NewConfig().TwoWays();
            TypeAdapterConfig<Employe, EmployeEditVM>.NewConfig().TwoWays();
            TypeAdapterConfig<Employe, EmployeIndexVM>.NewConfig().TwoWays()
                .Map(dest => dest.Departement, source => source.Departement.Nom)
                .Map(dest => dest.Pays, source => source.PaysOrigine.Nom)
                .Map(dest => dest.Id, source => source.EmployeId);
        }
    }
}
