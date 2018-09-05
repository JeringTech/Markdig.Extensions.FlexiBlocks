﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jering.Markdig.Extensions.FlexiBlocks {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Jering.Markdig.Extensions.FlexiBlocks.Strings", typeof(Strings).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to Base URI must be absolute , received &quot;{0}&quot;..
        /// </summary>
        internal static string ArgumentException_BaseUriMustBeAbsolute {
            get {
                return ResourceManager.GetString("ArgumentException_BaseUriMustBeAbsolute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Since source &quot;{0}&quot; is not an absolute URI, base URI cannot be null, white space or an empty string..
        /// </summary>
        internal static string ArgumentException_BaseUriMustBeDefinedIfSourceIsNotAnAbsoluteUri {
            get {
                return ResourceManager.GetString("ArgumentException_BaseUriMustBeDefinedIfSourceIsNotAnAbsoluteUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The directory to be used for file caching, &quot;{0}&quot; is invalid. Refer to the inner exception for details. .
        /// </summary>
        internal static string ArgumentException_InvalidCacheDirectory {
            get {
                return ResourceManager.GetString("ArgumentException_InvalidCacheDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; has the value &quot;{1}&quot;, which is not a valid value for the enum &quot;{2}&quot;..
        /// </summary>
        internal static string ArgumentException_InvalidEnumArgument {
            get {
                return ResourceManager.GetString("ArgumentException_InvalidEnumArgument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; must be defined..
        /// </summary>
        internal static string ArgumentException_MustBeDefined {
            get {
                return ResourceManager.GetString("ArgumentException_MustBeDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is not a valid  URI ..
        /// </summary>
        internal static string ArgumentException_NotAValidUri {
            get {
                return ResourceManager.GetString("ArgumentException_NotAValidUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one of &quot;{0}&quot; of &quot;{1}&quot; can be defined..
        /// </summary>
        internal static string ArgumentException_OnlyOneArgumentCanBeDefined {
            get {
                return ResourceManager.GetString("ArgumentException_OnlyOneArgumentCanBeDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Text has length &quot;{0}&quot;, start char index &quot;{1}&quot; is out of range..
        /// </summary>
        internal static string ArgumentException_StartCharIndexIsOutOfRange {
            get {
                return ResourceManager.GetString("ArgumentException_StartCharIndexIsOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  URI  &quot;{0}&quot; has unsupported scheme &quot;{1}&quot;..
        /// </summary>
        internal static string ArgumentException_UriSchemeUnsupported {
            get {
                return ResourceManager.GetString("ArgumentException_UriSchemeUnsupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot not be null or an empty string..
        /// </summary>
        internal static string ArgumentException_ValueCannotBeNullOrAnEmptyString {
            get {
                return ResourceManager.GetString("ArgumentException_ValueCannotBeNullOrAnEmptyString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot be null, white space or an empty string..
        /// </summary>
        internal static string ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString {
            get {
                return ResourceManager.GetString("ArgumentException_ValueCannotBeNullWhiteSpaceOrAnEmptyString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot be negative or greater than the number of empty elements in buffer..
        /// </summary>
        internal static string ArgumentOutOfRangeException_CountCannotBeNegativeOrGreaterThanTheNumberOfEmptyElementsInBuffer {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_CountCannotBeNegativeOrGreaterThanTheNumberOfEmptyEle" +
                        "mentsInBuffer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to End line number &quot;{0}&quot; with associated start line number &quot;{1}&quot; is invalid. Unless an end line number is -1, it cannot be less than its associated start line number..
        /// </summary>
        internal static string ArgumentOutOfRangeException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartLineNumber {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_EndLineNumberMustBeMinus1OrGreaterThanOrEqualToStartL" +
                        "ineNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; is not a valid line number. Line numbers must be greater than 0..
        /// </summary>
        internal static string ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0 {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_LineNumberMustBeGreaterThan0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot be negative. The specified value &quot;{0}&quot; is invalid..
        /// </summary>
        internal static string ArgumentOutOfRangeException_ValueCannotBeNegative {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_ValueCannotBeNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be within range {0}. The specified value &quot;{1}&quot; is invalid..
        /// </summary>
        internal static string ArgumentOutOfRangeException_ValueMustBeWithinRange {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_ValueMustBeWithinRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be within the interval containing &quot;{0}&quot;&apos;s indices..
        /// </summary>
        internal static string ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeException_ValueMustBeWithinTheIntervalContainingBuffersIndices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple attempts retrieve content from &quot;{0}&quot; have failed. Please ensure that the Url is valid and that your network connection is stable. Enable debug level logging and try again for more information on why requests are failing..
        /// </summary>
        internal static string ContentRetrieverException_FailedAfterMultipleAttempts {
            get {
                return ResourceManager.GetString("ContentRetrieverException_FailedAfterMultipleAttempts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Access to the remote Url &quot;{0}&quot; is forbidden..
        /// </summary>
        internal static string ContentRetrieverException_RemoteUriAccessForbidden {
            get {
                return ResourceManager.GetString("ContentRetrieverException_RemoteUriAccessForbidden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remote Url &quot;{0}&quot; does not exist..
        /// </summary>
        internal static string ContentRetrieverException_RemoteUriDoesNotExist {
            get {
                return ResourceManager.GetString("ContentRetrieverException_RemoteUriDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} starting at line &quot;{1}&quot;, column &quot;{2}&quot;, is invalid:
        ///{3}.
        /// </summary>
        internal static string FlexiBlocksException_InvalidFlexiBlock {
            get {
                return ResourceManager.GetString("FlexiBlocksException_InvalidFlexiBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The markdown at line &quot;{0}&quot;, column &quot;{1}&quot; is invalid:
        ///{2}.
        /// </summary>
        internal static string FlexiBlocksException_InvalidMarkdown {
            get {
                return ResourceManager.GetString("FlexiBlocksException_InvalidMarkdown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FlexiOptionsBlock must immediately precede the block that consumes it..
        /// </summary>
        internal static string FlexiBlocksException_MispositionedFlexiOptionsBlock {
            get {
                return ResourceManager.GetString("FlexiBlocksException_MispositionedFlexiOptionsBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse JSON &quot;{0}&quot;. Refer to the inner exception for more details..
        /// </summary>
        internal static string FlexiBlocksException_UnableToParseJson {
            get {
                return ResourceManager.GetString("FlexiBlocksException_UnableToParseJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FlexiOptionsBlock must be consumed..
        /// </summary>
        internal static string FlexiBlocksException_UnconsumedFlexiOptionsBlock {
            get {
                return ResourceManager.GetString("FlexiBlocksException_UnconsumedFlexiOptionsBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected exception occurred. Refer to the inner exception for more details..
        /// </summary>
        internal static string FlexiBlocksException_UnexpectedException {
            get {
                return ResourceManager.GetString("FlexiBlocksException_UnexpectedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected exception occurred in &quot;{0}&quot; while attempting to open a block. Refer to the inner exception for more details..
        /// </summary>
        internal static string FlexiBlocksException_UnexpectedExceptionWhileAttemptingToOpenBlock {
            get {
                return ResourceManager.GetString("FlexiBlocksException_UnexpectedExceptionWhileAttemptingToOpenBlock", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request to &quot;{0}&quot; failed with status code &quot;{1}&quot; and message &quot;{2}&quot;..
        /// </summary>
        internal static string HttpRequestException_UnsuccessfulRequest {
            get {
                return ResourceManager.GetString("HttpRequestException_UnsuccessfulRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following cycle was found in the includes tree: 
        ///{0}.
        /// </summary>
        internal static string InvalidOperationException_CycleInIncludes {
            get {
                return ResourceManager.GetString("InvalidOperationException_CycleInIncludes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid clipping, no line contains end line substring &quot;{0}&quot;..
        /// </summary>
        internal static string InvalidOperationException_InvalidClippingNoLineContainsEndLineSubstring {
            get {
                return ResourceManager.GetString("InvalidOperationException_InvalidClippingNoLineContainsEndLineSubstring", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid clipping, no line contains start line substring &quot;{0}&quot;..
        /// </summary>
        internal static string InvalidOperationException_InvalidClippingNoLineContainsStartLineSubstring {
            get {
                return ResourceManager.GetString("InvalidOperationException_InvalidClippingNoLineContainsStartLineSubstring", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The highlight line range &quot;{0}&quot; is not a subset of the actual number of lines, &quot;{1}&quot;..
        /// </summary>
        internal static string InvalidOperationException_InvalidHighlightLineRange {
            get {
                return ResourceManager.GetString("InvalidOperationException_InvalidHighlightLineRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The line number line range &quot;{0}&quot; is not a subset of the actual number of lines, &quot;{1}&quot;..
        /// </summary>
        internal static string InvalidOperationException_InvalidLineNumberLineRange {
            get {
                return ResourceManager.GetString("InvalidOperationException_InvalidLineNumberLineRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid line number ranges: the line number ranges &quot;{0}&quot; and &quot;{1}&quot; overlap..
        /// </summary>
        internal static string InvalidOperationException_LineNumbersCannotOverlap {
            get {
                return ResourceManager.GetString("InvalidOperationException_LineNumbersCannotOverlap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid line ranges for highlighting: the ranges &quot;{0}&quot; and &quot;{1}&quot; overlap..
        /// </summary>
        internal static string InvalidOperationException_LineRangesForHighlightingCannotOverlap {
            get {
                return ResourceManager.GetString("InvalidOperationException_LineRangesForHighlightingCannotOverlap", resourceCulture);
            }
        }
    }
}
