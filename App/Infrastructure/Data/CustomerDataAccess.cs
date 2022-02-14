using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using App.Abstraction.Data;
using App.Abstraction.Models;
using Microsoft.Data.SqlClient;

namespace App.Infrastructure.Data
{
    public static class CustomerDataAccess
    {
        public static async Task AddCustomer(Customer customer)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;

            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = CustomerDataAccessConstants.AddCustomerSproc
            };

            var firstNameParameter = new SqlParameter(CustomerDataAccessConstants.FirstNameParam, SqlDbType.VarChar, 50) { Value = customer.Firstname };
            command.Parameters.Add(firstNameParameter);
            var surnameParameter = new SqlParameter(CustomerDataAccessConstants.SurnameParam, SqlDbType.VarChar, 50) { Value = customer.Surname };
            command.Parameters.Add(surnameParameter);
            var dateOfBirthParameter = new SqlParameter(CustomerDataAccessConstants.DateOfBirthParam, SqlDbType.DateTime) { Value = customer.DateOfBirth };
            command.Parameters.Add(dateOfBirthParameter);
            var emailAddressParameter = new SqlParameter(CustomerDataAccessConstants.EmailAddressParam, SqlDbType.VarChar, 50) { Value = customer.EmailAddress };
            command.Parameters.Add(emailAddressParameter);
            var hasCreditLimitParameter = new SqlParameter(CustomerDataAccessConstants.HasCreditLimit, SqlDbType.Bit) { Value = customer.HasCreditLimit };
            command.Parameters.Add(hasCreditLimitParameter);
            var creditLimitParameter = new SqlParameter(CustomerDataAccessConstants.CreditLimitParam, SqlDbType.Int) { Value = customer.CreditLimit };
            command.Parameters.Add(creditLimitParameter);
            var companyIdParameter = new SqlParameter(CustomerDataAccessConstants.CompanyIdParam, SqlDbType.Int) { Value = customer.Company.Id };
            command.Parameters.Add(companyIdParameter);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public class CustomerDataAccessConstants
    {
        public const string AddCustomerSproc = "uspAddCustomer";
        public const string FirstNameParam = "@Firstname";
        public const string SurnameParam = "@Surname";
        public const string DateOfBirthParam = "@DateOfBirth";
        public const string EmailAddressParam = "@EmailAddress";
        public const string HasCreditLimit = "@HasCreditLimit";
        public const string CreditLimitParam = "@CreditLimit";
        public const string CompanyIdParam = "@CompanyId";
    }
}