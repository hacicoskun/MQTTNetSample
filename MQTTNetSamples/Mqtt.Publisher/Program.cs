using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

var mqttFactory = new MqttFactory();
var mqttClient = mqttFactory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost", 1883)
    .WithClientId("publisher_client")
    .WithCredentials("admin", "Password123!")
    .Build();

await mqttClient.ConnectAsync(options);

// Your publish logic here
var message = new MqttApplicationMessageBuilder()
    .WithTopic("values")
    .WithPayload("test message")
    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
    .WithRetainFlag()
    .Build();

await mqttClient.PublishAsync(message);

await mqttClient.DisconnectAsync();
 