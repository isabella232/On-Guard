﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnGuardCore.Src.Properties;

using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Options;
using MQTTnet.Exceptions;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet;
using System.Threading;
using System.IO;

namespace OnGuardCore
{
  public class MQTTPublish : IDisposable
  {
    static MqttClient s_client;
    private bool disposedValue;
    static bool _loggedError = false;
    static bool _loggedNotConnected = false;

    static MQTTPublish()
    {
#pragma warning disable CS4014 
      CreateClient();
#pragma warning restore CS4014 
    }


    public static async Task CreateClient()
    {
      var factory = new MqttFactory();
      s_client = (MqttClient)factory.CreateMqttClient();
      // await Connect();

    }

    public static async Task Publish(string camera, AreaOfInterest area, Frame frame, InterestingObject io)
    {
      bool result = true;
      _loggedError = false;
      _loggedNotConnected = false;

      if (area != null && frame != null)
      {
        string baseTopic = Storage.Instance.GetGlobalString("MQTTMotionTopic");
        baseTopic = baseTopic.Replace("{Camera}", camera);
        baseTopic = baseTopic.Replace("{Area}", area.AOIName);
        baseTopic = baseTopic.Replace("{Motion}", "on");

        string topic = baseTopic;
        topic = topic.Replace("{Object}", io.Label);

        string payload = Storage.Instance.GetGlobalString("MQTTMotionPayload");
        payload = payload.Replace("{Confidence}", ((int)(io.Confidence * 100.0)).ToString());

        if (payload.Contains("{Image}"))
        {
          payload = payload.Replace("{Image}", LoadImage(frame.Item.PendingFile));
        }

        payload = payload.Replace("{Object}", io.Label);
        payload = payload.Replace("{Motion}", "on");

        string tmp = frame.Item.PendingFile;
        bool jsonFormat = Storage.Instance.GetGlobalBool("JSONFormat");
        if (jsonFormat)
        {
          tmp = tmp.Replace(@"\", @"\\");
        }
      
        payload = payload.Replace("{File}", tmp);

        await SendToServer(topic, payload).ConfigureAwait(true);
      }
    }



    public static async Task SendToServer(string topic, string payload)
    {
      int connectTryCount = 0;

      while (connectTryCount < 2 && !s_client.IsConnected)
      {
        if (!_loggedNotConnected)
        {
          Dbg.Write(LogLevel.Warning, "MQTTPublish - Client NotConnected");
          _loggedNotConnected = true;
        }

        await Connect().ConfigureAwait(true);
        connectTryCount++;
        if (!s_client.IsConnected)
        {
          Task.Delay(1000 * 2);
        }
      }

      if (s_client.IsConnected == false)
      {
        if (!_loggedNotConnected)
        {
          Dbg.Write(LogLevel.Warning, "MQTTPublish - Client NotConnected");
          _loggedNotConnected = true;
        }
      }
      else
      {
        _loggedNotConnected = false;
        _loggedError = false;

        var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();


        try
        {
          await s_client.PublishAsync(message).ConfigureAwait(true);
          Dbg.Write(LogLevel.Info, "MQTT Message Sent");
        }
        catch (Exception ex)
        {
          Dbg.Write(LogLevel.Warning, "MQTTPublish - Error publishing message: " + ex.Message);
          try
          {
            if (s_client.IsConnected)
            {
              await s_client.DisconnectAsync();
            }
          }
          catch (Exception)
          {
            Dbg.Write(LogLevel.Info, "MQTTPublish - Exception closing connection after publish error - client still connected");
            // Expected if the publish failed (but, probably not if the client still thinks it is connected).
          }
        }
      }
    }

    public static async Task Connect()
    {

      string clientId = "OnGuard";
      string mqttURI = Storage.Instance.GetGlobalString("MQTTServerAddress");
      string mqttUser = Storage.Instance.GetGlobalString("MQTTUser");
      string mqttPassword = Storage.Instance.GetGlobalString("MQTTPassword");
      int mqttPort = Storage.Instance.GetGlobalInt("MQTTPort");
      bool mqttSecure = Storage.Instance.GetGlobalBool("MQTTUseSecureLink");


      var messageBuilder = new MqttClientOptionsBuilder()
        .WithClientId(clientId)
        .WithCredentials(mqttUser, mqttPassword)
        .WithTcpServer(mqttURI, mqttPort)
        .WithCleanSession();

      var options = mqttSecure
        ? messageBuilder
          .WithTls()
          .Build()
        : messageBuilder
          .Build();



      if (s_client.IsConnected == false)
      {
        try
        {
          MqttClientAuthenticateResult result = await s_client.ConnectAsync(options, CancellationToken.None).ConfigureAwait(true);
          if (result.ResultCode == MqttClientConnectResultCode.Success)
          {
            Dbg.Write(LogLevel.DetailedInfo, "MQTTPublish - Connected to server!");
            _loggedError = false;
            _loggedNotConnected = false;
          }
          else
          {
            HandleError(result);  // Here we let it go setup the disconnect handler -- things may improve in the future
          }
        }
        catch (MqttCommunicationException ex)
        {
          if (!_loggedNotConnected)
          {
            Dbg.Write(LogLevel.Warning, "MQTTPublish - The Connection to server failed - it is unreachable - check your server status and your MQTT settings: " + ex.Message);
            _loggedNotConnected = true;
          }

          return;

        }
        catch (Exception ex)
        {
          if (!_loggedNotConnected)
          {
            Dbg.Write(LogLevel.Warning, "MQTTPublish - Connection to server failed: " + ex.Message);
            _loggedNotConnected = true;
          }
          return;
        }
      }

    }

    static void HandleError(MqttClientAuthenticateResult result)
    {
      string err = string.Empty;

      switch (result.ResultCode)
      {
        case MqttClientConnectResultCode.BadUserNameOrPassword:
          err = "MQTTPublish - Connect - Invalid User Name or Password!";
          DisposeInstance();
          break;

        case MqttClientConnectResultCode.BadAuthenticationMethod:
          err = "MQTTPublish - Connect - Bad authentication method - secure connection type invaid?  " + result.ResultCode.ToString();
          DisposeInstance();
          break;

        case MqttClientConnectResultCode.NotAuthorized:
          err = "MQTTPublish - Connect - User Not Authorized: " + result.ResultCode.ToString();
          DisposeInstance();
          break;

        case MqttClientConnectResultCode.ServerUnavailable:
          err = "MQTTPublish - Connect - The server is unavailable";
          break;

        default:
          err = "MQTTPublish - Connect Error - Error Code: " + result.ResultCode.ToString();
          break;
      }

      if (!_loggedError)
      {
        Dbg.Write(LogLevel.Warning, err);
      }

      _loggedError = true;

    }

    static string LoadImage(string path)
    {
      string result = string.Empty;

      if (File.Exists(path))
      {
        int retryCount = 0;
        byte[] image = null;
        while (retryCount < 2)
        {
          try
          {
            image = File.ReadAllBytes(path);
            break;
          }
          catch (IOException)
          {
            // case seen when the file was in use
            ++retryCount;
            Thread.Sleep(200);
          }
        }

        if (retryCount < 2)
        {
          // success!
          result = Convert.ToBase64String(image);
        }
      }

      return result;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          s_client.Dispose();
        }

        disposedValue = true;
      }
    }

    private static void DisposeInstance()
    {
      if (null != s_client)
      {
        Dbg.Write(LogLevel.Warning, "MQTTPublish - Disposing - Cannot Continue");
        s_client.Dispose();
        s_client = null;
      }
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
