using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Event
{
    public class EventDataViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(120, ErrorMessage = "NameLength", MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(120, ErrorMessage = "DescriptionLength", MinimumLength = 4)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "LayoutId")]
        public int LayoutId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "StartDateTime")]
        public DateTime StartDateTime { get; set; }

        [Required]
        [Display(Name = "EndDateTime")]
        public DateTime EndDateTime { get; set; }
    }
}
