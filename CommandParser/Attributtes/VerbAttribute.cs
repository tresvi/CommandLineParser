using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CommandParser.Attributtes
{
    [AttributeUsage(AttributeTargets.Property , AllowMultiple = false, Inherited = false)]
    public class VerbAttribute : BaseArgumentAttribute
    {
        public VerbAttribute(bool isRequired, string helpText = "")
            :base("", "", isRequired, helpText)
        {


        }

        internal override Argument DetectKeyword(List<string> CLI_Arguments)
        {

            throw new NotImplementedException();
        }

        internal override void ParseAndAssign(PropertyInfo property, object targetObject, List<string> CLI_Arguments, ref List<string> ControlCLI_Arguments)
        {
            Type tipo = property.GetType();

            foreach (int algo in Enum.GetValues(tipo))
            {
                Debug.WriteLine($"ENUM: {algo.ToString()}");
            }
/*
            foreach (PropertyInfo property in targetObject.GetType().GetProperties())
            {
                foreach (BaseArgumentAttribute attribute in property.GetCustomAttributes(typeof(BaseArgumentAttribute), true))
                {
                    attribute.ParseAndAssign(property, targetObject, CLI_Arguments, ref ControlCLI_Arguments);
                }
            }
*/

            string verbo = CLI_Arguments[0];
            /*
            if (property.GetType() is typeof(Enum))
                Debug.WriteLine("Es enum!!!");
            else
                Debug.WriteLine("NOO Es enum!!!");
            */

            /*    if (CLI_Arguments.Count == 0)
                    if (this.IsDefaultVerb)

                verbName = CLI_Arguments()
                throw new NotImplementedException();
            */
        }
    }
}
