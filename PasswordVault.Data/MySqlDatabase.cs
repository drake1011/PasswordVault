﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Data
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class MySQLDatabase : IDatabase
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MySQLDatabase()
        {

        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public bool AddPassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeletePassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            throw new NotImplementedException();
        }

        public bool ModifyUser(User user, User modifiedUser)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // Database CLASS
} // PasswordVault.Data NAMESPACE
