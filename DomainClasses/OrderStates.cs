using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    /// <summary>
    /// The possible states an order can have
    /// </summary>
    public enum OrderStates
    {
        New = 1,
        Pending = 2,
        Rejected = 3,
        Complete = 4
    }
}