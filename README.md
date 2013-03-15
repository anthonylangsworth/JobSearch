Job Search Organizer
===

A simple library for organizing a job search (somewhat topical considering I
wrote this between jobs). The intention was to create a simple but potentially
useful application using the Microsoft Entity Framework for serialization, Code Contracts and an onion architecture.  There is no UI but it may be added later.

The library consists of job openings (JobOpening); contacts like HR
representatives, recruiters or hiring managers (Contact) and activities, like
interviews, followups or applying for jobs (Activity). Integration with a
calendar service like Google Calendar or iCloud is an obvious future
enhancement.

The intention is for the user to instantiate a JobOpening, then add activites
and contacts to it. The library offers helpers to add activities in groups with
standardized fields. For example, creating a job interview using the extension
method defind in JobOpeningInterviewExtensions not only adds the interview but
a followup phone call.

The library also demonstrates the use of 
- "Onion" architecture. The JobSearch assembly is the "Core" and implements
  interfaces defined in JobSearch.Interfaces. Serialization is handled in the
  JobSearch.Serialization assembly.
- Entity Framework 4.5 for ORM. It also contains a generic repository
  implementation in the EntityFrameworkRepository class in
  JobSearch.Serialization and ageneric repository test class in the TestEntityFrameworkRepositoryHelper in JobSearch.Serialization.Test.
- Code contracts (in the JobSearch assembly).
- NUnit for tests (JobSearch.Test and JobSearch.Serialization.Test assemblies).


