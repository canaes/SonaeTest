﻿namespace SonaeTestSol.Domain.Models
{
    public class Error
    {
        public string Message { get; }

        public Error(string message)
        {
            Message = message;
        }
    }
}
