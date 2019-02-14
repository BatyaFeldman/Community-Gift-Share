using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFund.Data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public int ContributorAmount { get; internal set; }
        public decimal Total { get; internal set; }
    }

    public class SimchaDb
    {
        private string _connectionString;
        public SimchaDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Simcha> GetSimchas()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from simchas";
                connection.Open();
                List<Simcha> results = new List<Simcha>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Simcha simcha = new Simcha();
                    simcha.Id = (int)reader["Id"];
                    simcha.Name = (string)reader["Name"];
                    simcha.Date = (DateTime)reader["Date"];
                    simcha.Total = GetTotalForSimcha(simcha.Id);
                    results.Add(simcha);

                    
                }
                return results;
            }
        }

        public Simcha GetSimchaById(int simchaId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from simchas where Id = @simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                Simcha simcha = new Simcha();
                simcha.Id = (int)reader["Id"];
                simcha.Date = (DateTime)reader["Date"];
                simcha.Name = (string)reader["Name"];
                return simcha;
            }
        }

        public IEnumerable<Contributor> GetContributors()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from contributors";
                connection.Open();
                List<Contributor> results = new List<Contributor>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Contributor contributor = new Contributor();
                    contributor.Id = (int)reader["Id"];
                    contributor.Name = (string)reader["Name"];
                    contributor.PhoneNumber = (string)reader["PhoneNumber"];
                    contributor.AlwaysInclude = (bool)reader["AlwaysInclude"];
                    contributor.Date = (DateTime)reader["Date"];
                    contributor.Balance = GetBalance(contributor.Id);
                    results.Add(contributor);

                }
                return results;
            }

        }

        public int GetContributorCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select count(*) from Contributors";
                connection.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void AddSimcha(Simcha simcha)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Simchas (Name, Date)" +
                    " values(@Name, @Date)";
                cmd.Parameters.AddWithValue("@Name", simcha.Name);
                cmd.Parameters.AddWithValue("@Date", simcha.Date);
                connection.Open();
                cmd.ExecuteNonQuery();

            }

        }



        public IEnumerable<Contribution> GetContributions()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select * from Contributions";
                connection.Open();
                List<Contribution> results = new List<Contribution>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Contribution
                    {
                        Id = (int)reader["Id"],
                        SimchaId = (int)reader["SimchaId"],
                        ContributorId = (int)reader["ContributorId"],
                        Amount = (decimal)reader["Amount"]
                    });
                }
                return results;
            }
        }

        public int CountContributionsPerSimcha(int SimchaId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select count(*) from contributions where SimchaId=@SimchaId";
                cmd.Parameters.AddWithValue("SimchaId", SimchaId);
                connection.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void AddContribution(Contribution contribution)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Contributions(SimchaId, ContributorId, Amount)" +
                    " values(@SimchaId, @ContributorId, @Amount)";
                cmd.Parameters.AddWithValue("@SimchaId", contribution.SimchaId);
                cmd.Parameters.AddWithValue("@ContributorId", contribution.ContributorId);
                cmd.Parameters.AddWithValue("@Amount", contribution.Amount);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int AddContributor(string name, string phoneNumber, bool alwaysInclude, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Contributors(Name, PhoneNumber, AlwaysInclude, Date)" +
                    " values(@Name, @PhoneNumber, @AlwaysInclude, @Date) Select SCOPE_IDENTITY()";
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@AlwaysInclude", alwaysInclude);
                cmd.Parameters.AddWithValue("@Date", date);

                connection.Open();
                return (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public void EditContributor(int Id, string Name, string PhoneNumber, bool AlwaysInclude, DateTime Date)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Update Contributors  " +
                    "set Name=@name, PhoneNumber=@phoneNumber, AlwaysInclude=@alwaysInclude, Date=@date" +
                    " where Id=@id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@phoneNumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@alwaysInclude", AlwaysInclude);
                cmd.Parameters.AddWithValue("@date", Date);
                connection.Open();
                cmd.ExecuteNonQuery();


            }

        }

        public void AddDeposit(int contributorId, decimal amount, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Deposits(ContributorId, Amount, Date) " +
                    "Values(@ContributorId, @Amount, @Date)";
                cmd.Parameters.AddWithValue("@ContributorId", contributorId);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Date", date);
                connection.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public void AddDepositForContributor(Deposit deposit, int contributorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Insert into Deposits(ContributorId, Amount, Date) " +
                    "Values(@ContributorId, @Amount, @Date)";
                cmd.Parameters.AddWithValue("@ContributorId", contributorId);
                cmd.Parameters.AddWithValue("@Amount", deposit.Amount);
                cmd.Parameters.AddWithValue("@Date", deposit.Date);
                connection.Open();
                cmd.ExecuteNonQuery();

            }
        }
        public decimal GetBalance(int contributorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select sum(Amount) from deposits where contributorId = @contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                var value = cmd.ExecuteScalar();
                decimal depositTotal;
                if (value == DBNull.Value)
                {
                    depositTotal = 0;
                }
                else
                {
                    depositTotal = (decimal)value;
                }
                cmd.Parameters.Clear();

                cmd.CommandText = "select sum(Amount) from contributions where contributorId=@contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                var valueb = cmd.ExecuteScalar();
                decimal contributionTotal;
                if (valueb == DBNull.Value)
                {
                    contributionTotal = 0;
                }
                else
                {
                    contributionTotal = (decimal)valueb;
                }

                return depositTotal - contributionTotal;

            }
        }

        private decimal GetTotalForSimcha(int simchaId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
                using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select Sum(Amount) as Total from contributions where simchaId=@simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                connection.Open();
                var value = cmd.ExecuteScalar();
                decimal total;
                if (value == DBNull.Value)
                {
                    total = 0;
                }
                else
                {
                    total = (decimal)value;
                }
                return total;
                
            }
        }

        public IEnumerable<SimchaContributor> GetSimchaContributors(int simchaId)
       {
            IEnumerable<Contributor> contributors = GetContributors();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select * from contributions where SimchaId=@simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                List<Contribution> contributions = new List<Contribution>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                  contributions.Add(new Contribution 

                    {
                        SimchaId = (int)reader["SimchaId"],
                        ContributorId = (int)reader["ContributorId"],
                        Amount = (decimal)reader["Amount"]
                        
                    });
                }
                List<SimchaContributor> SimchaContributors = new List<SimchaContributor>();
                foreach (Contributor c in contributors)
                {
                    SimchaContributor sc = new SimchaContributor();
                    sc.ContributorId = c.Id;
                    sc.Name = c.Name;
                    sc.AlwaysInclude = c.AlwaysInclude;
                    sc.Balance = GetBalance(c.Id);
                    
                    Contribution contribution= contributions.FirstOrDefault(contrib => contrib.ContributorId == c.Id);
                    if(contribution != null)
                    {
                        sc.Amount = contribution.Amount;
                    }
                    
                    SimchaContributors.Add(sc);
                }
                return SimchaContributors;
            }

            
        }

        public void UpdateContributions(int SimchaId, IEnumerable<ContributionInclusion>contributors)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
                using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "delete from contributions where SimchaId = @SimchaId";
                cmd.Parameters.AddWithValue("@SimchaId", SimchaId);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "insert into contributions(ContributorId, SimchaId, Amount, Date)" +
                    " values(@contributorId, @simchaId, @amount, GETDATE())";
                foreach(ContributionInclusion contributor in contributors.Where(c => c.Include))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@contributorId", contributor.ContributorId);
                    cmd.Parameters.AddWithValue("@simchaId", SimchaId);
                    cmd.Parameters.AddWithValue("@Amount", contributor.Amount);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public Contributor GetContributorById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select * from Contributors where Id=@contributorId";
                cmd.Parameters.AddWithValue("@contributorId", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                    
                }
                Contributor contributor = new Contributor();
                contributor.Id = (int)reader["Id"];
                contributor.Name = (string)reader["Name"];
                contributor.PhoneNumber = (string)reader["PhoneNumber"];
                contributor.AlwaysInclude = (bool)reader["AlwaysInclude"];
                contributor.Date = (DateTime)reader["Date"];
                contributor.Balance = GetBalance(contributor.Id);
                return contributor;

            }
        }
    
        public IEnumerable<History>GetContributionsForContrib(int contributorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString)) 
                using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select c.*, s.Name from contributions c " +
                    "left join Simchas s " +
                    "on c.SimchaId = s.Id " +
                    "where c.contributorId = @contributorId " +
                    "order by Date Asc";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                List<History> contributionHistory = new List<History>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    History contribution = new History();
                    String simchaName = (string)reader["Name"];
                    contribution.Action = "Contribution to the " + simchaName + " Simcha";
                    contribution.Date = (DateTime)reader["Date"];
                    decimal amount = (decimal)reader["Amount"];
                    contribution.Amount = "-" + amount.ToString("C");
                    contributionHistory.Add(contribution);

                }
                return contributionHistory;

            }
        
        }

        public IEnumerable<History> GetDepositsForContrib(int contributorId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select * from Deposits where contributorId=@contributorId";
                cmd.Parameters.AddWithValue("@contributorId", contributorId);
                List<History> DepositHistory = new List<History>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    History deposit = new History();
                    deposit.Action = "Deposit";
                    deposit.Date = (DateTime)reader["Date"];
                    decimal amount =(decimal)reader["Amount"];
                    deposit.Amount = "+" + amount.ToString("C");
                    DepositHistory.Add(deposit);
                }
                return DepositHistory;
            }
        }

        public IEnumerable<String>EmailListForSimcha(int simchaId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select cs.*, c.Name from contributions cs " +
                    "left join contributors c  " +
                    "on cs.contributorId=c.Id " +
                    "where cs.simchaId=@simchaId";
                cmd.Parameters.AddWithValue("@simchaId", simchaId);
                List<String> Names = new List<String>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    String name = (string)reader["Name"];
                    Names.Add(name);
                }
                return Names;
            }
        }
       



    }

}
