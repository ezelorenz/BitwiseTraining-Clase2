using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUDclase2.Models;

public partial class Cliente
{
    
    public int Id { get; set; }
    
    public string Nombre { get; set; }
    
    public string Direccion { get; set; }
    
    public string Ciudad { get; set; }
    
    public string Telefono { get; set; }
    
    public bool? Duedor { get; set; }
    
    public decimal? MontoDeuda { get; set; }
}
