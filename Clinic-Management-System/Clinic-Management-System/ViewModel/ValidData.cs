using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_Management_System.ViewModel
{
    public class ValidData
    {
        public string IsNotValid(object obj)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.GetValue(obj) == null)
                {
                    return property.Name;
                }
            }
            return "";
        }
    }
}
