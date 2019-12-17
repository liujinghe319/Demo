using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace LjhTools.Data
{
    public class DataSetHelper
    {
        public static byte[] GetBinaryFormatDataSet(DataSet ds)
        {
            MemoryStream serializationStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(serializationStream, ds);
            return serializationStream.GetBuffer();
        }

        public static DataSet RetrieveDataSet(byte[] binaryData)
        {
            MemoryStream serializationStream = new MemoryStream(binaryData);
            IFormatter formatter = new BinaryFormatter();
            return (formatter.Deserialize(serializationStream) as DataSet);
        }
    }

}