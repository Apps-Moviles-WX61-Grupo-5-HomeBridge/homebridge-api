namespace _3_Shared.Domain.Models.User;

public enum UserConstraints
{
    MaxPublicationBasicUser = 3,
    MaxPublicationPremiumUser = 9999999,
    
    TimeActiveInDaysBasicUser = 30,
    TimeActiveInDaysPremiumUser = 9999999,
    
    //  as percentage
    PublicationPriorityBasicUser = 50,
    PublicationPriorityPremiumUser = 100
}