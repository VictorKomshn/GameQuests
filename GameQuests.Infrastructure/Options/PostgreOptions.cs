﻿namespace GameQuests.Infrastructure.Options
{
    internal class PostgreOptions
    {
        public const string SectionName = "Postgres";

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }
    }
}
