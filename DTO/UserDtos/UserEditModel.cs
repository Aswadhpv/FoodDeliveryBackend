﻿namespace FoodDeliveryBackend.DTOs
{
public class UserEditModel
{
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}
}