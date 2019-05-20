/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace Microsoft.TeamExplorerSample
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Get the VersionControlExt extensibility object.
        /// </summary>
        public static VersionControlExt GetVersionControlExt(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                DTE2 dte = serviceProvider.GetService(typeof(SDTE)) as DTE2;
                if (dte != null)
                {
                    return dte.GetObject("Microsoft.VisualStudio.TeamFoundation.VersionControl.VersionControlExt") as VersionControlExt;
                }
            }

            return null;
        }
    }
}
