using Microsoft.VisualStudio.TestTools.UnitTesting;
using S7.Net.Types;
using System;

namespace S7.Net.UnitTest
{
    [TestClass]
    public class PLCAddressParsingTests
    {
        [TestMethod]
        public void T01_ParseM2000_1()
        {
            DataItem dataItem = DataItem.FromAddress("M2000.1");

            Assert.AreEqual(DataType.Memory, dataItem.DataType, "Wrong datatype for M2000.1");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for M2000.1");
            Assert.AreEqual(VarType.Bit, dataItem.VarType, "Wrong vartype for M2000.1");
            Assert.AreEqual(2000, dataItem.StartByteAdr, "Wrong startbyte for M2000.1");
            Assert.AreEqual(1, dataItem.BitAdr, "Wrong bit for M2000.1");
        }

        [TestMethod]
        public void T02_ParseMB200()
        {
            DataItem dataItem = DataItem.FromAddress("MB200");

            Assert.AreEqual(DataType.Memory, dataItem.DataType, "Wrong datatype for MB200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for MB200");
            Assert.AreEqual(VarType.Byte, dataItem.VarType, "Wrong vartype for MB200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for MB200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for MB200");
        }

        [TestMethod]
        public void T03_ParseMW200()
        {
            DataItem dataItem = DataItem.FromAddress("MW200");

            Assert.AreEqual(DataType.Memory, dataItem.DataType, "Wrong datatype for MW200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for MW200");
            Assert.AreEqual(VarType.Word, dataItem.VarType, "Wrong vartype for MW200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for MW200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for MW200");
        }

        [TestMethod]
        public void T04_ParseMD200()
        {
            DataItem dataItem = DataItem.FromAddress("MD200");

            Assert.AreEqual(DataType.Memory, dataItem.DataType, "Wrong datatype for MD200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for MD200");
            Assert.AreEqual(VarType.DWord, dataItem.VarType, "Wrong vartype for MD200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for MD200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for MD200");
        }


        [TestMethod]
        public void T05_ParseI2000_1()
        {
            DataItem dataItem = DataItem.FromAddress("I2000.1");

            Assert.AreEqual(DataType.Input, dataItem.DataType, "Wrong datatype for I2000.1");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for I2000.1");
            Assert.AreEqual(VarType.Bit, dataItem.VarType, "Wrong vartype for I2000.1");
            Assert.AreEqual(2000, dataItem.StartByteAdr, "Wrong startbyte for I2000.1");
            Assert.AreEqual(1, dataItem.BitAdr, "Wrong bit for I2000.1");
        }

        [TestMethod]
        public void T06_ParseIB200()
        {
            DataItem dataItem = DataItem.FromAddress("IB200");

            Assert.AreEqual(DataType.Input, dataItem.DataType, "Wrong datatype for IB200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for IB200");
            Assert.AreEqual(VarType.Byte, dataItem.VarType, "Wrong vartype for IB200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for IB200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for IB200");
        }

        [TestMethod]
        public void T07_ParseIW200()
        {
            DataItem dataItem = DataItem.FromAddress("IW200");

            Assert.AreEqual(DataType.Input, dataItem.DataType, "Wrong datatype for IW200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for IW200");
            Assert.AreEqual(VarType.Word, dataItem.VarType, "Wrong vartype for IW200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for IW200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for IW200");
        }

        [TestMethod]
        public void T08_ParseID200()
        {
            DataItem dataItem = DataItem.FromAddress("ID200");

            Assert.AreEqual(DataType.Input, dataItem.DataType, "Wrong datatype for ID200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for ID200");
            Assert.AreEqual(VarType.DWord, dataItem.VarType, "Wrong vartype for ID200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for ID200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for ID200");
        }


        [TestMethod]
        public void T09_ParseQ2000_1()
        {
            DataItem dataItem = DataItem.FromAddress("Q2000.1");

            Assert.AreEqual(DataType.Output, dataItem.DataType, "Wrong datatype for Q2000.1");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for Q2000.1");
            Assert.AreEqual(VarType.Bit, dataItem.VarType, "Wrong vartype for Q2000.1");
            Assert.AreEqual(2000, dataItem.StartByteAdr, "Wrong startbyte for Q2000.1");
            Assert.AreEqual(1, dataItem.BitAdr, "Wrong bit for Q2000.1");
        }

