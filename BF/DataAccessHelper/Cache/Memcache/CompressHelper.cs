using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccessHelper.Cache.Memcache
{
    public class CompressHelper
    {
        public static byte[] Compress(object obj)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressStream = new GZipStream(ms, CompressionMode.Compress, true);
            using (MemoryStream source = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(source, obj);
                byte[] buffer = new byte[1024];
                int pos = 0;
                source.Seek(0, SeekOrigin.Begin);
                do
                {
                    pos = source.Read(buffer, 0, buffer.Length);
                    compressStream.Write(buffer, 0, pos);
                }
                while (pos > 0);

                source.Flush();
                source.Close();
            }
            compressStream.Close();
            return ms.ToArray();
        }

        public static T Decompress<T>(byte[] data)
        {
            if (data == null || data.Length == 0)
                return default(T);

            T obj = default(T);
            //解压后数据流
            using (MemoryStream source = new MemoryStream())
            {
                try
                {
                    //解压前数据流
                    MemoryStream ms = new MemoryStream(data);
                    //解压流
                    GZipStream decompressedStream = new GZipStream(ms, CompressionMode.Decompress, true);
                    int pos = 0;
                    byte[] buffer = new byte[1024];
                    do
                    {
                        pos = decompressedStream.Read(buffer, 0, buffer.Length);
                        source.Write(buffer, 0, pos);
                    }
                    while (pos > 0);

                    decompressedStream.Close();
                    source.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter bf = new BinaryFormatter();
                    object result = bf.Deserialize(source);

                    source.Flush();
                    source.Close();

                    if (result != null && result is T)
                        obj = (T)result;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return obj;
        }
    }
}
