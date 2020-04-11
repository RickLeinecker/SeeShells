---
# SeeShells
SeeShells collects ShellBags specific Windows Registry keys and parses through them, and organizes the data found in them to display them on a graphical timeline.\
The graphical timeline is the unique feature that SeeShells offers over other existing parsers: this timeline makes ShellBag data easier to understand and facilitates the process of finding a significant pattern or piece of evidence.

---
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
1. Click on the **Offline Registry** option to enable offline parsing mode.
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

#### Parse All User Accounts
By default, SeeShells will only parse the information from the currently running user account.\
This option enables traversal over the filesystem to retrieve shellbags from other user accounts that are locally on the machine.\
This option also requires that SeeShells be run with administrative privileges due to the need to traverse through all user folders.\
SeeShells does not attempt to traverse over network domains or network locations for finding Shellbag information.

To separate Shellbag data by user account, use the **User Filter** while viewing the Timeline. *(See: Filters – User Filter)*

---
## Viewing the Data
After loading Shellbag data, the timeline will be displayed  and options for filtering  and exporting the shown data will be displayed.

### The Timeline 
The timeline displays Shellbag events by one minute intervals on an event’s respective day. You can navigate this timeline by using the horizontal scrollbar that is below the timeline. 

#### Events
Each event is represented as a single node on the timeline. An event is an action that occurred to the Shellbag with a particular timestamp. *(Example: a Creation event means the Shellbag was recorded with a timestamp to represent the creation date.)*\
To see detailed information about an event, left click on an event and the timeline will display a summary of the event.

An event's summary contains:
-	The name of the event, determined by a unique value within the events. *(example:  If the event was related to a folder, the folder name will be the name of the event.)* 
-	The full timestamp of when the event occurred. In the time zone of the running machine. 
-	The type of event, summarizing what happened at the timestamp. *(ex: Modified - the item represented by the Shellbag was modified)*\
Hovering over an event will expand the summary to show all information recorded about the Shellbag which produced the event.\
Left clicking on the expanded summary will open up the detailed information in another window.\
Right clicking on the event’s summary will give you the option to filter the timeline for events that only came from the Shellbag. *(See Filters: Event Parent Filter)*  

#### Multiple Events
 If multiple events occurred at the same time the events are considered stacked. A number inside the node will represent how many events occurred at that particular point in time. All event summaries will be shown once clicking on the stacked event. 

---
### Filters 

#### Event Date Filter
The timeline will be reduced to only show events that occur after midnight (12:00 AM) on the *Start Date* specified and before 11:59 on the *End Date* specified in the time zone of the computer. 

#### Event Name Filter 
The timeline will filter down to the events that have the specified name, filtering for exact matches only.\
The name of an event can be seen in the summary of an event after clicking on it.

#### Event Content Filter
The timeline will look for any mention of the specified search text inside the Shellbag data and filter down to those events which match any part of the content inside of their respective Shellbag.

Optionally you can use Regex expressions in this search as well. 

#### Event Parent Filter
The timeline will filter out all events that don’t belong to a particular Shellbag.\
This Filter can be used by right-clicking on the summary of an event and choosing **Filter for events with the same parent**. 

#### Event Type Filter
The timeline will only show events in which their type is one of the selected options. Each event’s type can be seen in the event summary. *(ex: Creation, Access)* 

#### User Filter 
This will filter events by the Windows user account(s) which created the Shellbag.  If the user name of the account cannot be determined then the Security Identifier (SID) that uniquely identifies the user will be shown instead.

### Exporting
Below the timeline are options for exporting the events and Shellbag data which creates the timeline.

#### HTML Timeline
The HTML timeline will extract the events shown in the timeline (using any applied filters) and produce an HTML timeline in chronological order. Each event is shown with all of the information from the Shellbag which was used to create it. 

#### CSV Raw Data
All Shellbags which were parsed by the program will be recorded in a Comma-Separated File (CSV) format.\
SeeShells can later use this CSV file to recreate the timeline of events without having to have the original registry present.  *(See: Toolbar – Import CSV)*

---
## Toolbar
The toolbar provides functionality needed throughout the entire application and can be seen at the top of the program.

### View

#### Home
This option will take you to the startup screen for SeeShells where you can parse Shellbags from an online or offline registry.\
It is possible to go back to this page after parsing to parse another file/live hive and the data will be added to the timeline.

#### Timeline Page
This page displays the timeline and the controls to help search through Shellbag data.\
Note: You cannot navigate to this page if Shellbag data has not been parsed or imported. 

### Help

#### Help Page
Checks for updates to the help content and displays this help content.

#### About
Shows information regarding the creation of SeeShells.

### Import CSV
This will allow you to import Shellbag data from a previous run of SeeShells.\
Once a CSV file has been specified the timeline will recreate the events from the file.