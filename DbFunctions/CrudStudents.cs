using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql;

using dotnetApi;
public class CRUDStudents
{
    private readonly DataBaseConnect _db;

    public CRUDStudents(DataBaseConnect db)
    {
        _db = db;
    }

    public List<Student> GetAllStudents()
    {
        var Allstudents = _db.Students.ToList();

        return Allstudents;
    }
}

