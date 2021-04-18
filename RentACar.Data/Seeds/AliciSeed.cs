using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Models;

namespace RentACar.Data.Seeds
{
    public class AliciSeed : IEntityTypeConfiguration<Alici>
    {
        public void Configure(EntityTypeBuilder<Alici> builder)
        {
            builder.HasData(
                new Alici
                {
                    AliciID = 1,
                    Ad = "Melek",
                    Soyad = "Alper Atmaca",
                    TCKimlikNo = "22222222220",
                    CepTel = "05385161703",
                    Mail = "melekalperatmaca@gmail.com",
                    Adres = "Göztepe İzmir"
                },
                  new Alici
                  {
                      AliciID = 2,
                      Ad = "Kenan",
                      Soyad = "Atmaca",
                      TCKimlikNo = "11111111110",
                      CepTel = "05",
                      Mail = "kenanatmaca123@gmail.com",
                      Adres = "Göztepe İzmir"
                  },
                    new Alici
                    {
                        AliciID = 3,
                        Ad = "Fıstık",
                        Soyad = "Alper",
                        TCKimlikNo = "44444444440",
                        CepTel = "33",
                        Mail = "fistikalper15@gmail.com",
                        Adres = "Göztepe İzmir"
                    }
            );
        }
    }
}