        [TestMethod]
        public void T10_ParseQB200()
        {
            DataItem dataItem = DataItem.FromAddress("QB200");

            Assert.AreEqual(DataType.Output, dataItem.DataType, "Wrong datatype for QB200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for QB200");
            Assert.AreEqual(VarType.Byte, dataItem.VarType, "Wrong vartype for QB200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for QB200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for QB200");
        }

        [TestMethod]
        public void T11_ParseQW200()
        {
            DataItem dataItem = DataItem.FromAddress("QW200");

            Assert.AreEqual(DataType.Output, dataItem.DataType, "Wrong datatype for QW200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for QW200");
            Assert.AreEqual(VarType.Word, dataItem.VarType, "Wrong vartype for QW200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for QW200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for QW200");
        }

        [TestMethod]
        public void T12_ParseQD200()
        {
            DataItem dataItem = DataItem.FromAddress("QD200");

            Assert.AreEqual(DataType.Output, dataItem.DataType, "Wrong datatype for QD200");
            Assert.AreEqual(0, dataItem.DB, "Wrong dbnumber for QD200");
            Assert.AreEqual(VarType.DWord, dataItem.VarType, "Wrong vartype for QD200");
            Assert.AreEqual(200, dataItem.StartByteAdr, "Wrong startbyte for QD200");
            Assert.AreEqual(0, dataItem.BitAdr, "Wrong bit for QD200");
        }

        [TestMethod]
        public void T13_ParseDbReal()
        {
            DataItem dataItem = DataItem.FromAddress("DB1.DBF10");

            Assert.AreEqual(DataType.DataBlock, dataItem.DataType);
            Assert.AreEqual(1, dataItem.DB);
            Assert.AreEqual(VarType.Real, dataItem.VarType);
            Assert.AreEqual(10, dataItem.StartByteAdr);
            Assert.AreEqual(1, dataItem.Count);
        }

        [TestMethod]
        public void T14_ParseDbDateTime()
        {
            DataItem dataItem = DataItem.FromAddress("DB1.DBT10");

            Assert.AreEqual(DataType.DataBlock, dataItem.DataType);
            Assert.AreEqual(1, dataItem.DB);
            Assert.AreEqual(VarType.DateTime, dataItem.VarType);
            Assert.AreEqual(10, dataItem.StartByteAdr);
            Assert.AreEqual(1, dataItem.Count);
        }

        [TestMethod]
        public void T15_ParseStringAddressesWithReservedLength()
        {
            DataItem stringItem = DataItem.FromAddress("DB1.DBB10.20");
            DataItem s7StringItem = DataItem.FromAddress("DB1.DBS30.40");
            DataItem s7WStringItem = DataItem.FromAddress("DB1.DBW50.60");

            Assert.AreEqual(VarType.String, stringItem.VarType);
            Assert.AreEqual(10, stringItem.StartByteAdr);
            Assert.AreEqual(20, stringItem.Count);

            Assert.AreEqual(VarType.S7String, s7StringItem.VarType);
            Assert.AreEqual(30, s7StringItem.StartByteAdr);
            Assert.AreEqual(40, s7StringItem.Count);

            Assert.AreEqual(VarType.S7WString, s7WStringItem.VarType);
            Assert.AreEqual(50, s7WStringItem.StartByteAdr);
            Assert.AreEqual(60, s7WStringItem.Count);
        }

        [TestMethod]
        public void T16_InvalidDbAddressesThrowInvalidAddressException()
        {
            Assert.ThrowsException<InvalidAddressException>(() => DataItem.FromAddress("DB1.DBQ10"));
            Assert.ThrowsException<InvalidAddressException>(() => DataItem.FromAddress("DB1.DBB10.foo"));
        }
    }
}
