using System;
using System.Collections.Generic;


namespace YummyGame.Framework
{
    public static class NetAPI
    {
        public static void FromInt32(int value, ref byte[] buffer,int offset)
        {
            if (buffer.Length < offset + 4) throw new ArgumentOutOfRangeException("长度不够");
            buffer[offset] = (byte)(value & 0xff);
            buffer[offset + 1] = (byte)(value >> 8 & 0xff);
            buffer[offset + 2] = (byte)(value >> 16 & 0xff);
            buffer[offset + 3] = (byte)(value >> 24 & 0xff);
        }

        public static void FromInt16(short value, ref byte[] buffer,int offset)
        {
            if (buffer.Length < offset + 2) throw new ArgumentOutOfRangeException("长度不够");
            buffer[offset] = (byte)(value & 0xff);
            buffer[offset+1] = (byte)(value >> 8 & 0xff);
        }

        public static int GetInt32(ref byte[] buffer, int offset)
        {
            if (buffer.Length < offset + 4) throw new ArgumentOutOfRangeException("长度不够");
            return (int)(buffer[offset] | buffer[offset + 1] << 8
                | buffer[offset + 2] << 16 | buffer[offset + 3] << 24);
        }

        public static short GetInt16(ref byte[] buffer, int offset)
        {
            if (buffer.Length < offset + 2) throw new ArgumentOutOfRangeException("长度不够");
            return (short)(buffer[offset] | buffer[offset + 1] << 8);
        }
    }
}
