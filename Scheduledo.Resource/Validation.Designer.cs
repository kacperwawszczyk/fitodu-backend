﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scheduledo.Resource {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Validation {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Validation() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Scheduledo.Resource.Validation", typeof(Validation).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string EmailFormatError {
            get {
                return ResourceManager.GetString("EmailFormatError", resourceCulture);
            }
        }
        
        public static string PhoneNumberFormatError {
            get {
                return ResourceManager.GetString("PhoneNumberFormatError", resourceCulture);
            }
        }
        
        public static string RequiredError {
            get {
                return ResourceManager.GetString("RequiredError", resourceCulture);
            }
        }
        
        public static string StringLengthError {
            get {
                return ResourceManager.GetString("StringLengthError", resourceCulture);
            }
        }
        
        public static string RangeError {
            get {
                return ResourceManager.GetString("RangeError", resourceCulture);
            }
        }
        
        public static string InvalidGrant {
            get {
                return ResourceManager.GetString("InvalidGrant", resourceCulture);
            }
        }
        
        public static string EmailTaken {
            get {
                return ResourceManager.GetString("EmailTaken", resourceCulture);
            }
        }
        
        public static string UrlTaken {
            get {
                return ResourceManager.GetString("UrlTaken", resourceCulture);
            }
        }
        
        public static string UrlFormatError {
            get {
                return ResourceManager.GetString("UrlFormatError", resourceCulture);
            }
        }
        
        public static string ResetPasswordExpired {
            get {
                return ResourceManager.GetString("ResetPasswordExpired", resourceCulture);
            }
        }
        
        public static string LimitReached {
            get {
                return ResourceManager.GetString("LimitReached", resourceCulture);
            }
        }
        
        public static string MaxFileSizeError {
            get {
                return ResourceManager.GetString("MaxFileSizeError", resourceCulture);
            }
        }
    }
}
