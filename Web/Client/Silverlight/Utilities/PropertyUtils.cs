﻿using System;
using System.Reflection;

namespace ClearCanvas.Web.Client.Silverlight.Utilities
{
    public static class PropertyUtils
    {
        private static void SetProperties(PropertyInfo[] fromFields,
                                       object fromRecord,
                                       object toRecord)
        {
            try
            {
                if (fromFields == null)
                {
                    return;
                }

                foreach (PropertyInfo t in fromFields)
                {
                    PropertyInfo fromField = t;
                    if (fromField.Name == "EntityConflict")
                        continue;  // Entity objects have this field and it throws an exception when copying

                    if (fromField.CanRead && fromField.CanWrite)
                    {
                        fromField.SetValue(toRecord,
                                           fromField.GetValue(fromRecord, null),
                                           null);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Copy(object source, object destination)
        {
            PropertyInfo[] fromFields;

            fromFields = source.GetType().GetProperties();

            SetProperties(fromFields, source, destination);
        }
    }    
}
