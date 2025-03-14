﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ElContadorPampero.Models;

[Table("CuentaContable")]
public partial class CuentaContable
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(25)]
    [Unicode(false)]
    public string Codigo { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Tipo { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Saldo { get; set; }

    [InverseProperty("CuentaContable")]
    public virtual ICollection<DetalleAsientoContable> DetalleAsientoContables { get; set; } = new List<DetalleAsientoContable>();

    [InverseProperty("CuentaContable")]
    public virtual ICollection<Mayoreo> Mayoreos { get; set; } = new List<Mayoreo>();
}