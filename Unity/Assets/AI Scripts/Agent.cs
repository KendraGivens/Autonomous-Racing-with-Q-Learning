using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using TMPro;

//Interfaces with the server 
//
public class Agent : MonoBehaviour
{
    public TextMeshProUGUI text_reward;
    public TextMeshProUGUI text_decelerate;
    public TextMeshProUGUI text_no_accelerate;
    public TextMeshProUGUI text_accelerate;
    public TextMeshProUGUI text_turnl;
    public TextMeshProUGUI text_nturn;
    public TextMeshProUGUI text_turnr;

    public static bool AI_Controlled = true;

    protected Model model = new Model();
    public CarController2 agent;

    System.Random random = new System.Random();
    
    //States
    List<float> previous_state = new List<float>();
    List<float> current_state = new List<float>();

    List<float> previous_predicted_actions = new List<float>();
    float previous_action_value = 0f;
    int previous_action_index = 0;

    List<float> current_predicted_actions = new List<float>();
    float current_action_value = 0f;
    float max_current_action_value = 0f;
    int current_action_index = 0;

    float Q_Value = 0f;

    //Hyperparameters
    public float gamma = 0.75f;
    public float epsilon = 0.1f;
    public float alpha = 0.01f;
    float probability = 0f;

    float previous_distance = -1f;
    float distance = 0f;
    float change = 0f;

    //Actions
    int acceleration = 0;
    int turning_angle = 0;

    //Reward
    float reward = 0f;

    int frame = 0;

    public List<float> StridedRange(List<float> list, int start, int stride, int num_elements)
    {
        List<float> subset = new List<float>();

        for (int i = start; i <= num_elements*stride + start; i+=stride) 
        {
            subset.Add(list[i]);
        }

        return subset;
    }

    public float CalculateReward(List<float> state)
    {
        float reward = 0f;

        //Unpack list into the variables
        (float m_steeringAngle, float velocity_x, float velocity_z, float fl_hit,
        float fr_hit, float l_hit, float r_hit, float rl_hit, float rr_hit, float x, float y, float z) = (state[0], state[1], state[2], state[3], state[4], state[5], state[6], state[7], state[8], state[9], state[10], state[11]);

        float speed_reward = (float)Math.Sqrt(Math.Abs((double)velocity_z));

        float straight_reward = 0;
        if (velocity_z < 0)
        {
            straight_reward = .2f * Math.Abs(velocity_z);
        }

        distance = (float) Math.Sqrt(x*x + y*y + z*z);
        change = previous_distance - distance;

        if (previous_distance < 0)
        {
            change = 0;
        }

        previous_distance = distance;

        //Calculate reward
        reward = agent.collision_reward + speed_reward + agent.checkpoint_reward - distance / 5;

        return reward; 
    }

    public float CalculateQ(float q_p, float m_q_c, float r)
    {
        //Q(s, A) = Q(s, A) + alpha[reward + gamma*max_A(Q(s, A') - Q(s, A)]
        //Value of previous s/a = value of previous s/a + lr * reward for current s/a + df * current s/a - previous s/a
        float q_value = q_p + alpha * (r + gamma * m_q_c - q_p);
        //UnityEngine.Debug.Log("Previous: " + q_p.ToString());
        //UnityEngine.Debug.Log("Current: " + m_q_c.ToString());

        return q_value;
    }

    public void TakeAction()
    {
        if (!AI_Controlled)
        {
            agent.m_horizontalInput = Input.GetAxis("Horizontal");
            agent.m_verticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            agent.m_horizontalInput = turning_angle;
            agent.m_verticalInput = acceleration;
        }
    }

    void FixedUpdate()
    {
        if (!AI_Controlled)
        {
            TakeAction();
            return;
        }

        if (frame % 20 != 0)
        {
            frame++;
            return;
        }

        //Get current state after taking actions
        current_state = agent.GetState();

        //Get reward for taking those actions 
        reward = CalculateReward(current_state);

        //Calculate action for current state
        current_predicted_actions = model.Predict(current_state);
        max_current_action_value = current_predicted_actions.Max();

        string s = "";
        foreach (var item in current_predicted_actions)
        {
            s += item.ToString() + ", ";
            
        }
        //UnityEngine.Debug.Log(s);

        //Get probability of taking a random action
        probability = (float)(random.NextDouble());

        //Take random action
        bool random_action = false;
        if (probability <= epsilon)
        {
            random_action = true;
            current_action_index = random.Next(0, 9);
        }
        else
            current_action_index = current_predicted_actions.IndexOf(max_current_action_value);

        current_action_value = current_predicted_actions[current_action_index];

        //Take action 
        acceleration = current_action_index / 3 - 1;
        turning_angle = current_action_index % 3 - 1;
        TakeAction();

        //Call Model.Fit with x as state and y as calculated q value
        if (previous_state.Count != 0)
        {
            //Get Q Value for state/action pair
            Q_Value = CalculateQ(previous_action_value, max_current_action_value, reward);

            previous_predicted_actions[previous_action_index] = Q_Value;

            model.Fit(previous_state, previous_predicted_actions);
        }

        //Set previous state
        previous_state = current_state;
        previous_predicted_actions = current_predicted_actions;
        previous_action_value = current_action_value;
        previous_action_index = current_action_index;
       
        Color32 green = new Color32(0, 255, 0, 255);
        Color32 white = new Color32(255, 255, 255, 255);

        if(random_action)
        {
            green = new Color32(255, 0, 255, 255);
        }

        text_reward.text = "Reward: " + reward.ToString();

        text_decelerate.color = acceleration == -1 ? green : white;
        text_decelerate.text = "Decelerate: " + previous_predicted_actions.GetRange(0, 3).Max().ToString();
        text_no_accelerate.color = acceleration == 0 ? green : white;
        text_no_accelerate.text = "No Acceleration: " + previous_predicted_actions.GetRange(3, 3).Max().ToString();
        text_accelerate.color = acceleration == 1 ? green : white;
        text_accelerate.text = "Accelerate: " + previous_predicted_actions.GetRange(6, 3).Max().ToString();

        text_turnl.color = turning_angle == -1 ? green : white;
        text_turnl.text = "Turn Left: " + StridedRange(previous_predicted_actions, 0, 2, 3).Max().ToString();
        text_nturn.color = turning_angle == 0 ? green : white;
        text_nturn.text = "No Turn: " + StridedRange(previous_predicted_actions, 1, 2, 3).Max().ToString();
        text_turnr.color = turning_angle == 1 ? green : white;
        text_turnr.text = "Turn Right: " + StridedRange(previous_predicted_actions, 2, 2, 3).Max().ToString();


        frame++;
    }
   
}