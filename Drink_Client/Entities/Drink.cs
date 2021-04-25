using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Drink_Client.Entities
{
    public class Drink : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }
        public int Count_Sell { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
    public partial class Drink_Repository
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConSTR"].ConnectionString;


        public void Create(Drink value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        var sqlQuery = "INSERT INTO [dbo].[Drink] (Name, Price, Amount, Count_Sell)" +
                                 "VALUES(@Name, @Price, @Amount, @Count_Sell);";
                        db.Execute(sqlQuery, new
                        {
                            Name = value.Name,
                            Price = value.Price,
                            Amount = value.Amount,
                            Count_Sell = 0

                        }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void Delete(Drink value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE  FROM [dbo].[Drink] WHERE Id = @Id";
                        db.Execute(sqlQuery, new { value.Id }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }



        public IEnumerable<Drink> GetColl()
        {
            List<Drink> coll = new List<Drink>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Drink>("SELECT * FROM [dbo].[Drink]").ToList();
            }
            return coll;
        }

        public void Update(Drink value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  [dbo].[Drink] SET Name = @Name , Price = @Price, Amount = @Amount, Count_Sell = @Count_Sell" +
                            " WHERE Id = @Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Id = value.Id,
                               Name = value.Name,
                               Price = value.Price,
                               Amount = value.Amount,
                               Count_Sell = value.Count_Sell
                           },
                           transaction);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}

