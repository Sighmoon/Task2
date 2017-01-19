using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace QueueFunc
{
    public class QueueObj
    {
        private IConnection connection;
        public IModel channel;

        private string queueName;
        private string hostName;
        private string exchange;
        private string login;
        private string password;
        private string port;
        private bool isDurable;
        private bool isExcuclusive;
        private bool isAutoDelete;

        static public QueueObj GetQueueObj(QueueParams param)
        {
            var connectionParams = new ConnectionFactory { HostName = param.HostName };
            QueueObj Qobj = new QueueObj();
            Qobj.connection = connectionParams.CreateConnection();
            Qobj.channel = Qobj.connection.CreateModel();
            Qobj.SettingsSetUp(param);
            Qobj.channel.QueueDeclare(param.QueueName, param.IsDurable, param.IsExcuclusive, param.IsAutoDelete, null);
            Qobj.channel.BasicQos(0, 1, false);
            return Qobj;
        }

        public string ConsumeOneMessage()
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);
            BasicDeliverEventArgs deliveryArguments;
            bool isConsumed = consumer.Queue.Dequeue(5000, out deliveryArguments);
            if(isConsumed==true)
            {
                string message = Encoding.UTF8.GetString(deliveryArguments.Body);
                channel.BasicAck(deliveryArguments.DeliveryTag, false);
                return message;
            }
            else
            {
                return null;
            }
        }

        public void ConsumeMessages<T>(Action<T> messageProccess, bool isInfinite)
        {
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);
            while (true)
            {
                BasicDeliverEventArgs deliveryArgs;
                bool isConsumed = consumer.Queue.Dequeue(7000, out deliveryArgs);
                if(isConsumed == true)
                {
                    string message = Encoding.UTF8.GetString(deliveryArgs.Body);
                    try
                    {
                        messageProccess(new Deserializer<T>().Deserialize(message));
                        channel.BasicAck(deliveryArgs.DeliveryTag, false);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        channel.BasicReject(deliveryArgs.DeliveryTag, false);
                    }
                }
                else
                {
                    if (isInfinite == false)
                        break;
                }
            }
        }

        public void PublishMessage<T>(T message)
        {
            var body = Encoding.UTF8.GetBytes(Convert.ToString(message));
            channel.BasicPublish(exchange, queueName, null, body);
        }

        public void PublishGroupOfMessages<T>(T[] messages)
        {
            foreach(T message in messages)
            {
                PublishMessage<T>(message);
            }
        }

        private void SettingsSetUp(QueueParams param)
        {
            queueName = param.QueueName;
            hostName = param.HostName;
            exchange = param.Exchange;
            login = param.Login;
            password = param.Password;
            port = param.Port;
            isDurable = param.IsDurable;
            isAutoDelete = param.IsAutoDelete;
            isExcuclusive = param.IsExcuclusive;
        }
    }

    public class QueueParams
    {
        public QueueParams(string hostName, string queueName)
        {
            HostName = hostName;
            IsAutoDelete = false;
            IsExcuclusive = false;
            IsDurable = true;
            Login = "guest";
            Password = "guest";
            QueueName = queueName;
            Exchange = "";
        }

        public QueueParams(string hostName, string queueName, string login, string password)
        {
            HostName = hostName;
            QueueName = queueName;
            Login = login;
            Password = password;
            IsAutoDelete = false;
            IsExcuclusive = false;
            IsDurable = true;
            Exchange = "";
        }
        public string HostName { get; set; }
        public string QueueName { get; set; }
        public string Exchange { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public bool IsDurable { get; set; }
        public bool IsExcuclusive { get; set; }
        public bool IsAutoDelete { get; set; }
    }

    public class Deserializer<T>
    {
        Dictionary<Type, Func<string, T>> @switch = new Dictionary<Type, Func<string, T>>
        {
            { typeof(string), (message) => (T) Convert.ChangeType(message as string, typeof(T)) },
            { typeof(int), (message) => (T) Convert.ChangeType(Convert.ToInt32(message), typeof(T)) },
            { typeof(long), (message) => (T) Convert.ChangeType(Convert.ToInt64(message), typeof(T)) }
        };

        public T Deserialize(string message)
        {
            if (@switch.ContainsKey(typeof(T)))
                return @switch[typeof(T)](message);
            else
                return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
