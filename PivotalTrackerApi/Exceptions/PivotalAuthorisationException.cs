﻿using System;

namespace DotNetPivotalTrackerApi.Exceptions
{
    public class PivotalAuthorisationException : Exception
    {
        public PivotalAuthorisationException() : base() { }
        public PivotalAuthorisationException(string message) : base(message) { }
        public PivotalAuthorisationException(string message, Exception innerException) :base(message, innerException) { }
    }
}
