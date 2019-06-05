using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.FileServer.Helper.GhostScriptSharp
{
    /// <summary>
    /// Output document physical dimensions
    /// </summary>
    public class GhostscriptPageSize
    {
        private GhostscriptPageSizes _fixed;
        private System.Drawing.Size _manual;

        /// <summary>
        /// Custom document size
        /// </summary>
        public System.Drawing.Size Manual
        {
            set
            {
                this._fixed = GhostscriptPageSizes.UNDEFINED;
                this._manual = value;
            }
            get
            {
                return this._manual;
            }
        }

        /// <summary>
        /// Standard paper size
        /// </summary>
        public GhostscriptPageSizes Native
        {
            set
            {
                this._fixed = value;
                this._manual = new System.Drawing.Size(0, 0);
            }
            get
            {
                return this._fixed;
            }
        }
    }
}
