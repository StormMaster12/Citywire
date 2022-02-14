using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using App.Abstraction.ThirdParty;

namespace App.Infrastructure.ThirdParty;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class CustomerCreditServiceClient : ClientBase<ICustomerCreditService>, ICustomerCreditService
{
    public CustomerCreditServiceClient()
    {
    }

    public CustomerCreditServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public CustomerCreditServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public CustomerCreditServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public CustomerCreditServiceClient(Binding binding, EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
    {
        return Channel.GetCreditLimit(firstname, surname, dateOfBirth);
    }
}