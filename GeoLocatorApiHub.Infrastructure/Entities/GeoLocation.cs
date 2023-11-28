using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoLocatorApiHub.Infrastructure.Entities
{
    public class GeoLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(39)]
        [Column(TypeName = "varchar(39)")]
        public required string Ip { get; set; }

        [Required]
        [StringLength(4)]
        [Column(TypeName = "varchar(4)")]
        public required string Type { get; set; }

        [Required]
        [StringLength(2)]
        [Column(TypeName = "varchar(2)")]
        public required string ContinentCode { get; set; }

        [Required]
        [StringLength(100)]
        public required string ContinentName { get; set; }

        [Required]
        [StringLength(2)]
        [Column(TypeName = "varchar(2)")]
        public required string CountryCode { get; set; }

        [Required]
        [StringLength(100)]
        public required string CountryName { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public required string RegionCode { get; set; }

        [Required]
        [StringLength(100)]
        public required string RegionName { get; set; }

        [Required]
        [StringLength(100)]
        public required string City { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar(20)")]
        public required string Zip { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
