using System.Security.Cryptography;
using BooksLibrary.Controllers;
namespace ProjectWorkAPI.Models;

public partial class Table
{
    public void close()
    {
        Occupied = false;
        OccupancyDate = null;
        Occupants = null;
        TableKey = null;
    }
    public void open(int occupants)
    {
        Occupied = true;
        OccupancyDate = DateTime.Now;
        Occupants = occupants;
        TableKey = CalculateTableKey();
    }

    private string CalculateTableKey()
    {
        return SecurityController.CalculateNewApiKey();
    }
}