﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ElContadorPampero.Models;

[Table("BalanceGeneral")]
public partial class BalanceGeneral
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal TotalActivos { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal TotalPasivos { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal TotalPatrimonio { get; set; }
}