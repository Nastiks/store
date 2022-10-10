namespace Store.Tests
{
    public class JewelryTests
    {
        [Fact]
        public void IsVendorCode_WithNull_ReturnFalse()
        {
            bool actual = Jewelry.IsVendorCode(null);

            Assert.False(actual);
        }

        [Fact]
        public void IsVendorCode_WithBlankString_ReturnFalse()
        {
            bool actual = Jewelry.IsVendorCode("    ");

            Assert.False(actual);
        }

        [Fact]
        public void IsVendorCode_WithInvalidVendorCode_ReturnFalse()
        {
            bool actual = Jewelry.IsVendorCode("Vendor code 123");

            Assert.False(actual);
        }

        [Fact]
        public void IsVendorCode_WithVendorCode10_ReturnTrue()
        {
            bool actual = Jewelry.IsVendorCode("Vendor code 0000000004");

            Assert.True(actual);
        }

        [Fact]
        public void IsVendorCode_WithVendorCode13_ReturnTrue()
        {
            bool actual = Jewelry.IsVendorCode("Vendor code 0000000004000");

            Assert.True(actual);
        }

        [Fact]
        public void IsVendorCode_WithTrashStart_ReturnFalse()
        {
            bool actual = Jewelry.IsVendorCode("xxxVendor code 0000000004000 yyy");

            Assert.False(actual);
        }
    }
}