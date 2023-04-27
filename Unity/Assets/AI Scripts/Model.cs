using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using System.Linq;
using UnityEngine;
using System.Net.Mail;

public class Model 
{
    private UdpClient client;
    IPEndPoint ep;
    IPEndPoint rep;

    public Model()
    {
        client = new UdpClient();
        ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
        rep = new IPEndPoint(IPAddress.Any, 0);
    }

    public void Send(byte[] bytes)
    {
        client.Send(bytes, bytes.Length, ep);
    }

    public byte[] Receive()
    {
        byte[] receivedData = client.Receive(ref rep);

        return receivedData;
    }

    public byte[] ConvertToBytes(List<float> state, byte prepend_byte, List<float> q_values)
    {
        //Get length of state
        byte[] state_length = BitConverter.GetBytes(state.Count);
        //Get length of q values
        byte[] q_length = BitConverter.GetBytes(q_values.Count);

        //Convert state to bytes 
        byte[] state_bytes = state.SelectMany(BitConverter.GetBytes).ToArray();
        //Convert q values to bytes 
        byte[] q_bytes = q_values.SelectMany(BitConverter.GetBytes).ToArray();

        //Concatenate prepend byte, state length, q length, state, q values together
        byte[] packet = new byte[] { prepend_byte }.Concat(state_length).Concat(q_length).Concat(state_bytes).Concat(q_bytes).ToArray();

        return packet;
    }

    public List<float> ConvertToFloats(byte[] bytes)
    {
        //Convert byte prediction to list of floats 

        List<float> floats = new List<float>();

        for (int i = 0; i < bytes.Length; i += 4)
        {
            floats.Add(BitConverter.ToSingle(bytes, i));
        }

        return floats;
    }

    //In: State space
    //Out: Nothing
    public void Fit(List<float> previous_state, List<float> q_values)
    {
        //Convert state to bytes   
        byte prepend_byte = 1; //Byte is 1 for training    
        Send(ConvertToBytes(previous_state, prepend_byte, q_values)); //Send current state to server
    }

    //In: State space
    //Out: Actions (Expected reward)
    public List<float> Predict(List<float> state)
    {
        List<float> empty_q = new List<float>();

        //Convert state to bytes  
        byte prepend_byte = 0; //Byte is 0 for prediction     
        Send(ConvertToBytes(state, prepend_byte, empty_q)); //Send current state to server 

        //Receive prediction from model
        byte[] byte_prediction = Receive();
        
        //Pass prediction to agent
        return ConvertToFloats(byte_prediction);
    }
}
