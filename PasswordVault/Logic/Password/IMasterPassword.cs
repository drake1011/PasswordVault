﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IMasterPassword
    {
        UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password);
        string GetFormattedString(UserEncrypedData data);
        bool VerifyPassword(string password, string salt, string hash);
        string GenerateRandomKey();

    }
}
