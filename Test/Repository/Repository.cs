
using Microsoft.Data.SqlClient;
using System.Data;
using Test.model;

namespace Test.Repository
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        public async Task<int> Add(Employee employee)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            //using SqlCommand cmd = new SqlCommand(@"Insert into Employee values(@Id,@Name,@Email,@Department,@Salary,@JoinDate);select scope_Identity() ", con);
            string query = @"
        INSERT INTO Employee
        (Name, Email, Department, Salary, JoinDate)
        VALUES
        (@Name, @Email, @Department, @Salary, @JoinDate);

         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand cmd = new(query, con);
            //cmd.Parameters.AddWithValue("@Id", employee.Id);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@JoinDate", employee.JoinDate);
            await con.OpenAsync();
            //await cmd.ExecuteNonQueryAsync();
            //return employee;
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<bool> Delete(int id)
        { 
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("Delete from Employee where Id=@Id  ", con);
            cmd.Parameters.AddWithValue("@Id", id);
            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Employee>> GetAll()
        {
            List<Employee> Employees = new List<Employee>();
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("Select Id, Name,Email,Department,Salary,JoinDate From Employee", con);
            await con.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Employees.Add(new Employee
                {
                    //Id = (Guid)reader["Id"],
                    Id = Convert.ToInt32(reader["Id"]),
                   Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Department = reader["Department"].ToString(),
                    Salary =Convert.ToDecimal(reader["Salary"]),
                    JoinDate =(DateTime)(reader["JoinDate"])
                });
            }
            return Employees;

        }

        public async Task<Employee?> GetById(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(@"Select Id,Name,Email,Department,Salary,JoinDate From Employee where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            await con.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;
                return new Employee
                {
                    //Id = (Guid)reader["Id"],
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Department = reader["Department"].ToString(),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    JoinDate = (DateTime)(reader["JoinDate"])
                   
                };
                
            
            
        }

        public async Task<bool> Update(int id, Employee employee)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(@"Update Employee set Name=@Name,Email=@Email,Department=@Department,Salary=@Salary,JoinDate=@JoinDate where Id=@Id  ", con);
            cmd.Parameters.AddWithValue("@Id",id);
            await con.OpenAsync();
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@JoinDate", employee.JoinDate);
            
           return await cmd.ExecuteNonQueryAsync() > 0;
            
        }

        // GET ALL
        //public async Task<List<Employee>> GetAll()
        //{
        //    List<Employee> employees = new();

        //    using SqlConnection con = new(_connectionString);
        //    using SqlCommand cmd = new(
        //        "SELECT Id,Name,Email,Department,Salary,JoinDate FROM Employee", con);

        //    await con.OpenAsync();
        //    using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        //    while (await reader.ReadAsync())
        //    {
        //        employees.Add(new Employee
        //        {
        //            Id = (Guid)reader["Id"],
        //            Name = reader["Name"].ToString(),
        //            Email = reader["Email"].ToString(),
        //            Department = reader["Department"] == DBNull.Value
        //                ? null : reader["Department"].ToString(),
        //            Salary = Convert.ToDecimal(reader["Salary"]),
        //            JoinDate = (DateTime)reader["JoinDate"]
        //        });
        //    }

        //    return employees;
        //}

        //// GET BY ID
        //public async Task<Employee?> GetById(Guid id)
        //{
        //    using SqlConnection con = new(_connectionString);
        //    using SqlCommand cmd = new(
        //        "SELECT Id,Name,Email,Department,Salary,JoinDate FROM Employee WHERE Id=@Id", con);

        //    cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

        //    await con.OpenAsync();
        //    using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        //    if (!await reader.ReadAsync())
        //        return null;

        //    return new Employee
        //    {
        //        Id = (Guid)reader["Id"],
        //        Name = reader["Name"].ToString(),
        //        Email = reader["Email"].ToString(),
        //        Department = reader["Department"] == DBNull.Value
        //            ? null : reader["Department"].ToString(),
        //        Salary = Convert.ToDecimal(reader["Salary"]),
        //        JoinDate = (DateTime)reader["JoinDate"]
        //    };
        //}

        //// CREATE
        //public async Task<Employee> Add(Employee employee)
        //{
        //    employee.Id = Guid.NewGuid();

        //    using SqlConnection con = new(_connectionString);
        //    using SqlCommand cmd = new(@"
        //        INSERT INTO Employee
        //        VALUES(@Id,@Name,@Email,@Department,@Salary,@JoinDate)", con);

        //    cmd.Parameters.AddWithValue("@Id", employee.Id);
        //    cmd.Parameters.AddWithValue("@Name", employee.Name);
        //    cmd.Parameters.AddWithValue("@Email", employee.Email);
        //    cmd.Parameters.AddWithValue("@Department",
        //        employee.Department ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
        //    cmd.Parameters.AddWithValue("@JoinDate", employee.JoinDate);

        //    await con.OpenAsync();
        //    await cmd.ExecuteNonQueryAsync();

        //    return employee;
        //}

        //// UPDATE
        //public async Task<bool> Update(Guid id, Employee employee)
        //{
        //    using SqlConnection con = new(_connectionString);
        //    using SqlCommand cmd = new(@"
        //        UPDATE Employee
        //        SET Name=@Name, Email=@Email, Department=@Department,
        //            Salary=@Salary, JoinDate=@JoinDate
        //        WHERE Id=@Id", con);

        //    cmd.Parameters.AddWithValue("@Id", id);
        //    cmd.Parameters.AddWithValue("@Name", employee.Name);
        //    cmd.Parameters.AddWithValue("@Email", employee.Email);
        //    cmd.Parameters.AddWithValue("@Department",
        //        employee.Department ?? (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
        //    cmd.Parameters.AddWithValue("@JoinDate", employee.JoinDate);

        //    await con.OpenAsync();

        //    return await cmd.ExecuteNonQueryAsync() > 0;
        //}

        //// DELETE
        //public async Task<bool> Delete(Guid id)
        //{
        //    using SqlConnection con = new(_connectionString);
        //    using SqlCommand cmd = new(
        //        "DELETE FROM Employee WHERE Id=@Id", con);

        //    cmd.Parameters.AddWithValue("@Id", id);

        //    await con.OpenAsync();

        //    return await cmd.ExecuteNonQueryAsync() > 0;
        //}


    }
}

