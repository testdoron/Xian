﻿#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Enterprise.Desktop;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;

namespace ClearCanvas.Ris.Client
{
	[ExtensionPoint]
	public class PatientProfileSummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	/// <summary>
	/// PatientSummaryComponent class
	/// </summary>
	[AssociateView(typeof(PatientProfileSummaryComponentViewExtensionPoint))]
	public class PatientProfileSummaryComponent : SummaryComponentBase<PatientProfileSummary, PatientProfileTable, TextQueryRequest>
	{
		private string _searchString;

		public PatientProfileSummaryComponent(bool dialogMode)
			: base(dialogMode)
		{
		}

		#region Presentation Model

		public string SearchString
		{
			get { return _searchString; }
			set { _searchString = value; }
		}

		#endregion

		/// <summary>
		/// Override this method to perform custom initialization of the action model,
		/// such as adding permissions or adding custom actions.
		/// </summary>
		/// <param name="model"></param>
		protected override void InitializeActionModel(AdminActionModel model)
		{
			base.InitializeActionModel(model);

			model.Add.SetPermissibility(Application.Common.AuthorityTokens.Workflow.Patient.Create);
			model.Edit.SetPermissibility(Application.Common.AuthorityTokens.Workflow.PatientProfile.Update);
		}

		/// <summary>
		/// Gets the list of items to show in the table, according to the specifed first and max items.
		/// </summary>
		/// <returns></returns>
		protected override IList<PatientProfileSummary> ListItems(TextQueryRequest request)
		{
			// don't execute an open query (it will just return TooManyMatches)
			if(string.IsNullOrEmpty(_searchString))
				return new List<PatientProfileSummary>();

			TextQueryResponse<PatientProfileSummary> response = null;
			Platform.GetService<IRegistrationWorkflowService>(
				service =>
					{
						request.TextQuery = _searchString;
						request.SpecificityThreshold = PatientProfileLookupSettings.Default.QuerySpecificityThreshold;
						response = service.PatientProfileTextQuery(request);
					});

			if (response.TooManyMatches)
				throw new WeakSearchCriteriaException();

			return response.Matches;
		}

		/// <summary>
		/// Called to handle the "add" action.
		/// </summary>
		/// <param name="addedItems"></param>
		/// <returns>True if items were added, false otherwise.</returns>
		protected override bool AddItems(out IList<PatientProfileSummary> addedItems)
		{
			addedItems = new List<PatientProfileSummary>();
			var editor = new PatientProfileEditorComponent();
			var exitCode = LaunchAsDialog(this.Host.DesktopWindow, editor, SR.TitleNewPatient);
			if (exitCode == ApplicationComponentExitCode.Accepted)
			{
				addedItems.Add(editor.PatientProfile.GetSummary());
				return true;
			}
			return false;
		}

		/// <summary>
		/// Called to handle the "edit" action.
		/// </summary>
		/// <param name="items">A list of items to edit.</param>
		/// <param name="editedItems">The list of items that were edited.</param>
		/// <returns>True if items were edited, false otherwise.</returns>
		protected override bool EditItems(IList<PatientProfileSummary> items, out IList<PatientProfileSummary> editedItems)
		{
			editedItems = new List<PatientProfileSummary>();
			var item = CollectionUtils.FirstElement(items);

			var editor = new PatientProfileEditorComponent(item.PatientProfileRef);
			var exitCode = LaunchAsDialog(this.Host.DesktopWindow, editor,
				string.Format(SR.TitleUpdatePatientProfile,
					Formatting.MrnFormat.Format(item.Mrn),
					Formatting.PersonNameFormat.Format(item.Name)));

			if (exitCode == ApplicationComponentExitCode.Accepted)
			{
				editedItems.Add(editor.PatientProfile.GetSummary());
				return true;
			}
			return false;
		}

		protected override bool DeleteItems(IList<PatientProfileSummary> items, out IList<PatientProfileSummary> deletedItems, out string failureMessage)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Compares two items to see if they represent the same item.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected override bool IsSameItem(PatientProfileSummary x, PatientProfileSummary y)
		{
			return x.PatientProfileRef.Equals(y.PatientProfileRef, true);
		}

	}
}
