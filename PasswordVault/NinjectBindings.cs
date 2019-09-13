﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
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
            Bind<IDatabase>().To<CsvDatabase>().InSingletonScope();
            Bind<IEncryptDecrypt>().To<EncryptDecrypt>();
            Bind<IMasterPassword>().To<MasterPassword>();
            Bind<IPasswordService>().To<PasswordService>().InSingletonScope();
            Bind<IMessageWriter>().To<WinFormsMessageWriter>();
            Bind<ILoginView>().To<LoginView>().InSingletonScope();
            Bind<IMainView>().To<MainView>().InSingletonScope();
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