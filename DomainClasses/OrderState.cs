using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainClasses
{
    public class OrderState
    {
        /// <summary>
        /// Constructs the OrderState object
        /// </summary>
        public OrderState(int stateId)
        {
            this._stateId = stateId;
        }

        /// <summary>
        /// The State of the order as a string
        /// </summary>
        public string State { get { return Enum.GetName(typeof(OrderStates), _stateId); } }

        /// <summary>
        /// The State ID of the order
        /// </summary>
        private int _stateId;

        /// <summary>
        /// The State ID of the order
        /// </summary>
        public int StateId 
        { 
            get { return _stateId; } 
            set { _stateId = value; } 
        }

        /// <summary>
        /// Sets the state ID to Complete
        /// </summary>
        public void Complete()
        {
            this.StateId = 4;
        }

        /// <summary>
        /// Sets the state ID to Rejected
        /// </summary>
        public void Reject()
        {
            this.StateId = 3;
        }

        /// <summary>
        /// Sets the state ID to Submitted
        /// </summary>
        public void Submit()
        {
            this.StateId = 2;
        }
    }
}