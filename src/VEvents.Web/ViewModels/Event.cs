using VEvents.Data.Enums;
using VEvents.Web.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace VEvents.Web.ViewModels
{
    public class Event
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Display(Name = "Details")]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Details { get; set; }

        [Display(Name = "DateAndTime")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateAndTime { get; set; }

        [Display(Name = "Status")]
        [Required]
        [EnumDataType(typeof(EventStatus))]
        public EventStatus Status { get; set; }

        public ApplicationUser Publisher { get; set; }
    }
}
