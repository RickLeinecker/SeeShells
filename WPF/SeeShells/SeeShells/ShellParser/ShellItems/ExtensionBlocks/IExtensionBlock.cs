using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems.ExtensionBlocks
{
    public interface IExtensionBlock
    {
        ushort Size { get; }
        ushort ExtensionVersion { get; }
        uint Signature { get; }

        /// <summary>
        /// This function returns all properties that exist in the Extension Block
        /// in Key-Value Format. Implementations of this method should append
        /// the base implementation and append any additional fields as created.
        /// </summary>
        /// <returns>A Dictionary with all gettable properties in string form.</returns>
        IDictionary<string, string> GetAllProperties();

    }
}