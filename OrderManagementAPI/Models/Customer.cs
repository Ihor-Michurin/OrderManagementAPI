﻿namespace Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int OrderCount { get; set; } = 0;
    }
}