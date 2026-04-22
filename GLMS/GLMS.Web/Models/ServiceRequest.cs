/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Web.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Contract")]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost (USD)")]
        public decimal CostUSD { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost (ZAR)")]
        public decimal CostZAR { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Display(Name = "Exchange Rate (USD/ZAR)")]
        public decimal ExchangeRate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
