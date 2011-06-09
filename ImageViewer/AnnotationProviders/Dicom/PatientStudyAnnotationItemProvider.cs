#region License

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
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Annotations;
using ClearCanvas.ImageViewer.Annotations.Dicom;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.AnnotationProviders.Dicom
{
	[ExtensionOf(typeof(AnnotationItemProviderExtensionPoint))]
	public class PatientStudyAnnotationItemProvider : AnnotationItemProvider
	{
		private readonly List<IAnnotationItem> _annotationItems;

		public PatientStudyAnnotationItemProvider()
			: base("AnnotationItemProviders.Dicom.PatientStudy", new AnnotationResourceResolver(typeof(PatientStudyAnnotationItemProvider).Assembly))
		{
			_annotationItems = new List<IAnnotationItem>();

			AnnotationResourceResolver resolver = new AnnotationResourceResolver(this);

			_annotationItems.Add
				(
					new DicomAnnotationItem<string>
					(
						"Dicom.PatientStudy.AdditionalPatientsHistory",
						resolver,
						delegate(Frame frame) { return frame.ParentImageSop.AdditionalPatientsHistory; },
						DicomDataFormatHelper.RawStringFormat
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<string>
					(
						"Dicom.PatientStudy.Occupation",
						resolver,
						FrameDataRetrieverFactory.GetStringRetriever(DicomTags.Occupation),
						DicomDataFormatHelper.RawStringFormat
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<string>
					(
						"Dicom.PatientStudy.PatientsAge",
						resolver,
						FrameDataRetrieverFactory.GetStringRetriever(DicomTags.PatientsAge),
						DicomDataFormatHelper.RawStringFormat
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<double>
					(
						"Dicom.PatientStudy.PatientsSize",
						resolver,
						FrameDataRetrieverFactory.GetDoubleRetriever(DicomTags.PatientsSize),
						delegate(double input)
						{
							if (double.IsNaN(input) || input == 0)
								return "";

							return String.Format("{0} {1}", input.ToString("F2"), SR.Label_metres);
						}
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<double>
					(
						"Dicom.PatientStudy.PatientsWeight",
						resolver,
						FrameDataRetrieverFactory.GetDoubleRetriever(DicomTags.PatientsWeight),
						delegate(double input)
						{
							if (double.IsNaN(input) || input == 0)
								return "";

							return String.Format("{0} {1}", input.ToString("F2"), SR.Label_kilograms);
						}
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<string>
					(
						"Dicom.PatientStudy.Composite.PatientsAgeSex",
						resolver,
						delegate(Frame frame)
						{
							var nil = SR.ValueNil;
							var age = !string.IsNullOrEmpty(frame.ParentImageSop.PatientsAge) ? frame.ParentImageSop.PatientsAge : nil;
							var sex = !string.IsNullOrEmpty(frame.ParentImageSop.PatientsSex) ? frame.ParentImageSop.PatientsSex : nil;
							if (age == nil && sex == nil)
								return nil;
							return string.Format(SR.FormatPatientsAgeSex, age, sex);
						},
						DicomDataFormatHelper.RawStringFormat
					)
				);

			_annotationItems.Add
				(
					new DicomAnnotationItem<string>
					(
						"Dicom.PatientStudy.Composite.PatientsAgeSexBirthDate",
						resolver,
						delegate(Frame frame)
						{
							var nil = SR.ValueNil;
							var age = !string.IsNullOrEmpty(frame.ParentImageSop.PatientsAge) ? frame.ParentImageSop.PatientsAge : nil;
							var sex = !string.IsNullOrEmpty(frame.ParentImageSop.PatientsSex) ? frame.ParentImageSop.PatientsSex : nil;
							var dob = !string.IsNullOrEmpty(frame.ParentImageSop.PatientsBirthDate) ? DicomDataFormatHelper.DateFormat(frame.ParentImageSop.PatientsBirthDate) : nil;
							if (age == nil && sex == nil && dob == nil)
								return nil;
							return string.Format(SR.FormatPatientsAgeSexBirthDate, age, sex, dob);
						},
						DicomDataFormatHelper.RawStringFormat
					)
				);
		}

		public override IEnumerable<IAnnotationItem> GetAnnotationItems()
		{
			return _annotationItems;
		}
	}
}
