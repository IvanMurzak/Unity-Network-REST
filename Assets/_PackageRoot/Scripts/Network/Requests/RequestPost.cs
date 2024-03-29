﻿using System;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;

namespace Network.Extension
{
    public abstract class RequestPost<T1, T2> : Request<T2>
    {
        public      override    string          RESTMethod  => UnityWebRequest.kHttpVerbPOST;

        public                  T1              data;

        public RequestPost(NetworkSO network, T1 data) : base(network)
        {
            this.data = data;
        }

        protected override UnityWebRequest CreateUnityWebRequest(string endpoint)
        {
            var json        = JsonConvert.SerializeObject(data);
            var byteData    = Encoding.UTF8.GetBytes(json);

#if UNITY_EDITOR
#pragma warning disable CS0168 // Variable is declared but never used
            try                     { Debug.Log($"JSON {RESTMethod}:\n\n{JsonPrettify(json)}\n"); }
            catch (Exception e)     { Debug.Log($"JSON {RESTMethod}:\n\n{json}\n"); }
#pragma warning restore CS0168 // Variable is declared but never used
#endif

            return new UnityWebRequest(endpoint)
            {
                method          = RESTMethod,
                uploadHandler   = new UploadHandlerRaw(byteData),
                downloadHandler = new DownloadHandlerBuffer()
            };
        }
    }

    public abstract class RequestPost<T1> : Request<T1>
    {
        public      override    string          RESTMethod  => UnityWebRequest.kHttpVerbPOST;

        public RequestPost(NetworkSO network) : base(network)
        {
        }

        protected override UnityWebRequest CreateUnityWebRequest(string endpoint)
        {
            return new UnityWebRequest(endpoint)
            {
                method          = RESTMethod,
                downloadHandler = new DownloadHandlerBuffer()
            };
        }
    }
}
