using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using App.Abstraction.Data;
using App.Abstraction.Models;
using Microsoft.Data.SqlClient;

namespace App.Infrastructure.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        public async Task<Company?> GetById(int id)
        {
            Company? company = null;
            var connectionString = ConfigurationManager.ConnectionStrings["appDatabase"].ConnectionString;

            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = CompanyRepositoryConstants.GetByIdSproc
            };

            var parameter = new SqlParameter(CompanyRepositoryConstants.CompanyIdParameter, SqlDbType.Int) { Value = id };
            command.Parameters.Add(parameter);

            connection.Open();
            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                company = new Company
                {
                    Id = int.Parse(reader[CompanyRepositoryConstants.CompanyId].ToString()),
                    Name = reader[CompanyRepositoryConstants.CompanyName].ToString(),
                };

                Enum.TryParse<Classification>(reader[CompanyRepositoryConstants.CompanyClassification].ToString(), out var classification);
                company.Classification = classification;
            }

            return company;
        }
    }

    public class CompanyRepositoryConstants
    {
        public const string GetByIdSproc = "uspGetCompanyById";
        public const string CompanyIdParameter = "@CompanyId";
        public const string CompanyId = "CompanyId";
        public const string CompanyName = "Name";
        public const string CompanyClassification = "ClassificationId";
    }
}
