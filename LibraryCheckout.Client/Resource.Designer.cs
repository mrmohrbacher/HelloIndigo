﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LendingLibrary.Client {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LendingLibrary.Client.Resource", typeof(Resource).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;Books&gt;
        ///  &lt;Book&gt;
        ///    &lt;Author&gt;Freeman,Eric;Freeman,Elisabeth&lt;/Author&gt;
        ///    &lt;ISBN&gt;0-596-00712-4&lt;/ISBN&gt;
        ///    &lt;Publisher&gt;O&apos;Reilly&lt;/Publisher&gt;
        ///    &lt;Title&gt;Head First Design Patterns&lt;/Title&gt;
        ///    &lt;Synopsis&gt;Get ahead of the competition.&lt;/Synopsis&gt;
        ///  &lt;/Book&gt;
        ///  &lt;Book&gt;
        ///    &lt;Author&gt;Musciano,Chuck;Kennedy, Bill&lt;/Author&gt;
        ///    &lt;ISBN&gt;1-56592-492-4&lt;/ISBN&gt;
        ///    &lt;Publisher&gt;O&apos;Reilly&lt;/Publisher&gt;
        ///    &lt;Title&gt;HTML: The Definitive Guide&lt;/Title&gt;
        ///    &lt;Synopsis&gt;A definitive, but dry loo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AppData_Books {
            get {
                return ResourceManager.GetString("AppData.Books", resourceCulture);
            }
        }
    }
}
