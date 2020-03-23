﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fitodu.Resource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Validation {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Validation() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fitodu.Resource.Validation", typeof(Validation).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong email format.
        /// </summary>
        public static string EmailFormatError {
            get {
                return ResourceManager.GetString("EmailFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The email address is already taken.
        /// </summary>
        public static string EmailTaken {
            get {
                return ResourceManager.GetString("EmailTaken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid grant.
        /// </summary>
        public static string InvalidGrant {
            get {
                return ResourceManager.GetString("InvalidGrant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The limit has been reached – upgrade your account.
        /// </summary>
        public static string LimitReached {
            get {
                return ResourceManager.GetString("LimitReached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum allowed file size is {0} MB.
        /// </summary>
        public static string MaxFileSizeError {
            get {
                return ResourceManager.GetString("MaxFileSizeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong phone number.
        /// </summary>
        public static string PhoneNumberFormatError {
            get {
                return ResourceManager.GetString("PhoneNumberFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field must be between {1} and {2}.
        /// </summary>
        public static string RangeError {
            get {
                return ResourceManager.GetString("RangeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field is required.
        /// </summary>
        public static string RequiredError {
            get {
                return ResourceManager.GetString("RequiredError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your recovery password link expired.
        /// </summary>
        public static string ResetPasswordExpired {
            get {
                return ResourceManager.GetString("ResetPasswordExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field is too long.
        /// </summary>
        public static string StringLengthError {
            get {
                return ResourceManager.GetString("StringLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong format – only small letters and digits.
        /// </summary>
        public static string UrlFormatError {
            get {
                return ResourceManager.GetString("UrlFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The company name is already taken.
        /// </summary>
        public static string UrlTaken {
            get {
                return ResourceManager.GetString("UrlTaken", resourceCulture);
            }
        }
    }
}