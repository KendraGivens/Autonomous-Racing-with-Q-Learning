using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Tensorflow;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Python.Runtime;
using Numpy;
using System;
using System.Net.Sockets;
using System.Collections;
using System.Net.Mail;
using static Tensorflow.GraphTransferInfo.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using static Tensorflow.CollectionDef.Types;

namespace ConsoleApp7
{
    internal class Server
    { 
        public static List<float> ConvertToFloats(byte[] bytes)
        {
            //Convert byte prediction to list of floats 

            List<float> floats = new List<float>();

            for (int i = 0; i < bytes.Length; i+=4)
            {
                floats.Add(BitConverter.ToSingle(bytes, i));
            }

            return floats;
        }

        static void Main(string[] args)
        {
            using (Py.GIL())
            {
                //Operation bytes that says whether to train or 
                const byte PREDICT = 0;
                const byte FIT = 1;

                //Model
                var state_input = new Input(shape: new Keras.Shape(12));
                var h1 = new Dense(64, activation: "gelu").Set(state_input);
                var h2 = new Dense(64, activation: "gelu").Set(h1);
                var actions_output = new Dense(9, activation: "linear").Set(h2);

                var model = new Keras.Models.Model(new Input[] { state_input }, new BaseLayer[] { actions_output });
                model.Summary();
                model.Compile(optimizer: "Adam", loss: "mse");

                //model.LoadWeight("model.h5");

                UdpClient udpServer = new UdpClient(1234);
                Console.WriteLine("UDP server started on port 1234");

                while (true)
                {
                    IPEndPoint remoteEP = null;
                    byte[] received_data = udpServer.Receive(ref remoteEP);
                    int data_length = received_data.Length;

                    //Get operation byte
                    byte chosen_operation = received_data[0];

                    //Get length of state
                    byte[] state_length_bytes = new byte[4];
                    Array.Copy(received_data, 1, state_length_bytes, 0, 4);
                    int state_length_int = BitConverter.ToInt32(state_length_bytes, 0);

                    //Get length of q values
                    byte[] q_length_bytes = new byte[4];
                    Array.Copy(received_data, 5, q_length_bytes, 0, 4);
                    int q_length_int = BitConverter.ToInt32(q_length_bytes, 0);

                    //Get state
                    byte[] state_bytes = new byte[state_length_int * 4];
                    System.Buffer.BlockCopy(received_data, 9, state_bytes, 0, state_length_int * 4);
                    List<float> state = ConvertToFloats(state_bytes);

                    //Get q values
                    byte[] q_values_bytes = new byte[q_length_int * 4];
                    System.Buffer.BlockCopy(received_data, 9 + state_length_int * 4, q_values_bytes, 0, q_length_int * 4);
                    List<float> q_values = ConvertToFloats(q_values_bytes);
           
                    NDarray x = np.expand_dims(np.array(state.ToArray()), 0);
                    NDarray y = np.expand_dims(np.array(q_values.ToArray()), 0);

                    if (chosen_operation == PREDICT)
                    {
                        float[] result = model.Predict(x, verbose: 0).GetData<float>();

                        byte[] predictions = result.SelectMany(BitConverter.GetBytes).ToArray();

                        udpServer.Send(predictions, predictions.Length, remoteEP);
                    }
                    if (chosen_operation == FIT)
                    {
                       model.Fit(x, y, batch_size: 1, epochs: 1, verbose: 0);
                        model.SaveWeight("model.h5");
                    }
                   


                }
            }
        }
    }
}