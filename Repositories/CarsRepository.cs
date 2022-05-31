using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GregsListAgain.Models;

namespace GregsListAgain.Repositories
{
    public class CarsRepository
    {
        private readonly IDbConnection _db;

        public CarsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal List<Car> Get()
        {
            string sql = @"
            SELECT
            c.*,
            acct.*
            FROM car c
            JOIN account acct ON c.creatorId = acct.id;
            ";
            return _db.Query<Car, Account, Car>(sql, (car, account) =>
            {
                car.Creator = account;
                return car;
            }).ToList();
        }
        internal List<Car> Get(int id)
        {
            string sql = @"
            SELECT
            c.*,
            acct.*
            FROM car c
            JOIN accounts acct ON c.creatorId = acct.id
            WHERE c.id = @id
            ";
            return _db.Query<Car, Account, Car>(sql, (car, account) =>
            {
                car.Creator = account;
                return car;
            }, new { id }).FirstOrDefault();
        }

        internal Car Create(Car carData)
        {
            string sql = @"
            INSERT INTO cars
            (make, model, imgUrl, creatorId)
            VALUES
            (@Make, @Model, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
            carData.Id = _db.ExecuteScalar<int>(sql, carData);
            return carData;
        }

        internal void Edit(Car original)
        {
            string sql = @"
            UPDATE cars
            SET
            make = @Make,
            model = @Model,
            imgUrl = @ImgUrl,
            WHERE id = @Id;
            ";
            _db.Execute(sql, original);
        }

        internal void Delete(int id)
        {
            string sql = "DELETE FROM cars WHERE id = @id LIMIT 1";
            _db.Execute(sql, new { id });
        }
    }
}