using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;

namespace ClearCanvas.Desktop.Actions
{
    /// <summary>
    /// Represents an action path.  An action path specifies the location of an action within
    /// a menu or toolbar hierarchy.
    /// </summary>
    public class ActionPath
    {
        const char SEPARATOR = '/';

        /// <summary>
        /// Creates a new <see cref="ActionPath"/> from the specified path string, resolving
        /// resource keys in the path string using the specified <see cref="ResourceResolver"/>.
        /// </summary>
        /// <remarks>
        /// The path string may contain any combination of literals and resource keys.  Localization
        /// will be attempted on each path segment by treating the segment as a resource key,
        /// and path segments that fail as resource keys will be treated as literals.
        /// </remarks>
        /// <param name="path">The path string to parse</param>
        /// <param name="resolver">The <see cref="ResourceResolver"/> to use for localization</param>
        /// <returns>A new <see cref="ActionPath"/> object</returns>
        public static ActionPath ParseAndLocalize(string path, ResourceResolver resolver)
        {
            string[] parts = path.Split(new char[] { SEPARATOR });

            int n = parts.Length;
            ActionPathSegment[] segments = new ActionPathSegment[n];
            for (int i = 0; i < n; i++)
            {
                segments[i] = new ActionPathSegment(parts[i], resolver.Resolve(parts[i]));
            }

            return new ActionPath(segments);
        }


        private ActionPathSegment[] _segments;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="segments"></param>
        internal ActionPath(ActionPathSegment[] segments)
        {
            _segments = segments;
        }

        /// <summary>
        /// The set of individual segments contained in this path.
        /// </summary>
        public ActionPathSegment[] Segments
        {
            get { return _segments; }
            set { _segments = value; }
        }

        /// <summary>
        /// The final segment in this path, or null if this path is empty
        /// </summary>
        public ActionPathSegment LastSegment
        {
            get { return _segments.Length > 0 ? _segments[_segments.Length - 1] : null; }
        }

        /// <summary>
        /// Converts this path back to the original string from which it was created.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ActionPathSegment segment in _segments)
            {
                if (sb.Length > 0)
                    sb.Append(SEPARATOR);
                sb.Append(segment.ResourceKey);
            }
            return sb.ToString();
        }
    }
}
