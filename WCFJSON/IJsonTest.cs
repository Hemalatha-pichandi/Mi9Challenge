using JSONExamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFJSON
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IJsonTest" in both code and config file together.
    [ServiceContract]
    public interface IJsonTest
    {
        //[OperationContract]
        //Customer GetJsonData();

        //[OperationContract]
        //string PostJsonData(Customer customer);

        [OperationContract]
        Payload GetJsonData();

        [OperationContract]
        string PostJsonData(string jsonData);
    }
}
