﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Data;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
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
    public class NinjectBindings : Ninject.Modules.NinjectModule
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

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public override void Load()
        {
            Bind<ICSVReader>().To<CSVReader>();
            Bind<ICSVWriter>().To<CSVWriter>();
            Bind<ICSVPasswordManager>().To<CSVPasswordManager>();
            Bind<ICSVUserManager>().To<CSVUserManager>();
            Bind<IDatabase>().To<SQLiteDatabase>().InSingletonScope();
            Bind<IEncryption>().To<RijndaelManagedEncryption>();
            Bind<IMasterPassword>().To<MasterPassword>();
            Bind<IPasswordService>().To<PasswordService>().InSingletonScope();
            Bind<ILoginView>().To<LoginView>().InSingletonScope();
            Bind<IMainView>().To<MainView>().InSingletonScope();
            Bind<IChangePasswordView>().To<ChangePasswordView>().InSingletonScope();
            Bind<IEditUserView>().To<EditUserView>().InSingletonScope();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // NinjectBindings CLASS
} // PasswordVault NAMESPACE
