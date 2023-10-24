using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options) : base(options) {

        }

        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<Villa> Villas { get; set; }

        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(

                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
                    ImageUrl = "",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate=DateTime.Now,

                },
                new Villa()
                {
                    Id = 2,
                    Name = "Royal Villa",
                    Details = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                    Occupancy = 5,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Premium Pool Villa",
                    Details = " Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur",
                    ImageUrl= "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                    Occupancy=4,
                    Rate=300,
                    Sqft=550,
                    Amenity="",
                    CreatedDate = DateTime.Now,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Diamond Villa",
                    Details = "Inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut ",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                    Occupancy = 4,
                    Rate = 550,
                    Sqft = 900,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                },
                new Villa
                {
                    Id = 5,
                    Name = "Diamond Pool Villa",
                    Details = "Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur",
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                    Occupancy = 4,
                    Rate = 550,
                    Sqft = 900,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                }

                );
        }
    }
}
