using Vessels.Data.Models;

namespace Vessel.UnitTests
{
    public class VesselPostionModelTests
    {
        [Fact]
        public void CreateModelOutOfRangeLatitude()
        {
            bool passed = false;
            try { var position = new VesselPosition() { Latitude = 200 }; }
            catch(ArgumentOutOfRangeException ex) { passed = true; }
            Assert.True(passed);
        }
    }
}