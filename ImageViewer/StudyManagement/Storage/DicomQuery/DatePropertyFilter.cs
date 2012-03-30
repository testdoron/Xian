﻿using System;
using System.Linq;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.ImageViewer.StudyManagement.Storage.DicomQuery
{
    internal abstract class DatePropertyFilter<T> : PropertyFilter<T>
    {
        private bool _isRange;
        private DateTime? _date1;
        private DateTime? _date2;
        private bool _parsedCriterion;

        protected DatePropertyFilter(DicomTagPath path, DicomAttributeCollection criteria)
            : base(path, criteria)
        {
            Platform.CheckTrue(path.ValueRepresentation.Name == "DA", "Path is not VR=DA");
            if (Criterion != null)
                Platform.CheckTrue(Criterion.Tag.VR.Name == "DA", "Criteria is not VR=DA");
        }

        public DateTime? Date1
        {
            get
            {
                if (!_parsedCriterion)
                    ParseCriterion();
                
                return _date1;
            }
        }

        public DateTime? Date2
        {
            get
            {
                if (!_parsedCriterion)
                    ParseCriterion();

                return _date2;
            }
        }

        private void ParseCriterion()
        {
            _parsedCriterion = true;
            DateRangeHelper.Parse(Criterion.GetString(0, ""), out _date1, out _date2, out _isRange);
        }

        protected virtual IQueryable<T> AddEqualsToQuery(IQueryable<T> query, DateTime date)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddLessOrEqualToQuery(IQueryable<T> query, DateTime date)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddGreaterOrEqualToQuery(IQueryable<T> query, DateTime date)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddBetweenDatesToQuery(IQueryable<T> query, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected sealed override IQueryable<T> AddToQuery(IQueryable<T> query)
        {
            if (Date1 != null && Date2 != null)
                return AddBetweenDatesToQuery(query, Date1.Value, Date2.Value);
            
            if (Date1 != null)
            {
                if (_isRange)
                    return AddGreaterOrEqualToQuery(query, Date1.Value);

                return AddEqualsToQuery(query, Date1.Value);
            }

            if (Date2 != null)
                return AddLessOrEqualToQuery(query, Date2.Value);

            return base.AddToQuery(query);
        }

        protected sealed override System.Collections.Generic.IEnumerable<T> FilterResults(System.Collections.Generic.IEnumerable<T> results)
        {
            throw new NotSupportedException("Any date filtering supported is done in the database only.");
        }
    }
}
