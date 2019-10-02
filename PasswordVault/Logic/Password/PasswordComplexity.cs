﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* Source: https://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=4721
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Desktop.Winforms
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/
    public enum PasswordComplexityLevel
    {
        Weak,
        Mediocre,
        Ok,
        Great,
    }

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public static class PasswordComplexity
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
  		public static PasswordComplexityLevel checkEffectiveBitSize(int passSize, string pass)
  		{
  			int charSet = 0;
            PasswordComplexityLevel passStrength = PasswordComplexityLevel.Weak;
  
  			charSet = getCharSetUsed(pass);
  
  			double result = Math.Log(Math.Pow(charSet, passSize)) / Math.Log(2);
  
  			if(result <= 32 )
  			{
  				passStrength = PasswordComplexityLevel.Weak;
  			}
  			else if(result <= 64 )
  			{
  				passStrength = PasswordComplexityLevel.Mediocre;
  			}
  			else if(result <= 128 )
  			{
  				passStrength = PasswordComplexityLevel.Ok;
  			}
  			else if(result > 128 )
  			{
  				passStrength = PasswordComplexityLevel.Great;
  			}

            return passStrength;

        }
        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
  		private static int getCharSetUsed(string pass)
  		{
  			int ret = 0;
  
  			if(containsNumbers(pass ) )
  			{
  				ret += 10;
  			}
  
  			if(containsLowerCaseChars(pass ) )
  			{
  				ret += 26;
  			}
  
  			if(containsUpperCaseChars(pass ) )
  			{
  				ret += 26;
  			}
  
  			if(containsPunctuation(pass ) )
  			{
  				ret += 31;
  			}
  
  			return ret;
  		}

        /*************************************************************************************************/
        private static bool containsNumbers(string str)
  		{
  			Regex pattern = new Regex(@"[\d]");
  			return pattern.IsMatch(str );
  		}

        /*************************************************************************************************/
        private static bool containsLowerCaseChars(string str)
  		{
  			Regex pattern = new Regex("[a-z]");
  			return pattern.IsMatch(str );
  		}

        /*************************************************************************************************/
        private static bool containsUpperCaseChars(string str)
  		{
  			Regex pattern = new Regex("[A-Z]");
  			return pattern.IsMatch(str );
  		}

        /*************************************************************************************************/
        private static bool containsPunctuation(string str)
  		{
  			// regular expression include _ as a valid char for alphanumeric.. 
  			// so we need to explicity state that its considered punctuation.
  			Regex pattern = new Regex(@"[\W|_]");
  			return pattern.IsMatch(str );
  		}

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // PasswordComplexity CLASS
} // PasswordHashTest NAMESPACE
