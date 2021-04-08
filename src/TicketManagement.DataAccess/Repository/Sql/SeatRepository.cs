using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.Sql
{
    public class SeatRepository : IRepository<Seat>, ISeatRepositoryExtension
    {
        private readonly DbConnection _connection;

        public SeatRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public int Insert(Seat obj)
        {
            if (obj != null)
            {
                int insertedId;

                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Seat (AreaId, Row, Number) VALUES (@areaid, @row, @number); SELECT CAST(scope_identity() as int)";
                    command.CommandType = System.Data.CommandType.Text;

                    var areaidParam = new SqlParameter("@areaid", obj.AreaId);
                    command.Parameters.Add(areaidParam);

                    var rowParam = new SqlParameter("@row", obj.Row);
                    command.Parameters.Add(rowParam);

                    var numberParam = new SqlParameter("@number", obj.Number);
                    command.Parameters.Add(numberParam);

                    _connection.Open();

                    insertedId = (int)command.ExecuteScalar();

                    _connection.Close();
                }

                return insertedId;
            }
            else
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public int Delete(int id)
        {
            int rowAffected = 0;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Seat WHERE Id=@id; SELECT @@ROWCOUNT";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                rowAffected = (int)command.ExecuteScalar();

                _connection.Close();
            }

            return rowAffected;
        }

        public List<Seat> FilterSeatsByLayoutId(int id)
        {
            var seats = new List<Seat>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "Sp_FilterSeatsByLayoutId";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var layoutParam = new SqlParameter("@layout", id);
                command.Parameters.Add(layoutParam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seats.Add(new Seat
                        {
                            Id = reader.GetInt32(0),
                            AreaId = reader.GetInt32(1),
                            Row = reader.GetInt32(2),
                            Number = reader.GetInt32(3),
                        });
                    }
                }

                _connection.Close();
            }

            return seats;
        }

        public List<Seat> FilterByRowAndNumberInArea(Seat obj)
        {
            if (obj != null)
            {
                var seats = new List<Seat>();
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Id] FROM Seat WHERE (Row=@row) And (Number=@number) And (AreaId=@area)";
                    command.CommandType = System.Data.CommandType.Text;

                    var rowParam = new SqlParameter("@row", obj.Row);
                    command.Parameters.Add(rowParam);

                    var numberParam = new SqlParameter("@number", obj.Number);
                    command.Parameters.Add(numberParam);

                    var areaParam = new SqlParameter("@area", obj.AreaId);
                    command.Parameters.Add(areaParam);

                    _connection.Open();

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seats.Add(new Seat
                            {
                                Id = reader.GetInt32(0),
                                AreaId = reader.GetInt32(1),
                                Row = reader.GetInt32(2),
                                Number = reader.GetInt32(3),
                            });
                        }
                    }

                    _connection.Close();
                }

                return seats;
            }
            else
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public Seat GetById(int id)
        {
            Seat seat = null;
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[AreaId],[Row],[Number] FROM Seat WHERE Id=@id";
                command.CommandType = System.Data.CommandType.Text;

                var idparam = new SqlParameter("@id", id);
                command.Parameters.Add(idparam);

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seat = new Seat
                        {
                            Id = reader.GetInt32(0),
                            AreaId = reader.GetInt32(1),
                            Row = reader.GetInt32(2),
                            Number = reader.GetInt32(3),
                        };
                    }
                }

                _connection.Close();
            }

            return seat;
        }

        public IQueryable<Seat> GetAll()
        {
            List<Seat> seats = new List<Seat>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT [Id],[AreaId],[Row],[Number] FROM Seat";
                command.CommandType = System.Data.CommandType.Text;

                _connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        seats.Add(new Seat
                        {
                            Id = reader.GetInt32(0),
                            AreaId = reader.GetInt32(1),
                            Row = reader.GetInt32(2),
                            Number = reader.GetInt32(3),
                        });
                    }
                }

                _connection.Close();
            }

            return seats.AsQueryable();
        }

        public int Update(Seat obj)
        {
            int rowsAffected = 0;
            if (obj != null)
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = "Update Seat SET AreaId=@areaid, Row=@row, Number=@number WHERE Id=@id; SELECT @@ROWCOUNT";
                    command.CommandType = System.Data.CommandType.Text;

                    var areaidParam = new SqlParameter("@areaid", obj.AreaId);
                    command.Parameters.Add(areaidParam);

                    var rowParam = new SqlParameter("@row", obj.Row);
                    command.Parameters.Add(rowParam);

                    var numberParam = new SqlParameter("@number", obj.Number);
                    command.Parameters.Add(numberParam);

                    var idparam = new SqlParameter("@id", obj.Id);
                    command.Parameters.Add(idparam);

                    _connection.Open();

                    rowsAffected = (int)command.ExecuteScalar();

                    _connection.Close();
                }

                return rowsAffected;
            }
            else
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }
    }
}
