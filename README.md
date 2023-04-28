# AI Anarchy

## Autonomous Racing with Q Learning

Demo Video: https://drive.google.com/file/d/1H-8_xKAzMcxwsz5zbRbSEiHNYJkLN09Y/view?usp=sharing

### Opening the project in Unity3d

- Open git bash or a terminal
- Run the command git clone https://github.com/KendraGivens/Autonomous-Racing-with-Q-Learning.git
- Open the Unity Editor
- Click open project
- Navigate to the directory where the repository is stored
- Select the Unity folder and click open

### Opening the Main Menu Scene
- Go to the project folder in Unity
- Open up the Scenes folder
- To view the Main Menu Scene open up the RacingUI scene

![image](https://user-images.githubusercontent.com/100613566/235009727-1ec64896-079f-4475-b419-b00cc60356ec.png)

### Get User Control
- Click User Controlled to get to a user controlled car that can move through arrow keys

### Get AI Control
- Click on the AI Controlled button
- Follow the directions below to set up the server enviroment
- Start the server before pressing play in the Unity Engine

### Configure server enviroment
The enviroment requires the Python 3.8 DLL. It must be set up as an enviroment variable with a path such as C:\Python38

https://www.python.org/downloads/release/python-3810/

Open up the NuGet packet manager in visual studio

Install the following packages: 
- Keras.Net
- TensorFlow.Keras
- TensorFlow.Net
- SciSharp.Tensorflow.Redist
- Numpy.Bare

Once it has been configured, you can run the server. It might take a minute to load as Keras is slow. It should look like this once it loads. 
Once it has loaded, pressing play in the Unity Engine will start up the AI

![image](https://user-images.githubusercontent.com/100613566/234760953-fea2ed6d-971a-427f-b676-300fb58b7ca2.png)

