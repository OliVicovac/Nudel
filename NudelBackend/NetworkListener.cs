﻿using JustConnect.Tcp;
using Newtonsoft.Json;
using Nudel.Networking.Requests;
using Nudel.Networking.Responses;
using Nudel.Networking.Responses.Base;
using System;
using System.Net.Sockets;

namespace Nudel.Backend
{
    public class NetworkListener
    {
        private const int PORT = 8181;
        private Server server;
        private NudelService nudel;
        private readonly JsonSerializerSettings jsonSettings;

        public NetworkListener()
        {
            server = new Server(PORT);
            nudel = new NudelService();
            jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            server.Received += ProcessRequest;
        }

        public void Start()
        {
            server.Start();
        }

        public void Stop()
        {
            server.Stop();
        }

        public void SendResponse(Response response, Socket clientSocket)
        {
            string responseString = JsonConvert.SerializeObject(response);
            server.Send(responseString, clientSocket);
        }

        private void ProcessRequest(string data, Socket clientSocket)
        {
            Object rawRequest = JsonConvert.DeserializeObject<Object>(data, jsonSettings);

            Console.WriteLine($"Received Request of Type: {rawRequest.GetType()}");

            if (rawRequest is RegisterRequest)
            {
                RegisterRequest request = rawRequest as RegisterRequest;

                string sessionToken = nudel.Register(
                    request.Username,
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName
                );

                SendResponse(new LoginResponse(sessionToken), clientSocket);
            }
            else if (rawRequest is LoginRequest)
            {
                LoginRequest request = rawRequest as LoginRequest;

                string sessionToken = nudel.Login(request.UsernameOrEmail, request.Password);

                SendResponse(new LoginResponse(sessionToken), clientSocket);
            }
            else if (rawRequest is CreateEventRequest)
            {
                CreateEventRequest request = rawRequest as CreateEventRequest;

                nudel.CreateEvent(
                    request.Title,
                    request.Description,
                    request.Time,
                    request.Location,
                    request.Options
                );
            }
        }
    }
}
