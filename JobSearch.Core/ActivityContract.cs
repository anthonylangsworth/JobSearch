using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// Code contracts for <see cref="IActivity"/>.
    /// </summary>
    [ContractClassFor(typeof(IActivity))]
    internal abstract class ActivityContract: IActivity
    {
        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(Contact != null,
                "Contact cannot be null");
            Contract.Invariant(Duration == Duration.Duration(),
                "Duration must be positive");
        }

        /// <summary>
        /// The date/time this activity starts. 
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// The name of this activity.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// How long the activity takes.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// The person involved in the activity.
        /// </summary>
        public IContact Contact { get; private set; }

        /// <summary>
        /// Is the activity completed or done?
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; private set; }
    }
}
