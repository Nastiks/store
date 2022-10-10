using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    public class JewelryServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithVendorCode_CallsGetAllByVendorCode()
        {
            var jewelryRepositoryStub = new Mock<IJewelryRepository>();
            jewelryRepositoryStub.Setup(x => x.GetAllByVendorCode(It.IsAny<string>()))
                .Returns(new[] { new Jewelry(1, "", "", "") });

            jewelryRepositoryStub.Setup(x => x.GetAllByTitleOrMaterial(It.IsAny<string>()))
                .Returns(new[] { new Jewelry(2, "", "", "") });

            var jewelryService = new JewelryService(jewelryRepositoryStub.Object);
            var validVendorCode = "Vendor code 0000000003";

            var actual = jewelryService.GetAllByQuery(validVendorCode);

            Assert.Collection(actual, jewelry => Assert.Equal(1, jewelry.Id));

        }

        [Fact]
        public void GetAllByQuery_WithMaterial_CallsGetAllByTitleOrMaterial()
        {
            var jewelryRepositoryStub = new Mock<IJewelryRepository>();
            jewelryRepositoryStub.Setup(x => x.GetAllByVendorCode(It.IsAny<string>()))
                .Returns(new[] { new Jewelry(1, "", "", "") });

            jewelryRepositoryStub.Setup(x => x.GetAllByTitleOrMaterial(It.IsAny<string>()))
                .Returns(new[] { new Jewelry(2, "", "", "") });

            var jewelryService = new JewelryService(jewelryRepositoryStub.Object);
            var invalidVendorCode = "0000000003";

            var actual = jewelryService.GetAllByQuery(invalidVendorCode);

            Assert.Collection(actual, jewelry => Assert.Equal(2, jewelry.Id));

        }
    }
}
