﻿namespace ProjectManagment.Data
{
    public abstract class IssuesElement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
