# Autonomous-Racing-with-Q-Learning

## AI Anarchy

### Opening the project in Unity3d

- Run the command git clone
- Open the Unity Editor
- Click open project
- Navigate to the directory where the repository is stored
- Select the Unity folder and click open

### Opening the Main Menu Scene
- Go to the project folder in Unity
- Open up the Scenes folder
- To view the Main Menu Scene open up the RacingUI scene

### Get User Control
- Go to the inspector and click on the white car object
- Ensure that the AI Controlled checkbox shown below is unchecked
- Click play
- Two buttons: Start and AI Console will be shown
- Click Start to get to a user controlled car that can move through arrow keys

![image](https://user-images.githubusercontent.com/100613566/235009727-1ec64896-079f-4475-b419-b00cc60356ec.png)

![image](https://user-images.githubusercontent.com/100613566/235009488-4a878554-ba9a-4e67-ad85-6f858a902807.png)

### Get AI Control
There is currently a bug in the AI Console button, so to access the AI control, open up the AI scene
- Click on the car object and ensure that AI Controlled is checked
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

