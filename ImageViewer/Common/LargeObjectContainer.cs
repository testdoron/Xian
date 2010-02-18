﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Threading;

namespace ClearCanvas.ImageViewer.Common
{
	/// <summary>
	/// Indicator of the relative cost to regenerate an object were the <see cref="MemoryManager"/> to unload it.
	/// </summary>
	public enum RegenerationCost
	{
		/// <summary>
		/// Fairly low cost to regenerate.
		/// </summary>
		Low = 0,
		/// <summary>
		/// Average cost to regenerate.
		/// </summary>
		Medium = 1,
		/// <summary>
		/// Very high cost to regenerate.
		/// </summary>
		High = 2
	}

	/// <summary>
	/// Simple data class describing the contents of an <see cref="ILargeObjectContainer"/>.
	/// </summary>
	public class LargeObjectContainerData : ILargeObjectContainer
	{
		private readonly Guid _identifier;
		private int _lockCount;
		private volatile int _largeObjectCount;
		private long _totalBytesHeld;
		private volatile RegenerationCost _regenerationCost;
		private DateTime _lastAccessTime;
		private uint _lastAccessTimeAccuracyMilliseconds = 500;
		private int _lastAccessUpdateTickCount;

		/// <summary>
		/// Constructor.
		/// </summary>
		public LargeObjectContainerData(Guid identifier)
		{
			_identifier = identifier;
		}

		#region ILargeObjectContainer Members

		/// <summary>
		/// Gets the unique identifier for the container.
		/// </summary>
		public Guid Identifier
		{
			get { return _identifier; }
		}

		/// <summary>
		/// Gets or sets the total number of large objects held by the container.
		/// </summary>
		/// <remarks>A large object is typically a large array, like a byte array.</remarks>
		public int LargeObjectCount
		{
			get { return _largeObjectCount; }
			set { _largeObjectCount = value; }
		}

		/// <summary>
		/// Gets or sets the total number of bytes held by the container.
		/// </summary>
		public long BytesHeldCount
		{
			get { return _totalBytesHeld; }
			set { _totalBytesHeld = value; }
		}

		/// <summary>
		/// Gets the precision/accuracy to be used when updating
		/// the <see cref="LastAccessTime"/>, via a call to <see cref="UpdateLastAccessTime"/>.
		/// </summary>
		/// <remarks>
		/// Repeated calls to <see cref="DateTime.Now"/> can be extremely expensive, especially when
		/// called in a tight loop.  The <see cref="LastAccessTime"/> doesn't typically need to be
		/// all that accurate, and this property helps to minimize the number of calls to <see cref="DateTime.Now"/>.
		/// </remarks>
		public uint LastAccessTimeAccuracyMilliseconds
		{
			get { return _lastAccessTimeAccuracyMilliseconds; }
			set { _lastAccessTimeAccuracyMilliseconds = value; }
		}

		/// <summary>
		/// Gets the last time the container was accessed.
		/// </summary>
		/// <remarks>
		/// The default <see cref="IMemoryManagementStrategy"/> uses a "least recently used" approach
		/// when deciding which <see cref="ILargeObjectContainer"/>s to unload.
		/// </remarks>
		public DateTime LastAccessTime
		{
			get { return _lastAccessTime; }
		}

		/// <summary>
		/// Gets or sets the <see cref="RegenerationCost"/>, which is a relative value intended to 
		/// give the <see cref="IMemoryManagementStrategy">memory management strategy</see> a hint
		/// when deciding which containers to unload.
		/// </summary>
		public RegenerationCost RegenerationCost
		{
			get { return _regenerationCost; }
			set { _regenerationCost = value; }
		}

		/// <summary>
		/// Gets whether or not the container is locked.  See <see cref="Lock"/> for details.
		/// </summary>
		public bool IsLocked
		{
			get { return Thread.VolatileRead(ref _lockCount) > 0; }
		}

