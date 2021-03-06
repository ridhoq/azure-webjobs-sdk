﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading;

namespace Microsoft.Azure.WebJobs.Host
{
    /// <summary>
    /// Configuration options for controlling function execution timeout behavior.
    /// </summary>
    public class JobHostFunctionTimeoutOptions
    {
        /// <summary>
        /// Gets the timeout value.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether function invocations will timeout when
        /// a <see cref="Timeout"/> is specified and a debugger is attached. False by default.
        /// </summary>
        public bool TimeoutWhileDebugging { get; set; }

        /// <summary>
        /// When true, an exception is thrown when a function timeout expires.
        /// </summary>
        public bool ThrowOnTimeout { get; set; }

        /// <summary>
        /// The amount of time to wait between canceling the timeout <see cref="CancellationToken"/> and throwing
        /// a FunctionTimeoutException. This gives functions time to perform any graceful shutdown. 
        /// Only applies if <see cref="ThrowOnTimeout"/> is true.
        /// </summary>
        public TimeSpan GracePeriod { get; set; }

        internal TimeoutAttribute ToAttribute()
        {
            return new TimeoutAttribute(this.Timeout.ToString())
            {
                TimeoutWhileDebugging = this.TimeoutWhileDebugging,
                ThrowOnTimeout = this.ThrowOnTimeout,
                GracePeriod = this.GracePeriod,
            };
        }
    }
}
