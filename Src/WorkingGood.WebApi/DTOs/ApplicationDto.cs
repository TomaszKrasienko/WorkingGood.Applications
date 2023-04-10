using System;
namespace WorkingGood.WebApi.DTOs
{
	public class ApplicationDto
	{
		public string? CandidateFirstName { get; set; }
        public string? CandidateLastName { get; set; }
        public string? CandidateEmail { get; set; }
        public string? Description { get; set; }
        public string? Document { get; set; }
    }
}

