using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Tls;
using System.Linq;

namespace Ssl.Net
{
    /// <summary>
    /// 可靠的传输对象
    /// </summary>
    public class ReliableClient
    {
        private readonly DtlsClient _client;

        private readonly List<Packet> _sendBuffer;
        private readonly List<Packet> _receiveBuffer;

        private ushort _selfSn = 0xfff8;
        private ushort _wantSn = 0xfff8;

        public ReliableClient(DtlsClient client)
        {
            _client = client;
            _sendBuffer = new List<Packet>();
            _receiveBuffer = new List<Packet>();
        }

        public void Close()
        {
            _client.Close();
        }



        public int Receive(byte[] buf, int off, int len, int waitMillis)
        {
            int startMillis = Environment.TickCount;
            while (true)
            {
                if (Environment.TickCount - startMillis > waitMillis)
                {
                    return -1;
                }
                else if (Environment.TickCount - startMillis > 1000)
                {
                    ResendAsync();
                }
                else if (Environment.TickCount - startMillis > 600)
                {
                    SendAck();
                }
                Packet packet = _receiveBuffer.FirstOrDefault(x => x.Header.SelfSn == _wantSn);
                if (packet.Data == null)
                {
                    var size = _client.Receive(buf, 0, buf.Length, 500);
                    if (size <= 0)
                    {
                        continue;
                    }
                    packet.FromByteArray(buf, 0, size);
                }
                else
                {
                    _receiveBuffer.Remove(packet);
                }


                _sendBuffer.RemoveAll(x => (x.Header.SelfSn < packet.Header.WantSn) || (packet.Header.WantSn == 1 && x.Header.SelfSn <= ushort.MaxValue));

                switch (packet.Header.Type)
                {
                    case 0x16: // data


                        if (packet.Header.SelfSn == _wantSn) // 数据正常到
                        {
                            _wantSn = (ushort)(_wantSn == ushort.MaxValue ? ushort.MinValue + 1 : _wantSn + 1);
                            Array.Copy(packet.Data, 0, buf, off, (int)packet.Header.DatLen);
                            startMillis = Environment.TickCount;
                            return (int)packet.Header.DatLen;
                        }
                        else if (packet.Header.SelfSn > _wantSn) // 数据提前到达
                        {
                            _receiveBuffer.Add(packet);
                        }
                        else
                        {
                            ResendAsync();
                        }
                        break;
                    case 0x01: // ack
                        break;
                    default:
                        break;
                }


            }

        }

        private void SendAck()
        {
            Packet packet = new Packet { Header = { Signt = 0x6789abcd, Type = 0x0001, WantSn = _wantSn } };
            var data = packet.ToByteArray();
            _client.Send(data, 0, data.Length);

        }


        public void Send(byte[] buf, int off, int len)
        {
            Packet packet = new Packet
            {
                Header = new PacketHeader { Signt = 0X6789ABCD, Type = 0x0016, SelfSn = _selfSn, WantSn = _wantSn, DatLen = (uint)len },
                Data = new byte[len]
            };
            Buffer.BlockCopy(buf, off, packet.Data, 0, len);

            byte[] data = packet.ToByteArray();
            _client.Send(data, 0, data.Length);
            _selfSn = (ushort)(_selfSn == ushort.MaxValue ? ushort.MinValue + 1 : _selfSn + 1);
            _sendBuffer.Add(packet);
        }


        private void ResendAsync()
        {
            foreach (var item in _sendBuffer)
            {
                Packet packet = item;
                packet.Header.WantSn = _wantSn;
                var data = packet.ToByteArray();
                _client.Send(data, 0, data.Length);
            }
        }




        private struct PacketHeader
        {
            public uint Signt;
            public ushort SrcId;
            public ushort DstId;
            public ushort Flag;
            public ushort Type;
            public ushort SelfSn;
            public ushort WantSn;
            public uint Param;
            public uint DatLen;

            public static readonly int Size = Marshal.SizeOf<PacketHeader>();

        }

        private struct Packet
        {
            public PacketHeader Header;
            public byte[] Data;

            public int State { get; set; }
            public void FromByteArray(byte[] buf, int off, int len)
            {
                var hPtr = Marshal.AllocHGlobal(PacketHeader.Size);
                Marshal.Copy(buf, off, hPtr, PacketHeader.Size);
                Header = Marshal.PtrToStructure<PacketHeader>(hPtr);
                Marshal.FreeHGlobal(hPtr);
                if (Header.DatLen > 0)
                {
                    Data = new byte[Header.DatLen];
                    Buffer.BlockCopy(buf, off + PacketHeader.Size, Data, 0, (int)Header.DatLen);
                }
            }

            public byte[] ToByteArray()
            {
                var data = new byte[PacketHeader.Size + Header.DatLen];
                var hPtr = Marshal.AllocHGlobal(PacketHeader.Size);
                Marshal.StructureToPtr(Header, hPtr, false);
                Marshal.Copy(hPtr, data, 0, PacketHeader.Size);
                Marshal.FreeHGlobal(hPtr);
                if (Header.DatLen > 0)
                    Buffer.BlockCopy(Data, 0, data, PacketHeader.Size, (int)Header.DatLen);
                return data;
            }
        }
    }
}