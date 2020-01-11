
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    public interface IShellItem
    {
        ushort Size { get; }
        byte Type { get; }
        /// <summary>
        /// Human readable interpretation of  <see cref="Type"/>
        /// </summary>
        string TypeName { get; }
        string Name { get; }
        DateTime ModifiedDate { get; }
        DateTime AccessedDate { get; }
        DateTime CreationDate { get; }

        /// <summary>
        /// This function returns all properties that exist in the ShellItem
        /// in Key-Value Format. Implementations of this method should append
        /// the base implementation and append any additional fields as created.
        /// </summary>
        /// <returns>A Dictionary with all gettable properties in string form.</returns>
        IDictionary<string, string> GetAllProperties();

    }
}
