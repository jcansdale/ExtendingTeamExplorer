/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/

// Guids.cs
// MUST match guids.h
using System;

namespace Microsoft.TeamExplorerSample
{
    static class GuidList
    {
        public const string guidTeamExplorerSamplePkgString = "3643d296-bd3d-4115-86aa-77a176c068d6";
        public const string guidTeamExplorerSampleCmdSetString = "2168ae06-b783-42df-b585-d6fec91f727e";

        public static readonly Guid guidTeamExplorerSampleCmdSet = new Guid(guidTeamExplorerSampleCmdSetString);
    };
}