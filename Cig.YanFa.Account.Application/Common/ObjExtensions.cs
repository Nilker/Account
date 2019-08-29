using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cig.YanFa.Account.Application.Common
{
   
    public static class ObjExtensions
    {

        public static string GetDescription(this object obj, string name)
        {
            var referencedType = obj.GetType();
            return GetDescription(referencedType, name);
        }

        public static string GetDescription(Type referencedType, string propertyName)
        {
            //  gets the properties of the referenced type
            PropertyDescriptorCollection properties= TypeDescriptor.GetProperties(referencedType);

            if (properties != null)
            {

                // gets a PropertyDescriptor to the specific property.
                PropertyDescriptor property = properties[propertyName];
                if (property != null)
                {
                    //  gets the attributes of the required property
                    AttributeCollection attributes = property.Attributes;

                    // Gets the description attribute from the collection.
                    DescriptionAttribute descript = (DescriptionAttribute)attributes[typeof(DescriptionAttribute)];

                    // register the referenced description
                    if (!String.IsNullOrEmpty(descript.Description))
                    { return descript.Description; }
                }

            }

            return string.Empty;
        }

    }
}
