using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using S7.Net.Protocol;
using S7.Net.Types;

namespace S7.Net.UnitTest.CommunicationTests;

[TestClass]
public class ReadMultipleVarsBatching
{
    private static RequestResponsePair LowPduCommunicationSetup { get; } = new RequestResponsePair(
        """
            // TPKT
            03 // Version
            00 // Reserved
            00 19 // Length

            // Data header
            02 // Length
            F0 // Data identifier
            80 // PDU number and end of transmission

            // S7 header
            32 // Protocol ID
            01 // Message type job request
            00 00 // Reserved
            PDU1 PDU2 // PDU reference
            00 08 // Parameter length (Communication Setup)
            00 00 // Data length

            // Communication Setup
            F0 // Function code
            00 // Reserved
            00 03 // Max AMQ caller
            00 03 // Max AMQ callee
            03 C0 // PDU size (960)
        """,
        """
            // TPKT
            03 // Version
            00 // Reserved
            00 1B // Length

            // Data header
            02 // Length
            F0 // Data identifier
            80 // PDU number and end of transmission

            // S7 header
            32 // Protocol ID
            03 // Message type ack data
            00 00 // Reserved
            PDU1 PDU2 // PDU reference
            00 08 // Parameter length (Communication Setup)
            00 00 // Data length
            00 // Error class
            00 // Error code

            // Communication Setup
            F0 // Function code
            00 // Reserved
            00 03 // Max AMQ caller
            00 03 // Max AMQ callee
            00 22 // PDU size (34)
        """
    );

    [TestMethod, Timeout(1000)]
    public async Task Sync_ReadMultipleVars_Splits_When_Request_Exceeds_Pdu()
    {
        var cs = new CommunicationSequence
        {
            ConnectionOpenTemplates.ConnectionRequestConfirm,
            LowPduCommunicationSetup,
            ReadSingleByteRequestResponse(0, 0x11),
            ReadSingleByteRequestResponse(1, 0x22)
        };

        static async Task Client(int port)
        {
            var conn = new Plc(IPAddress.Loopback.ToString(), port, new TsapPair(new Tsap(1, 2), new Tsap(3, 4)));
            await conn.OpenAsync();

            var dataItems = new List<DataItem>
            {
                ByteItem(0),
                ByteItem(1)
            };

            conn.ReadMultipleVars(dataItems);

            Assert.AreEqual((byte)0x11, dataItems[0].Value);
            Assert.AreEqual((byte)0x22, dataItems[1].Value);
            conn.Close();
        }

        await Task.WhenAll(cs.Serve(out var port), Client(port));
    }

    [TestMethod, Timeout(1000)]
    public async Task Async_ReadMultipleVars_Splits_When_Request_Exceeds_Pdu()
    {
        var cs = new CommunicationSequence
        {
            ConnectionOpenTemplates.ConnectionRequestConfirm,
            LowPduCommunicationSetup,
            ReadSingleByteRequestResponse(2, 0x33),
            ReadSingleByteRequestResponse(3, 0x44)
        };

        static async Task Client(int port)
        {
            var conn = new Plc(IPAddress.Loopback.ToString(), port, new TsapPair(new Tsap(1, 2), new Tsap(3, 4)));
            await conn.OpenAsync();

            var dataItems = new List<DataItem>
            {
                ByteItem(2),
                ByteItem(3)
            };

            var result = await conn.ReadMultipleVarsAsync(dataItems);

            Assert.AreSame(dataItems, result);
            Assert.AreEqual((byte)0x33, result[0].Value);
            Assert.AreEqual((byte)0x44, result[1].Value);
            conn.Close();
        }

        await Task.WhenAll(cs.Serve(out var port), Client(port));
    }

    [TestMethod]
    public void ReadMultipleVars_Empty_List_Does_Not_Require_Connection()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);
        conn.ReadMultipleVars(new List<DataItem>());
    }

    [TestMethod]
    public async Task ReadMultipleVarsAsync_Empty_List_Returns_Same_List_Without_Connection()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);
        var dataItems = new List<DataItem>();

        var result = await conn.ReadMultipleVarsAsync(dataItems);

        Assert.AreSame(dataItems, result);
    }

    [TestMethod]
    public void ReadMultipleVars_Throws_When_Single_Item_Exceeds_Pdu()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);

        Assert.ThrowsException<System.Exception>(() => conn.ReadMultipleVars(new List<DataItem>
        {
            new DataItem
            {
                DataType = DataType.DataBlock,
                DB = 1,
                StartByteAdr = 0,
                VarType = VarType.Byte,
                Count = conn.MaxPDUSize
            }
        }));
    }

    [TestMethod]
    public async Task ReadMultipleVarsAsync_Throws_When_Single_Item_Exceeds_Pdu()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);

        await Assert.ThrowsExceptionAsync<System.Exception>(() => conn.ReadMultipleVarsAsync(new List<DataItem>
        {
            new DataItem
            {
                DataType = DataType.DataBlock,
                DB = 1,
                StartByteAdr = 0,
                VarType = VarType.Byte,
                Count = conn.MaxPDUSize
            }
        }));
    }

    private static DataItem ByteItem(int startByteAdr)
    {
        return new DataItem
        {
            DataType = DataType.DataBlock,
            DB = 1,
            StartByteAdr = startByteAdr,
            VarType = VarType.Byte,
            Count = 1
        };
    }

    private static RequestResponsePair ReadSingleByteRequestResponse(int startByteAdr, byte value)
    {
        var bitAddress = startByteAdr * 8;
        return new RequestResponsePair(
            $"""
                // TPKT
                03 00 00 1f

                // COTP
                02 f0 80

                // S7 read request
                32 01 00 00 PDU1 PDU2 00 0e 00 00 04 01

                // Item
                12 0a 10 02 00 01 00 01 84 00 {(bitAddress >> 8):X2} {(bitAddress & 0xff):X2}
            """,
            $"""
                // TPKT
                03 00 00 1b

                // COTP
                02 f0 80

                // S7 read response
                32 03 00 00 PDU1 PDU2 00 02 00 06 00 00 04 01

                // Item data
                ff 04 00 08 {value:X2} 00
            """
        );
    }
}
