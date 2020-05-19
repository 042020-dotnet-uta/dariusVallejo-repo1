using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IFridge
    {
        [OperationContract]
        int countFruit();
        [OperationContract]
        int addFruit(int amount);
        [OperationContract]
        int subtractFruit(int amount);
    }
}