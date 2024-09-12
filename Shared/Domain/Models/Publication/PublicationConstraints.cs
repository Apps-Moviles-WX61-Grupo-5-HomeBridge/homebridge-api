namespace _3_Shared.Domain.Models.Publication;

public enum PublicationConstraints
{
    MaxTitleLength = 100,
    MinTitleLength = 10,
    
    MaxDescriptionLength = 1000, 
    MinDescriptionLength = 15,
    
    //  Price surely can't be less than 0, but
    //  it can be 0 if the publication's object to be sold is free.
    //  ~for some reason ;-;
    MinPrice = 0,   
}