		/// <summary>
		/// Updates the <see cref="LastAccessTime"/> with the 
		/// precision/accuracy of <see cref="LastAccessTimeAccuracyMilliseconds"/>.
		/// </summary>
		public void UpdateLastAccessTime()
		{
			//DateTime.Now is extremely expensive if called in a tight loop, so we minimize the potential impact
			//of this problem occurring by only updating the last access time every second or so.
			if (Environment.TickCount - _lastAccessUpdateTickCount < _lastAccessTimeAccuracyMilliseconds)
				return;

			_lastAccessUpdateTickCount = Environment.TickCount;
			_lastAccessTime = DateTime.Now;
		}

		/// <summary>
		/// Locks the container.
		/// </summary>
		/// <remarks>
		/// <para>
		///	The <see cref="IMemoryManagementStrategy">memory management strategy</see> will look at the <see cref="IsLocked"/> property
		/// when deciding whether or not to unload the container.  The <see cref="IMemoryManagementStrategy">memory management strategy</see>
		/// could still call <see cref="ILargeObjectContainer.Unload"/> if more memory is needed.
		/// </para>
		/// <para>
		/// Make sure to call <see cref="Unlock"/> once for every call to <see cref="Lock"/>.
		/// </para>
		/// </remarks>
		public void Lock()
		{
			Interlocked.Increment(ref _lockCount);
		}

		/// <summary>
		/// Unlocks the container.  See <see cref="Lock"/> for details.
		/// </summary>
		public void Unlock()
		{
			Interlocked.Decrement(ref _lockCount);
		}

		/// <summary>
		/// Unloads the container.
		/// </summary>
		/// <remarks>
		/// Although the <see cref="IMemoryManagementStrategy">memory management strategy</see> can try to unload
		/// the container, it is still ultimately up to the container itself whether or not it actually responds to
		/// the request.  Ideally, the container should always try to unload when the request is made.
		/// </remarks>
		public void Unload()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}

	/// <summary>
	/// Defines an object that is a container for one or more "large objects".  See <see cref="MemoryManager"/> for more details.
	/// </summary>
	public interface ILargeObjectContainer
	{
		/// <summary>
		/// Gets the unique identifier for the container.
		/// </summary>
		Guid Identifier { get; }

		/// <summary>
		/// Gets the total number of large objects held by the container.
		/// </summary>
		/// <remarks>A large object is typically a large array, like a byte array.</remarks>
		int LargeObjectCount { get; }
		
		/// <summary>
		/// Gets the total number of bytes held by the container.
		/// </summary>
		long BytesHeldCount { get; }

		/// <summary>
		/// Gets the last time the container was accessed.
		/// </summary>
		/// <remarks>
		/// The <see cref="DefaultMemoryManagementStrategy"/> uses a "least recently used" approach
		/// when deciding which <see cref="ILargeObjectContainer"/>s to unload.
		/// </remarks>
		DateTime LastAccessTime { get; }

		/// <summary>
		/// Gets or sets the <see cref="RegenerationCost"/>, which is a relative value intended to 
		/// give the <see cref="IMemoryManagementStrategy"/> a hint when deciding which containers to unload.
		/// </summary>
		RegenerationCost RegenerationCost { get; }

		/// <summary>
		/// Gets whether or not the container is locked.  See <see cref="Lock"/> for details.
		/// </summary>
		bool IsLocked { get; }

		/// <summary>
		/// Locks the container.
		/// </summary>
		/// <remarks>
		/// <para>
		///	The <see cref="IMemoryManagementStrategy">memory management strategy</see> will look at the <see cref="IsLocked"/> property
		/// when deciding whether or not to unload the container.  The <see cref="IMemoryManagementStrategy">memory management strategy</see>
		/// could still call <see cref="ILargeObjectContainer.Unload"/> if more memory is needed.
		/// </para>
		/// <para>
		/// Make sure to call <see cref="Unlock"/> once for every call to <see cref="Lock"/>.
		/// </para>
		/// </remarks>
		void Lock();

		/// <summary>
		/// Unlocks the container.  See <see cref="Lock"/> for details.
		/// </summary>
		void Unlock();

		/// <summary>
		/// Unloads the container.
		/// </summary>
		/// <remarks>
		/// Although the <see cref="IMemoryManagementStrategy">memory management strategy</see> can try to unload
		/// the container, it is still ultimately up to the container itself whether or not it actually responds to
		/// the request.  Ideally, the container should always try to unload when the request is made.
		/// </remarks>
		void Unload();
	}
}
