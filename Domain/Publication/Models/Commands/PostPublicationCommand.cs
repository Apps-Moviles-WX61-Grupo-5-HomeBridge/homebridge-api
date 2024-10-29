using System.ComponentModel;
using Domain.Publication.Models.ValueObjects;

namespace _2_Domain.Publication.Models.Commands;

public class PostPublicationCommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string _Location_Address { get; set; }
    public int UserId { get; set; }
    public float TotalArea { get; set; }
    [DefaultValue(EPropertyType.Casa)] public int Type { get; set; }
    [DefaultValue(EOperation.Alquiler)] public int Operation { get; set; }
    public int DormitoryQuantity { get; set; }
    public int BathroomQuantity { get; set; }
    public int ParkingLotQuantity { get; set; }
    public int Antiquity { get; set; }
    
    public int Size { get; set; }
    
    public int Rooms { get; set; }
    
    public int Garages { get; set; }
}