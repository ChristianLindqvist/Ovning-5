using Xunit;
using Ovning_5;

namespace Ovning_5.tests
{
    public class GarageTests
    {
        [Fact]
        public void AddVehicle_WhenGarageHasSpace_ShouldReturnTrue()
        {
            // Arrange - Skapa garaget med gränssnittet IVehicle
            var garage = new Garage<IVehicle>(2);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };

            // Act - Försök lägga till bilen
            bool result = garage.AddVehicle(car);

            // Assert - Verifiera att det lyckades (true)
            Assert.True(result);
        }

        [Fact]
        public void AddVehicle_WhenGarageIsFull_ShouldReturnFalse()
        {
            // Arrange - Skapa ett garage som bara har 1 plats
            var garage = new Garage<IVehicle>(1);
            garage.AddVehicle(new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" });

            // Skapa en extra bil som inte kommer att få plats
            var extraCar = new Car { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 4, FuelType = "Gasoline" };

            // Act - Försök lägga till den extra bilen
            bool result = garage.AddVehicle(extraCar);

            // Assert - Verifiera att det misslyckades (false)
            Assert.False(result);
        }


        [Fact]
        public void RemoveVehicle_WhenVehicleExists_ShouldRemoveAndReturnTrue()
        {
            // Arrange - Skapa garage och parkera en bil
            var garage = new Garage<IVehicle>(2);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            garage.AddVehicle(car);

            // Act - Ta bort bilen
            bool result = garage.RemoveVehicle("ABC123");

            // Assert - Kolla att metoden säger att det lyckades, samt att platsen blev ledig
            Assert.True(result);
            Assert.Equal(2, garage.GetFreeSpotsCount()); // Garaget ska nu ha 2 lediga platser igen
        }

        [Fact]
        public void RemoveVehicle_WhenVehicleDoesNotExist_ShouldReturnFalse()
        {
            // Arrange - Garaget är tomt
            var garage = new Garage<IVehicle>(2);

            // Act - Försök ta bort en bil som inte finns
            bool result = garage.RemoveVehicle("XYZ987");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveVehicle_ShouldBeCaseInsensitive()
        {
            // Arrange - Parkera en bil med STORA bokstäver
            var garage = new Garage<IVehicle>(2);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            garage.AddVehicle(car);

            // Act - Ta bort med små bokstäver ("abc123")
            bool result = garage.RemoveVehicle("abc123");

            // Assert - Det ska fungera ändå
            Assert.True(result);
        }





        [Fact]
        public void GetVehicleTypesCount_WhenGarageHasVehicles_ShouldReturnCorrectCounts()
        {
            // Arrange - Förbered ett garage och parkera två bilar och en motorcykel
            var garage = new Garage<IVehicle>(5);

            var car1 = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            var car2 = new Car { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 4, FuelType = "Diesel" };
            var mc = new Motorcycle { RegistrationNumber = "MC1111", Color = "Blue", NumberOfWheels = 2, CylinderVolume = 600 };

            garage.AddVehicle(car1);
            garage.AddVehicle(car2);
            garage.AddVehicle(mc);

            // Act - Anropa metoden
            Dictionary<string, int> result = garage.GetVehicleTypesCount();

            // Assert - Verifiera att Dictionaryt innehåller exakt rätt antal
            Assert.True(result.ContainsKey("Car"));
            Assert.True(result.ContainsKey("Motorcycle"));

            Assert.Equal(2, result["Car"]);        // Det ska finnas exakt 2 bilar
            Assert.Equal(1, result["Motorcycle"]); // Det ska finnas exakt 1 motorcykel
        }

        [Fact]
        public void GetVehicleTypesCount_WhenGarageIsEmpty_ShouldReturnEmptyDictionary()
        {
            // Arrange - Skapa ett helt tomt garage
            var garage = new Garage<IVehicle>(5);

            // Act
            Dictionary<string, int> result = garage.GetVehicleTypesCount();

            // Assert - Verifiera att ordlistan är helt tom
            Assert.Empty(result);
        }



        [Fact]
        public void GetFreeSpotsCount_WhenGarageIsEmpty_ShouldReturnTotalCapacity()
        {
            // Arrange - Skapa ett tomt garage med kapacitet 5
            var garage = new Garage<IVehicle>(5);

            // Act - Hämta antalet lediga platser
            int freeSpots = garage.GetFreeSpotsCount();

            // Assert - Det ska finnas exakt 5 lediga platser kvar
            Assert.Equal(5, freeSpots);
        }

        [Fact]
        public void GetFreeSpotsCount_WhenVehiclesAreAdded_ShouldDecreaseCorrectly()
        {
            // Arrange - Skapa ett garage med kapacitet 3 och lägg till en bil
            var garage = new Garage<IVehicle>(3);
            var car = new Car
            {
                RegistrationNumber = "ABC123",
                Color = "Black",
                NumberOfWheels = 4,
                FuelType = "Gasoline"
            };
            garage.AddVehicle(car);

            // Act - Hämta antalet lediga platser
            int freeSpots = garage.GetFreeSpotsCount();

            // Assert - Kapaciteten var 3, vi tog 1 plats, så det ska finnas exakt 2 platser kvar
            Assert.Equal(2, freeSpots);
        }



        [Fact]
        public void RegistrationNumberExists_WhenVehicleExists_ShouldReturnTrue()
        {
            // Arrange - Parkera en bil med registreringsnummer ABC123
            var garage = new Garage<IVehicle>(3);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            garage.AddVehicle(car);

            // Act
            bool exists = garage.RegistrationNumberExists("ABC123");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void RegistrationNumberExists_WhenVehicleDoesNotExist_ShouldReturnFalse()
        {
            // Arrange - Garaget är tomt
            var garage = new Garage<IVehicle>(3);

            // Act
            bool exists = garage.RegistrationNumberExists("XYZ987");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void RegistrationNumberExists_ShouldBeCaseInsensitive()
        {
            // Arrange - Parkera en bil med STORA bokstäver
            var garage = new Garage<IVehicle>(3);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            garage.AddVehicle(car);

            // Act - Sök med små bokstäver ("abc123")
            bool exists = garage.RegistrationNumberExists("abc123");

            // Assert - Det bör fortfarande returnera true
            Assert.True(exists);
        }




        [Fact]
        public void SearchByProperties_ShouldFilterByColorAndType_WhenParametersAreGiven()
        {
            // Arrange - Parkera två bilar (en svart, en röd) och en röd motorcykel
            var garage = new Garage<IVehicle>(5);

            var blackCar = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4, FuelType = "Gasoline" };
            var redCar = new Car { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 4, FuelType = "Diesel" };
            var redMc = new Motorcycle { RegistrationNumber = "MC1111", Color = "Red", NumberOfWheels = 2, CylinderVolume = 600 };

            garage.AddVehicle(blackCar);
            garage.AddVehicle(redCar);
            garage.AddVehicle(redMc);

            // Act - Sök specifikt efter fordon av typen "Car" som är "Red" (hjul och emptyWheelSearch ignoreras genom true)
            var results = garage.SearchByProperties("Car", "Red", 0, true);

            // Assert - Det ska bara finnas exakt 1 matchning (den röda bilen, inte den svarta bilen eller den röda motorcykeln)
            Assert.Single(results);
            Assert.Contains(results, v => v.RegistrationNumber == "XYZ987");
        }

        [Fact]
        public void SearchByProperties_ShouldReturnAllVehicles_WhenAllParametersAreBlank()
        {
            // Arrange
            var garage = new Garage<IVehicle>(5);
            garage.AddVehicle(new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 });
            garage.AddVehicle(new Motorcycle { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 2 });

            // Act - Skicka in tomma strängar och true för hjul (vilket betyder att användaren lämnade allt tomt)
            var results = garage.SearchByProperties("", "", 0, true);

            // Assert - Båda fordonen ska returneras
            var resultList = new List<IVehicle>(results);
            Assert.Equal(2, resultList.Count);
        }

        [Fact]
        public void SearchByProperties_ShouldFilterByWheels_WhenWheelSearchIsActive()
        {
            // Arrange
            var garage = new Garage<IVehicle>(5);
            garage.AddVehicle(new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 });
            garage.AddVehicle(new Motorcycle { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 2 });

            // Act - Sök efter fordon med exakt 2 hjul (och strunta i typ/färg genom tomma strängar, emptyWheelSearch = false)
            var results = garage.SearchByProperties("", "", 2, false);

            // Assert - Det ska bara hitta motorcykeln
            Assert.Single(results);
            Assert.Contains(results, v => v.NumberOfWheels == 2);
        }





        [Fact]
        public void SearchByRegNo_WhenVehicleExists_ShouldReturnSpecificVehicle()
        {
            // Arrange - Parkera två bilar
            var garage = new Garage<IVehicle>(5);
            var car1 = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 };
            var car2 = new Car { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 4 };

            garage.AddVehicle(car1);
            garage.AddVehicle(car2);

            // Act - Sök efter den första bilen
            var results = garage.SearchByRegNo("ABC123");

            // Assert - Det ska finnas exakt 1 matchning och det ska vara rätt bil
            Assert.Single(results);
            Assert.Contains(results, v => v.RegistrationNumber == "ABC123");
        }

        [Fact]
        public void SearchByRegNo_ShouldBeCaseInsensitive()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            var car = new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 };
            garage.AddVehicle(car);

            // Act - Sök med små bokstäver ("abc123")
            var results = garage.SearchByRegNo("abc123");

            // Assert
            Assert.Single(results);
            Assert.Contains(results, v => v.RegistrationNumber == "ABC123");
        }

        [Fact]
        public void SearchByRegNo_WhenRegNoIsEmpty_ShouldReturnAllVehicles()
        {
            // Arrange - Parkera två fordon
            var garage = new Garage<IVehicle>(5);
            garage.AddVehicle(new Car { RegistrationNumber = "ABC123", Color = "Black", NumberOfWheels = 4 });
            garage.AddVehicle(new Motorcycle { RegistrationNumber = "XYZ987", Color = "Red", NumberOfWheels = 2 });

            // Act - Skicka in en tom sträng (användaren tryckte bara Enter)
            var results = garage.SearchByRegNo("");

            // Assert - Båda fordonen ska returneras
            var resultList = new List<IVehicle>(results);
            Assert.Equal(2, resultList.Count);
        }
    }
}
