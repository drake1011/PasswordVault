﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* Serves as the bridge between the database and the presenters
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/
    public enum LoginResult
    {
        PasswordIncorrect, 
        UsernameDoesNotExist,
        Successful,
        UnSuccessful
    }

    public enum CreateUserResult
    {
        UsernameTaken,
        UsernameNotValid,
        PasswordNotValid,
        Successful,
        Unsuccessful,
    }

    public enum AddPasswordResult
    {
        DuplicatePassword,
        Failed,
        Success,
    }

    public enum DeletePasswordResult
    {
        PasswordDoesNotExist,
        Success,
        Failed
    }

    public enum LogOutResult
    {
        Success,
        Failed
    }

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class PasswordService : IPasswordService
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const int DEFAULT_PASSWORD_LENGTH = 15;
        private const int MINIMUM_PASSWORD_LENGTH = 8;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private User _currentUser;                       // Current user's username and password
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview
        private IDatabase _dbcontext;                    // Method of storing the passwords (ie. csv file or database)
        private IMasterPassword _masterPassword;
        private IEncryptDecrypt _encryptDecrypt;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public PasswordService(IDatabase dbcontext, IMasterPassword masterPassword, IEncryptDecrypt encryptDecrypt)
        {
            _dbcontext = dbcontext;
            _masterPassword = masterPassword;
            _encryptDecrypt = encryptDecrypt;

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public LoginResult Login(string username, string password)
        {
            LoginResult loginResult = LoginResult.UnSuccessful;

            if (!IsLoggedIn())
            {
                // Perform user login verification
                if (!_dbcontext.UserExists(username))
                {
                    loginResult = LoginResult.UsernameDoesNotExist;
                }
                else
                {
                    User user = _dbcontext.GetUser(username);

                    bool valid = _masterPassword.VerifyPassword(password, user.Salt, user.Hash); // Hash password with user.Salt and compare to user.Hash

                    if (valid)
                    {
                        _currentUser = new User(username, user.Salt, user.Hash, password, true);
                    }
                    else
                    {
                        loginResult = LoginResult.PasswordIncorrect;
                        _currentUser = new User(false);
                    }
                }

                // Set table name and read passwords
                if (_currentUser.ValidKey)
                {
                    loginResult = LoginResult.Successful;
                    UpdatePasswordListFromDB();
                }
            }

            return loginResult;
        }

        /*************************************************************************************************/
        public LogOutResult Logout()
        {
            LogOutResult result = LogOutResult.Failed;

            if (IsLoggedIn())
            {
                _passwordList.Clear();
                _currentUser = new User(false);

                result = LogOutResult.Success;
            }

            return result;        
        }

        /*************************************************************************************************/
        public bool IsLoggedIn()
        {
            if (_currentUser.ValidKey)
            {
                return true;
            }

            return false;
        }

        /*************************************************************************************************/
        public void DeleteUser()
        {
            if (IsLoggedIn())
            {

            }

            throw new NotImplementedException();
        }

        /*************************************************************************************************/
        public string GetCurrentUserID()
        {
            string user = "";

            if (IsLoggedIn())
            {
                user = _currentUser.UserID;
            }

            return user;
        }

        /*************************************************************************************************/
        public void ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            if (IsLoggedIn())
            {

            }

            throw new NotImplementedException();
        }

        /*************************************************************************************************/
        public AddPasswordResult AddPassword(Password password)
        {
            AddPasswordResult addResult = AddPasswordResult.Failed;

            if (IsLoggedIn())
            {
                List<Password> result = (from Password pass in _passwordList
                                         where pass.Application == password.Application
                                         select pass).ToList<Password>();

                if (result.Count <= 0) // Verify that new password is not a duplicate of an existing one
                {
                    Password encryptPassword = ConvertPlaintextPasswordToEncryptedPassword(password); // Need to first encrypt the password
                    _dbcontext.AddPassword(ConvertToEncryptedDatabasePassword(encryptPassword)); // Add the encrypted password to the database

                    // Update the passwordservice list.
                    // This solves issue when deleting a newly added password where the unique ID hasn't been updated in the service.
                    // since the database autoincrements the unique ID.
                    UpdatePasswordListFromDB(); 

                    addResult = AddPasswordResult.Success;
                }
                else
                {
                    addResult = AddPasswordResult.DuplicatePassword;
                }
            }

            return addResult;
        }

        /*************************************************************************************************/
        public void RemovePassword(Password password)
        {
            if (IsLoggedIn())
            {
                Password result = (from Password pass in _passwordList
                                   where pass.Application == password.Application
                                   where pass.Username == password.Username
                                   where pass.Description == password.Description
                                   where pass.Website == password.Website
                                   select pass).First();

                _passwordList.Remove(result);
                _dbcontext.DeletePassword(ConvertToEncryptedDatabasePassword(result));
            }

            // TODO - 1 - Should return result of remove password
        }

        /*************************************************************************************************/
        public AddPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword)
        {
            AddPasswordResult result = AddPasswordResult.Failed;

            if (IsLoggedIn())
            {
                Password modifiedEncryptedPassword = ConvertPlaintextPasswordToEncryptedPassword(modifiedPassword);

                int index = _passwordList.FindIndex(x => (x.Application == originalPassword.Application) && (x.Username == originalPassword.Username) && (x.Description == originalPassword.Description) && (x.Website == originalPassword.Website));

                if (index != -1)
                {
                    _passwordList[index] = modifiedEncryptedPassword;
                    _dbcontext.ModifyPassword(ConvertToEncryptedDatabasePassword(originalPassword), ConvertToEncryptedDatabasePassword(modifiedEncryptedPassword));
                    result = AddPasswordResult.Success;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public List<Password> GetPasswords()
        {
            return _passwordList;
        }

        /*************************************************************************************************/
        public Password DecryptPassword(Password password)
        {
            Password decryptedPassword;

            decryptedPassword = DecryptEncryptedPassword(password);

            return decryptedPassword;
        }

        /*************************************************************************************************/
        public CreateUserResult CreateNewUser(string username, string password)
        {
            CreateUserResult createUserResult = CreateUserResult.Unsuccessful;
            User user = _dbcontext.GetUser(username);

            if (user != null)
            {
                createUserResult = CreateUserResult.UsernameTaken;
            }
            else
            {
                // Verify that username and password pass requirements
                if (username == null || username == "")
                {
                    createUserResult = CreateUserResult.UsernameNotValid;
                }
                else if (password == null || password == "" || password.Length < MINIMUM_PASSWORD_LENGTH) // TODO - 2 - Check if password contains special characters
                {
                    createUserResult = CreateUserResult.PasswordNotValid;
                }
                else
                {
                    createUserResult = CreateUserResult.Successful;
                    CryptData_S newPassword = _masterPassword.HashPassword(password);
                    _dbcontext.AddUser(username, newPassword.Salt, newPassword.Hash);
                }
            }

            return createUserResult;
        }

        /*************************************************************************************************/
        public int GetMinimumPasswordLength()
        {
            return MINIMUM_PASSWORD_LENGTH;
        }

        /*************************************************************************************************/
        public string GeneratePasswordKey()
        {
            return _encryptDecrypt.CreateKey(DEFAULT_PASSWORD_LENGTH);
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            foreach (var item in _dbcontext.GetUserPasswords(_currentUser.UserID))
            {
                // Add encrypted password to _passwordList
                Password password = new Password(
                    item.UniqueID,
                    _encryptDecrypt.Decrypt(item.Application, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Username, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Description, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Website, _currentUser.Key),
                    item.Passphrase // Leave the password encrypted
                    );

                _passwordList.Add(password);
            }
        }

        /*************************************************************************************************/
        private DatabasePassword ConvertToEncryptedDatabasePassword(Password password)
        {
            return new DatabasePassword(
                password.UniqueID,
                _currentUser.UserID, // TODO - 7 - Change to unique ID - Use unencrypted username for now
                _encryptDecrypt.Encrypt(password.Application, _currentUser.Key),
                _encryptDecrypt.Encrypt(password.Username, _currentUser.Key),
                _encryptDecrypt.Encrypt(password.Description, _currentUser.Key),
                _encryptDecrypt.Encrypt(password.Website, _currentUser.Key),
                password.Passphrase // Password is already encrypted
                );
        }

        /*************************************************************************************************/
        private Password ConvertPlaintextPasswordToEncryptedPassword(Password password)
        {
            return new Password(
                password.UniqueID,
                password.Application,
                password.Username, 
                password.Description,
                password.Website, 
                _encryptDecrypt.Encrypt(password.Passphrase, _currentUser.Key) 
                );
        }

        /*************************************************************************************************/
        private Password DecryptEncryptedPassword(Password password)
        {
            return new Password(
                password.UniqueID,
                password.Application,
                password.Username,
                password.Description,
                password.Website,
                _encryptDecrypt.Decrypt(password.Passphrase, _currentUser.Key)
    );
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // PasswordWrapper CLASS
} // PasswordHashTest NAMESPACE
