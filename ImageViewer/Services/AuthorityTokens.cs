#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common;
using ClearCanvas.Common.Authorization;

namespace ClearCanvas.ImageViewer.Services
{
    [ExtensionOf(typeof(DefineAuthorityGroupsExtensionPoint), Enabled = false)]
	internal class DefineAuthorityGroups : IDefineAuthorityGroups
	{
		#region IDefineAuthorityGroups Members

		/// <summary>
		/// Get the authority group definitions.
		/// </summary>
		public AuthorityGroupDefinition[] GetAuthorityGroups()
		{
			return new AuthorityGroupDefinition[]
            {
                new AuthorityGroupDefinition(DefaultAuthorityGroups.HealthcareAdministrators,
                    new string[] 
                    {
						AuthorityTokens.Administration.DicomServer,
						AuthorityTokens.Administration.Storage,
						AuthorityTokens.Administration.Services,
						AuthorityTokens.Administration.ReIndex
                    })
            };
		}

		#endregion
	}

	public static class AuthorityTokens
	{
		public static class Administration
		{
		    //TODO (Marmot): Move some of these.
			[AuthorityToken(Description = "Allow administration/configuration of the local DICOM Server (e.g. set AE Title, Port).")]
			public const string DicomServer = "Viewer/Administration/DICOM Server";

            [AuthorityToken(Description = "Allow configuration of local DICOM storage.", Formerly = "Viewer/Administration/Diskspace Manager")]
			public const string Storage = "Viewer/Administration/Storage";

            [AuthorityToken(Description = "Permission to re-index the local file store.", Formerly = "Viewer/Administration/Reindex Local Data Store")]
			public const string ReIndex = "Viewer/Administration/Re-index";

			[AuthorityToken(Description = "Allow administration of the viewer services (e.g. Start/Stop/Restart).")]
			public const string Services = "Viewer/Administration/Services";
		}
	}
}
