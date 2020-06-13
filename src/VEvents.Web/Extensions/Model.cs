using System.Linq;
using System.Security.Claims;
using VEvents.Web.Data;
using VEvents.Web.Models;
using VEvents.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoEvent = VEvents.Data.Models.Event;

namespace VEvents.Web.Extensions
{
    public static class Model
    {
        public static User ToUserViewModel(this ApplicationUser applicationUser)
        {
            return new User
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                FullName = applicationUser.FullName,
                PhoneNumber = applicationUser.PhoneNumber
            };
        }

        public static ApplicationUser ToUserModel(this User userViewModel)
        {
            return new ApplicationUser
            {
                Id = userViewModel.Id,
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                FullName = userViewModel.FullName,
                PhoneNumber = userViewModel.PhoneNumber
            };
        }

        public static Event ToEventViewModel(this MongoEvent @event, ApplicationDbContext context)
        {
            return new Event
            {
                Id = @event.Id,
                Title = @event.Title,
                Details = @event.Details,
                DateAndTime = @event.DateAndTime,
                Status = @event.Status,
                LikersCount = @event.LikersCount,
                Liked = @event.Liked,
                Publisher = context.Users.Find(@event.PublisherId)
            };
        }

        public static MongoEvent ToEventModel(this Event eventViewModel)
        {
            return new MongoEvent
            {
                Id = eventViewModel.Id,
                Title = eventViewModel.Title,
                Details = eventViewModel.Details,
                DateAndTime = eventViewModel.DateAndTime,
                Status = eventViewModel.Status,
                PublisherId = eventViewModel.Publisher?.Id
            };
        }
    }
}
