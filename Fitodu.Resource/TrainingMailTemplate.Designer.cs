//------------------------------------------------------------------------------
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
    public class TrainingMailTemplate {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TrainingMailTemplate() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fitodu.Resource.TrainingMailTemplate", typeof(TrainingMailTemplate).Assembly);
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
        ///   Looks up a localized string similar to Hi -coachName-! &lt;br&gt;&lt;br&gt; Your workout with -clientName-, starting at -date-, has just been cancelled by your client. Head to &lt;a href=&quot;-url-&quot;&gt;FITODU&lt;/a&gt; to plan your workout at a different date!.
        /// </summary>
        public static string TrainingClientWithdrawalBody {
            get {
                return ResourceManager.GetString("TrainingClientWithdrawalBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fitodu - your workout has been cancelled.
        /// </summary>
        public static string TrainingClientWithdrawalSubject {
            get {
                return ResourceManager.GetString("TrainingClientWithdrawalSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi -clientName-! &lt;br&gt;&lt;br&gt; Your workout starting at -date-, has just been cancelled by your coach. Head to &lt;a href=&quot;-url-&quot;&gt;FITODU&lt;/a&gt; to plan your workout at a different date!.
        /// </summary>
        public static string TrainingCoachWithdrawalBody {
            get {
                return ResourceManager.GetString("TrainingCoachWithdrawalBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fitodu - your workout has been cancelled.
        /// </summary>
        public static string TrainingCoachWithdrawalSubject {
            get {
                return ResourceManager.GetString("TrainingCoachWithdrawalSubject", resourceCulture);
            }
        }
    }
}
