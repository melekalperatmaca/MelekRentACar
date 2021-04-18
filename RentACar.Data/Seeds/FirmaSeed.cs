using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Models;

namespace RentACar.Data.Seeds
{
    public class FirmaSeed : IEntityTypeConfiguration<Firma>
    {
        public void Configure(EntityTypeBuilder<Firma> builder)
        {
            builder.HasData(
                new Firma
                {
                    FirmaID = 1,
                    Unvan = "AtmacAlpers",
                    Telefon = "0535210",
                    Mail = "atmacalper@atmacalper.com",
                    Adres = "Göztepe",
                    VergiNo = "A123"
                },
                new Firma
                {
                    FirmaID = 2,
                    Unvan = "Fistiks",
                    Telefon = "0326569",
                    Mail = "fistik@gmail.com",
                    Adres = "Göztepe",
                    VergiNo = "F125"
                },
                 new Firma
                 {
                     FirmaID = 3,
                     Unvan = "Atmacas",
                     Telefon = "0789542",
                     Mail = "atmaca@gmail.com",
                     Adres = "Göztepe",
                     VergiNo = "D567"
                 }
            );
        }
    }
}
