# Autonomous-Racing-with-Q-Learning

## AI Anarchy

### Opening the project in Unity3d

- Run the command git clone
- Open the Unity Editor
- Click open project
- Navigate to the directory where the repository is stored
- Select the Unity folder and click open

The main menu screen will be presented first if the user is in the racing UI scene. The user can select either the Start button or the AI Console button. The start button will allow the user to control the car movements using the arrow keys. The forward and backward keys will allow the car to move forward or reverse. The left and right arrow keys will allow the user to move left and right. 

If the user chooses the AI Console, it will open up the coastal track scene and the car will drive on its own. The car is learning how to drive around the track through Q-learning. This mode requires a server. 

In either mode, the user can press P to pause the game which will also stop all music. From the pause menu, the user can exit to the main menu where they can choose different mode. 

There is an error that pops up saying the Fody package is malfunctioning, but it can be ignored and the game will still run.

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

