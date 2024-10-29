using System.ComponentModel;

namespace _2_Domain.Publication.Models.Commands;

public class PostPublicationCommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string _Location_Address { get; set; }
    public int UserId { get; set; }
    public float CoveredArea { get; set; }
    public float TotalArea { get; set; }
    [DefaultValue("Casa")] public string Type { get; set; }
    [DefaultValue("Venta")] public string Operation { get; set; }
    public string Delivery { get; set; }
    public int DormitoryQuantity { get; set; }
    public int BathroomQuantity { get; set; }
    public int ParkingLotQuantity { get; set; }
    [DefaultValue("Disponible")] public string SaleState { get; set; }
    [DefaultValue("Iniciado")] public string ProjectStage { get; set; }
    public DateTime ProjectStartDate { get; set; }
    public int Antiquity { get; set; }
    
    public int Size { get; set; }
    
    public int Rooms { get; set; }
    
    public int Garages { get; set; }
}