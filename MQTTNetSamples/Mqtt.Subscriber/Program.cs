using MQTTnet;
using MQTTnet.Client;
using System.Text;

var mqttFactory = new MqttFactory();
var mqttClient = mqttFactory.CreateMqttClient();
var options = new MqttClientOptionsBuilder()
.WithTcpServer("localhost", 1883)
//Broker Ip adresi ve Portu
.WithClientId("publisher_client")
//Brokera yayın yapan veya dinleyen Client Kimliği.
.WithCredentials("admin", "Password123!")
//Brokera bağlanmak için kullanıcı adı ve şifre var ise bu alan gereklidir.
.WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
//Mqtt versiyonunu belirler
.WithWillContentType("text/plain")
// Will mesajı: Mqtt broker yanıt vermediği durumlarda
// son mesaj olarak kabul edilir.
.WithWillPayload("Mqtt sunucusuna bağlantı kesildi.")
// Will mesajının içeriğini belirleme
.WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)

//AtMostOnce  (0): Mesaj, alıcıya en fazla bir kez iletilir. 
//Bu seviye, mesajın en az bir kere alıcıya ulaşmasını garanti etmez.

//AtLeastOnce (1): Mesaj, alıcıya en az bir kez ulaşır. 
//Ancak, alıcı bir onay göndermediğinde, mesaj tekrar gönderilebilir.

//ExactlyOnce (2): Mesaj, tam olarak bir kez alıcıya ulaşır. 
//Bu seviye, bir mesajın sadece bir kez alıcıya teslim edilmesini garanti eder.
.Build();

mqttClient.ApplicationMessageReceivedAsync += e =>
{
    Console.WriteLine($"Received application message. Topic: {e.ApplicationMessage.Topic}, Payload: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");

    return Task.CompletedTask;
};

await mqttClient.ConnectAsync(options);

await mqttClient.SubscribeAsync("values");

Console.ReadLine();

await mqttClient.DisconnectAsync();