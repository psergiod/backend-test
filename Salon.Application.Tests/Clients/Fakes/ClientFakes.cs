using MongoDB.Bson;
using Salon.Domain.Clients.Contracts;
using Salon.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using ClientEntity = Salon.Domain.Clients.Entities.Client;

namespace Salon.Application.Tests.Clients.Fakes
{
    public static class ClientFakes
    {
        public const string NAME_JAMES = "James Jhonson";
        public static DateTime BIRTHDATE_JAMES = new DateTime(1976, 01, 02).ToUniversalTime();
        public static string CONTACTNUMBER_JAMES = "3024-5504";
        public static string EMAIL_JAMES = "james.jhonsons@hotmail.com";

        public static ObjectId IdMike = new ObjectId("649205a22524c80b79e0d1c6");
        public const string NAME_MIKE = "Mike Smith";
        public static DateTime BIRTHDATE_MIKE = new DateTime(1978, 02, 04).ToUniversalTime();
        public static string CONTACTNUMBER_MIKE = "3024-6543";
        public static string EMAIL_MIKE = "mike.smith@hotmail.com";

        public static ObjectId IdBob = new ObjectId("649205a22524c80b79e0d1cb");
        public const string NAME_BOB = "Bob Dylan";
        public static DateTime BIRTHDATE_BOB = new DateTime(1988, 10, 06).ToUniversalTime();
        public static string CONTACTNUMBER_BOB = "3024-8844";
        public static string EMAIL_BOB = "bob.dylan@hotmail.com";

        public static ObjectId IdTony = new ObjectId("649205a22524c80b79e0d1c7");
        public const string NAME_TONY = "Tony Stark";
        public const string NEW_NAME_TONY = "Anthony Stark";
        public static DateTime BIRTHDATE_TONY = new DateTime(1994, 12, 16).ToUniversalTime();
        public static string CONTACTNUMBER_TONY = "3024-4646";
        public static string EMAIL_TONY = "tony.stark@hotmail.com";

        public static ClientEntity GetClientEntity(string name, DateTime birthDate, string contactNumber, string email, ObjectId? id = null)
        {
            var client = new ClientEntity(name)
                .InformBirthDate(birthDate)
                .InformContactNumber(contactNumber)
                .InformEmail(email);
            client.Id = id ?? ObjectId.Empty;

            return client;
        }
        public static ClientCommand GetClientCommand()
            => new ClientCommand()
            {
                Name = NAME_JAMES,
                BirthDate = BIRTHDATE_JAMES.Date,
                ContactNumbers = new List<string> { CONTACTNUMBER_JAMES },
                Email = EMAIL_JAMES,
                Gender = Gender.Male,
            };

        public static UpdateClientCommand GetBobUpdateCommand(string id = null)
            => new UpdateClientCommand()
            {
                Name = NAME_BOB,
                BirthDate = BIRTHDATE_BOB.Date,
                ContactNumbers = new List<string> { CONTACTNUMBER_BOB },
                Email = EMAIL_BOB,
                Gender = Gender.Male,
                Id = id
            };

        public static UpdateClientCommand GetTonyUpdateCommand()
            => new UpdateClientCommand()
            {
                Name = NEW_NAME_TONY,
                BirthDate = BIRTHDATE_TONY.Date,
                ContactNumbers = new List<string> { CONTACTNUMBER_TONY },
                Email = EMAIL_TONY,
                Gender = Gender.Male,
                Id = IdTony.ToString()
            };

        public static ClientResponse GetClientResponseBob()
            => new ClientResponse()
            {
                Id = IdBob.ToString(),
                Name = NAME_BOB,
                BirthDate = BIRTHDATE_BOB.Date,
                ContactNumbers = new List<string> { CONTACTNUMBER_BOB },
                Email = EMAIL_BOB,
                Gender = Gender.Male
            };

        public static ClientEntity GetClientMike()
            => GetClientEntity(NAME_MIKE, BIRTHDATE_MIKE, CONTACTNUMBER_MIKE, EMAIL_MIKE, IdMike);

        public static ClientEntity GetClientBob()
           => GetClientEntity(NAME_BOB, BIRTHDATE_BOB, CONTACTNUMBER_BOB, EMAIL_BOB, IdBob);

        public static ClientEntity GetClientJames()
            => GetClientEntity(NAME_JAMES, BIRTHDATE_JAMES, CONTACTNUMBER_JAMES, EMAIL_JAMES);

        public static ClientEntity GetClientTony()
            => GetClientEntity(NAME_TONY, BIRTHDATE_TONY, CONTACTNUMBER_TONY, EMAIL_TONY,IdTony);

        public static ClientEntity GetClientTonyUpdated()
    => GetClientEntity(NEW_NAME_TONY, BIRTHDATE_TONY, CONTACTNUMBER_TONY, EMAIL_TONY, IdTony);
    }
}
