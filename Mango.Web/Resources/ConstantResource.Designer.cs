﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mango.Web.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ConstantResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ConstantResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Mango.Web.Resources.ConstantResource", typeof(ConstantResource).Assembly);
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
        ///   Looks up a localized string similar to ADMIN.
        /// </summary>
        public static string AdminRole {
            get {
                return ResourceManager.GetString("AdminRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CUSTOMER.
        /// </summary>
        public static string CustomerRole {
            get {
                return ResourceManager.GetString("CustomerRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller.
        /// </summary>
        public static string ReplaceString {
            get {
                return ResourceManager.GetString("ReplaceString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to role.
        /// </summary>
        public static string RoleType {
            get {
                return ResourceManager.GetString("RoleType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to error.
        /// </summary>
        public static string Tempdata_Error {
            get {
                return ResourceManager.GetString("Tempdata_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to success.
        /// </summary>
        public static string Tempdata_Success {
            get {
                return ResourceManager.GetString("Tempdata_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JWTToken.
        /// </summary>
        public static string TokenCookie {
            get {
                return ResourceManager.GetString("TokenCookie", resourceCulture);
            }
        }
    }
}
