using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace App.Abstraction.ThirdParty;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(ConfigurationName = "App.ICustomerCreditService")]
public interface ICustomerCreditService
{
    [OperationContract(Action = "http://tempuri.org/ICustomerCreditService/GetCreditLimit",
        ReplyAction = "http://tempuri.org/ICustomerCreditService/GetCreditLimitResponse")]
    int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth);
}