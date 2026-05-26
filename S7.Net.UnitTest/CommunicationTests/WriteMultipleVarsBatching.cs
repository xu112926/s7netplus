using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using S7.Net.Protocol;
using S7.Net.Types;

namespace S7.Net.UnitTest.CommunicationTests;

[TestClass]
public class WriteMultipleVarsBatching
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
    public async Task Sync_Write_Splits_When_Request_Exceeds_Pdu()
    {
        var cs = new CommunicationSequence
        {
            ConnectionOpenTemplates.ConnectionRequestConfirm,
            LowPduCommunicationSetup,
            WriteSingleByteRequestResponse(0, 0x11),
            WriteSingleByteRequestResponse(1, 0x22)
        };

        static async Task Client(int port)
        {
            var conn = new Plc(IPAddress.Loopback.ToString(), port, new TsapPair(new Tsap(1, 2), new Tsap(3, 4)));
            await conn.OpenAsync();

            conn.Write(ByteItem(0, 0x11), ByteItem(1, 0x22));

            conn.Close();
        }

        await Task.WhenAll(cs.Serve(out var port), Client(port));
    }

    [TestMethod, Timeout(1000)]
    public async Task Async_Write_Splits_When_Request_Exceeds_Pdu()
    {
        var cs = new CommunicationSequence
        {
            ConnectionOpenTemplates.ConnectionRequestConfirm,
            LowPduCommunicationSetup,
            WriteSingleByteRequestResponse(2, 0x33),
            WriteSingleByteRequestResponse(3, 0x44)
        };

        static async Task Client(int port)
        {
            var conn = new Plc(IPAddress.Loopback.ToString(), port, new TsapPair(new Tsap(1, 2), new Tsap(3, 4)));
            await conn.OpenAsync();

            await conn.WriteAsync(ByteItem(2, 0x33), ByteItem(3, 0x44));

            conn.Close();
        }

        await Task.WhenAll(cs.Serve(out var port), Client(port));
    }

    [TestMethod]
    public void Write_Throws_When_Single_Item_Exceeds_Pdu()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);

        Assert.ThrowsException<System.Exception>(() => conn.Write(new DataItem
        {
            DataType = DataType.DataBlock,
            DB = 1,
            StartByteAdr = 0,
            VarType = VarType.Byte,
            Count = conn.MaxPDUSize,
            Value = new byte[conn.MaxPDUSize]
        }));
    }

    [TestMethod]
    public async Task WriteAsync_Throws_When_Single_Item_Exceeds_Pdu()
    {
        var conn = new Plc(CpuType.S71200, "127.0.0.1", 0, 0);

        await Assert.ThrowsExceptionAsync<System.Exception>(() => conn.WriteAsync(new DataItem
        {
            DataType = DataType.DataBlock,
            DB = 1,
            StartByteAdr = 0,
            VarType = VarType.Byte,
            Count = conn.MaxPDUSize,
            Value = new byte[conn.MaxPDUSize]
        }));
    }

    private static DataItem ByteItem(int startByteAdr, byte value)
    {
        return new DataItem
        {
            DataType = DataType.DataBlock,
            DB = 1,
            StartByteAdr = startByteAdr,
            VarType = VarType.Byte,
            Count = 1,
            Value = value
        };
    }

    private static RequestResponsePair WriteSingleByteRequestResponse(int startByteAdr, byte value)
    {
        var bitAddress = startByteAdr * 8;
        return new RequestResponsePair(
            $"""
                // TPKT
                03 00 00 24

                // COTP
                02 f0 80

                // S7 write request
                32 01 00 00 05 00 00 0e 00 05 05 01

                // Item
                12 0a 10 02 00 01 00 01 84 00 {(bitAddress >> 8):X2} {(bitAddress & 0xff):X2}

                // Item data
                00 04 00 08 {value:X2}
            """,
            """
                // TPKT
                03 00 00 16

                // COTP
                02 f0 80

                // S7 write response
                32 03 00 00 05 00 00 02 00 01 00 00 05 01

                // Item result
                ff
            """
        );
    }
}
