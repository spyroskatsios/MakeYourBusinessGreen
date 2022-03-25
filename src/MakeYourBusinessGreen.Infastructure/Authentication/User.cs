﻿using Microsoft.AspNetCore.Identity;

namespace MakeYourBusinessGreen.Infastructure.Authentication;
public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
