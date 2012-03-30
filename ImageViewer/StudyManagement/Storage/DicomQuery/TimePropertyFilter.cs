﻿using System;
using System.Linq;
using ClearCanvas.Dicom.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Common;

namespace ClearCanvas.ImageViewer.StudyManagement.Storage.DicomQuery
{
    internal abstract class TimePropertyFilter<T> : PropertyFilter<T>
    {
        private bool _isRange;
        private long? _time1Ticks;
        private long? _time2Ticks;
        private bool _parsedCriterion;

        protected TimePropertyFilter(DicomTagPath path, DicomAttributeCollection criteria)
            : base(path, criteria)
        {
            Platform.CheckTrue(path.ValueRepresentation.Name == "TM", "Path is not VR=TM");
            if (Criterion != null)
                Platform.CheckTrue(Criterion.Tag.VR.Name == "TM", "Criteria is not VR=TM");
        }

        public long? Time1Ticks
        {
            get
            {
                if (!_parsedCriterion)
                    ParseCriterion();

                return _time1Ticks;
            }
        }

        public long? Time2Ticks
        {
            get
            {
                if (!_parsedCriterion)
                    ParseCriterion();

                return _time2Ticks;
            }
        }

        private void ParseCriterion()
        {
            //TODO (Marmot): We've never supported time queries before.
            _parsedCriterion = true;

            //DateTime? time1, time2;
            //TimeParser.Parse(Criterion.GetString(0, ""), out time1, out time2, out _isRange);
        }

        protected virtual IQueryable<T> AddEqualsToQuery(IQueryable<T> query, long timeTicks)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddLessOrEqualToQuery(IQueryable<T> query, long timeTicks)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddGreaterOrEqualToQuery(IQueryable<T> query, long timeTicks)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected virtual IQueryable<T> AddBetweenTimesToQuery(IQueryable<T> query, long startTimeTicks, long endTimeTicks)
        {
            throw new NotImplementedException("If AddToQueryEnabled is true, this must be implemented.");
        }

        protected sealed override IQueryable<T> AddToQuery(IQueryable<T> query)
        {
            if (Time1Ticks != null && Time2Ticks != null)
                return AddBetweenTimesToQuery(query, Time1Ticks.Value, Time2Ticks.Value);

            if (Time1Ticks != null)
            {
                if (_isRange)
                    return AddGreaterOrEqualToQuery(query, Time1Ticks.Value);

                return AddEqualsToQuery(query, Time1Ticks.Value);
            }

            if (Time2Ticks != null)
                return AddLessOrEqualToQuery(query, Time2Ticks.Value);

            return base.AddToQuery(query);
        }

        protected sealed override System.Collections.Generic.IEnumerable<T> FilterResults(System.Collections.Generic.IEnumerable<T> results)
        {
            throw new NotSupportedException("Any time filtering supported is done in the database only.");
        }
    }
}