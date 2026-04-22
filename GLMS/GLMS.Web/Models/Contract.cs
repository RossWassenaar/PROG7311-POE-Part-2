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
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Draft";

        [Required]
        [Display(Name = "Service Level")]
        public string ServiceLevel { get; set; } = "Standard";

        [Display(Name = "Signed Agreement")]
        public string? SignedAgreementPath { get; set; }

        [Display(Name = "File Name")]
        public string? SignedAgreementFileName { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}
