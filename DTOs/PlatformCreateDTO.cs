using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
    public class PlatformCreateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Publisher { get; set; }
    }
}