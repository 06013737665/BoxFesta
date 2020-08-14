using System;
using System.Reflection;
using System.Web.Mvc;

namespace Box.Festa.Attribute
{
    /// <summary>
    /// Class MultipleButtonAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        /// <summary>
        /// Obtém ou define o nome.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtém ou define o argumento.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// Determines whether the action name is valid in the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="methodInfo">Information about the action method.</param>
        /// <returns>true if the action name is valid in the specified controller context; otherwise, false.</returns>
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", this.Name, this.Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[this.Name] = this.Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
}