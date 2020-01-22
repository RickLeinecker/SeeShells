using SeeShells.ShellParser.parent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    public class Event : Block, IEvent
    {
        IList<IShellItem> parent = new List<IShellItem>(); 
        
        public virtual List<IShellItem> Parent
        {
            get
            {
                return parent;
            }
            protected set
            {

            }
        }
        public virtual string Name
        {
            get
            {
                if(parent.Contains(Name))
                    return Name;
                return null;
            }
            protected set
            {

            }
        }
        public virtual String TypeName
        {
            get
            {
                if (parent.Contains(TypeName))
                    return TypeName;
                return null;
            }
        }
        public virtual DateTime ModifiedDate
        {
            get
            {
                if(parent.Contains(ModifiedDate))
                    return ModifiedDate;
                return null;
            }
            protected set
            {

            }
        }
        public virtual DateTime AccessedDate
        {
            get
            {
                if(parent.Contains(AccessedDate))
                    return AccessedDate();
                return null;
            }
            protected set
            {

            }
        }
        public virtual DateTime CreationDate
        {
            get
            {
                if(parent.Contains(CreationDate))
                    return CreationDate();
                return null;
            }
            protected set
            {

            }
        }
        public virtual String EventType
        {
            get
            {
                IDictionary<String, String> parser = IShellItem.GetAllProperties();
                foreach (var item in parser)
                {
                    if (item.Key == "ModifiedDate")
                    {
                        String temp = item.Key;
                        String[] type = temp.Split("D");
                        return type[0];
                    }
                    if (item.Key == "AccessedDate")
                    {
                        String temp = item.Key;
                        String[] type = temp.Split("D");
                        return type[0];
                    }
                    if (item.Key == "CreationDate")
                    {
                        String temp = item.Key;
                        String[] type = temp.Split("D");
                        return type[0];
                    }
                }
                return "";
            }
            protected set
            {

            }
        }
     
        

    }
}