using Bomb;
using NUnit.Framework;

namespace Tests
{
    public class SeatHelperTest
    {
        [Test]
        public void TestMapping()
        {
            int count = 4;
            int selfSeat = 1;

            // 3
            int val = SeatHelper.MappingToView(selfSeat, 0, count);
            Assert.AreEqual(val, 3);

            // 0
            val = SeatHelper.MappingToView(selfSeat, 1, count);
            Assert.AreEqual(val, 0);

            // 1
            val = SeatHelper.MappingToView(selfSeat, 2, count);
            Assert.AreEqual(val, 1);

            // 2
            val = SeatHelper.MappingToView(selfSeat, 3, count);
            Assert.AreEqual(val, 2);
        }

        [Test]
        public void TestMapping1()
        {
            int count = 4;
            int selfSeat = 2;

            // 0
            int val = SeatHelper.MappingToView(selfSeat, 2, count);
            Assert.AreEqual(val, 0);

            // 1
            val = SeatHelper.MappingToView(selfSeat, 3, count);
            Assert.AreEqual(val, 1);

            // 2
            val = SeatHelper.MappingToView(selfSeat, 1, count);
            Assert.AreEqual(val, 3);

            // 3
            val = SeatHelper.MappingToView(selfSeat, 0, count);
            Assert.AreEqual(val, 2);
        }

        [Test]
        public void TestInverseMapping()
        {
            // 2
            int val = SeatHelper.InverseMapping(1, 1, 4);
            Assert.AreEqual(val, 2);

            val = SeatHelper.InverseMapping(1, 2, 4);
            Assert.AreEqual(val, 3);
            
            val = SeatHelper.InverseMapping(1, 3, 4);
            Assert.AreEqual(val, 0);
            
            val = SeatHelper.InverseMapping(1, 0, 4);
            Assert.AreEqual(val, 1);
        }
        
        [Test]
        public void TestInverseMapping1()
        {
            int val = SeatHelper.InverseMapping(2, 1, 4);
            Assert.AreEqual(val, 3);

            val = SeatHelper.InverseMapping(3, 1, 4);
            Assert.AreEqual(val, 0);
            
            val = SeatHelper.InverseMapping(3, 3, 4);
            Assert.AreEqual(val, 2);
            
            val = SeatHelper.InverseMapping(3, 0, 4);
            Assert.AreEqual(val, 3);
        }
    }
}