using System;

namespace Smartwyre.DeveloperTest.Types
{
    public class IncentiveTypeAttribute : Attribute
    {
        public IncentiveType IncentiveType;

        public IncentiveTypeAttribute(IncentiveType incentiveType)
        {
            IncentiveType = incentiveType;
        }
    }
}
