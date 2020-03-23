# SeeShells
SeeShells collects ShellBags specific Windows Registry keys and parses through them, and organizes the data found in them to display them on a graphical timeline.\
The graphical timeline is the unique feature that SeeShells offers over other existing parsers: this timeline makes ShellBag data easier to understand and facilitates the process of finding a significant pattern or piece of evidence.

## How to Use
### Parsing Shellbags on your machine (Online Parsing)
By default SeeShells will use the registry of the currently running Windows machine.\
_(Note: You must be running the program in administrative mode.)_

To analyze Shellbag information for the current PC, click on the **Parse** button.\
SeeShells will automatically configure and parse the relevant registry information.

The timeline will be displayed when the program finishes analyzing. 

### Parsing Shellbags from registry files (Offline Parsing)
SeeShells can analyze registry hives located in **.REG** or **.DAT** file(s) from a Windows machine.

Shellbag information can be found in the following files underneath a Windows user folder (e.g. C:\Users\Username):
- NTUSER.DAT
- USRCLASS.DAT

_(Note: Depending on Windows OS Version either file or both may be present.)_

To perform analysis on a registry hive file:
1. Click on the **Offline** checkbox to enable offline parsing mode.
2. Browse to the location of the offline hive file(s) and select them for opening.
3. Select the Windows operating system which the files selected came from.\
_(See **Advanced Configuration** below if the needed operating system was not in this list.)_
4. Click on the **Parse** button to begin analysis. 

The timeline will be displayed when the program finishes analyzing the file(s). 



### Advanced Configuration
These advanced configurations allow you to modify how SeeShells parses Shellbags. 

Each configuration file can be updated from the internet to retrieve the latest definitions from the maintainers of SeeShells by clicking on the respective **Update File** button.
- These updates are saved to the directory where the SeeShells executable is running with their respective file names below

All files are in JSON notation and can be modified for expanding the capabilities of SeeShells.
 
#### OS Configuration File (_OS.json_)
The OS Configuration file details the Windows operating systems supported for parsing.\
Included in the file is:
- The list of supported operating systems.
- The registry keys where SeeShells will look for Shellbag information with respect to each operating system.
When a new OS Configuration File is selected, the list of OS Versions usable for Offline Parsing is also updated.

#### GUID Configuration File (_GUIDs.json_)
The GUID Configuration file contains one-to-one mappings from various Windows controls and well known folder names to a unique string for identification.\
Because the identifiers for folders and Windows controls are stored as unique identifiers  in Shellbags, SeeShells will use this file show what the Shellbag item was identified as instead of the GUID if a mapping is present.

#### Shell Item Script Configuration File (_Scripts.json_)
The script configuration file contains Lua scripts for parsing ShellItems contents which were discovered after the initial release of SeeShells.\
By updating this file, SeeShells can display more events or more detailed information about known events inside the timeline and while exporting information.