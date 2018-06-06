﻿using MongoDB.Driver;
using Nudel.Backend.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Nudel.Backend
{
    public class NudelService
    {
        private MongoClient mongo;
        private IMongoDatabase db;
        private IMongoCollection<User> userCollection;
        private IMongoCollection<Event> eventCollection;

        public NudelService()
        {
            mongo = new MongoClient(new MongoClientSettings {
                Server = new MongoServerAddress("localhost", 27017)
            });
            db = mongo.GetDatabase("nudel");
            userCollection = db.GetCollection<User>("users");
            eventCollection = db.GetCollection<Event>("events");
        }

        public string Register(string username, string email, string password, string firstName, string lastName)
        {
            long id = userCollection.Count(x=>true) + 1;

            userCollection.InsertOne(new User
            {
                ID = id,
                Username = username,
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            });

            return "1234";
        }

        public string Login(string usernameOrEmail, string password) => throw new NotImplementedException();

        public void CreateEvent(string title, string description, DateTime time, Tuple<double, double> location, List<DateTime> options)
        {
            long id = eventCollection.Count(x => true) + 1;

            eventCollection.InsertOne(new Event
            {
                ID = id,
                Title = title,
                Description = description,
                Time = time,
                Location = location,
                Options = options
            });
        }

        public Event FindEvent(long id)
        {
            var result = eventCollection.Find(x => x.ID == id);

            if (result.Count() != 1)
            {
                return null;
            }
            return result.First();
        }

        public List<Event> FindEvents(string title) => throw new NotImplementedException();

        public void SubscribeEvent(Event @event) => throw new NotImplementedException();

        public void SubscribeEvent(long id) => throw new NotImplementedException();

        public void SubscribeEvent(string title) => throw new NotImplementedException();

        public User FindUser(long id) => throw new NotImplementedException();

        public User FindUser(string usernameOrEmail) => throw new NotImplementedException();

        public void NotifyUser(Event @event, User user) => throw new NotImplementedException();

        public void NotifyUsers(Event @event, List<User> user) => throw new NotImplementedException();
    }
}
