﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ElContadorPampero.Models;

[Table("DetalleAsientoContable")]
public partial class DetalleAsientoContable
{
    [Key]
    public int Id { get; set; }

    public int AsientoContableId { get; set; }

    public int CuentaContableId { get; set; }

    [Required]
    [StringLength(5)]
    [Unicode(false)]
    public string Cargo { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Monto { get; set; }

    [ForeignKey("AsientoContableId")]
    [InverseProperty("DetalleAsientoContables")]
    public virtual AsientoContable AsientoContable { get; set; }

    [ForeignKey("CuentaContableId")]
    [InverseProperty("DetalleAsientoContables")]
    public virtual CuentaContable CuentaContable { get; set; }
}