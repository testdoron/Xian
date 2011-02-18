#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Web.UI;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities;
using ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Code;
using SR = ClearCanvas.ImageServer.Web.Application.App_GlobalResources.SR;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Controls
{
    /// <summary>
    /// Patient summary information panel within the <see cref="StudyDetailsPanel"/> 
    /// </summary>
    public partial class PatientSummaryPanel : UserControl
    {
        #region private members
        private PatientSummary _patientSummary;
        #endregion private members


        #region Public Properties
        /// <summary>
        /// Gets or sets the <see cref="PatientSummary"/> object used by the panel.
        /// </summary>
        public PatientSummary PatientSummary
        {
            get { return _patientSummary; }
            set { _patientSummary = value; }
        }

        #endregion Public Properties


        #region Protected methods

        public override void DataBind()
        {
            base.DataBind();
            if (_patientSummary != null)
            {

                personName.PersonName = _patientSummary.PatientName;
                PatientDOB.Value = _patientSummary.Birthdate;
				if (!String.IsNullOrEmpty(_patientSummary.PatientsAge))
				{
                     string patientAge = _patientSummary.PatientsAge.Substring(0, 3).TrimStart('0');

                    switch (_patientSummary.PatientsAge.Substring(3))
                    {
                        case "Y":
                            patientAge += " " + SR.Years;
                            break;
                        case "M":
                            patientAge += " " + SR.Months;
                            break;
                        case "W":
                            patientAge += " " + SR.Weeks;
                            break;
                        default:
                            patientAge += " " + SR.Days;
                            break;
                    }

                    if (_patientSummary.PatientsAge.Substring(0, 3).Equals("001"))
                        patientAge = patientAge.TrimEnd('s');

                    PatientAge.Text = patientAge;
				}
				else
				{
                    PatientAge.Text = ClearCanvas.ImageServer.Web.Application.App_GlobalResources.SR.Years;
				}

            	if (String.IsNullOrEmpty(_patientSummary.Sex))
                    PatientSex.Text = SR.Unknown;
                else
                {
                    if (_patientSummary.Sex.StartsWith("F"))
                        PatientSex.Text = SR.Female;
                    else if (_patientSummary.Sex.StartsWith("M"))
                        PatientSex.Text = SR.Male;
                    else if (_patientSummary.Sex.StartsWith("O"))
                        PatientSex.Text = SR.Other;
                    else
                        PatientSex.Text = SR.Unknown;
                }


                PatientId.Text = _patientSummary.PatientId;

            }

        }

        #endregion Protected methods
    }
}