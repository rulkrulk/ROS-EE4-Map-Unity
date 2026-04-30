# ROS Mapping Visualization

A Unity-based visualization tool for ROS 2 3D point cloud and sensor data mapping, bridging compute boards and display boards via ROS Bridge.


## About

This project is designed for **Team Project EE4** and provides a real-time visualization interface for ROS 2-based robotic mapping systems. It enables seamless integration between a ROS compute board (running sensor drivers and mapping algorithms) and a display board (running the Unity visualization client) through ROS TCP Bridge communication.

## Demo

Watch the visualization in action: [ROS_MAPPING.mp4](example_images/ROS_MAPPING.mp4)

## Features

- Real-time 3D point cloud visualization
- Camera stream integration
- OctoMap visualization support
- IMU and odometry data display
- First-person camera controls
- XR/AR ready architecture

## Prerequisites

### On ROS Machine
- **ROS 2 Jazzy** or compatible version
- **OctoMap** or equivalent mapping solution installed
- **OS1 LiDAR driver** (or compatible sensor drivers)
- **rosbag2** for recorded data playback
- Python 3.6+

### On Display Machine (Unity)
- **Unity 2022 LTS** or later
- **TextMesh Pro** (included with Unity)
- **.NET Framework** compatible with your Unity version

## Installation

### 1. ROS Machine Setup

Clone this repository and install ROS dependencies:

```bash
git clone https://github.com/rulkrulk/ROS_MAPPING.git

### 2. Display Machine Setup

1. Clone this repository to your local machine
2. Open the project in Unity (2022 LTS or later)
3. The required packages will be automatically imported from the `Packages/manifest.json`

## Getting Started

### On ROS Device

1. **Source the ROS environment:**
   ```bash
   source /opt/ros/jazzy/setup.bash
   ```

2. **Start the ROS TCP Bridge (this must run first):**
   ```bash
   ros2 run ros_tcp_endpoint default_server_endpoint
   ```

   

3. **Publish static transforms for sensor frames (we are using the Coloradar Dataset):**
   ```bash
   ros2 run tf2_ros static_transform_publisher 0 0 0 0 0 0 os1_sensor os1_lidar
   ros2 run tf2_ros static_transform_publisher 0 0 0 0 0 0 imu_link_enu imu_viz_link
   ```

4. **Run the odometry-to-transform publisher:**
   ```bash
   python3 odom_to_tf.py
   ```

5. **Start your sensor data source (choose one):**

   **Option A: Play from a rosbag file**
   ```bash
   ros2 bag play ec_hallways_run4 --clock
   ```

   **Option B: Run live sensors**
   ```bash
   ros2 launch os_driver driver.launch.py
   ```

### On Display Machine

1. Open the Unity project
2. Press **Play** to start the visualization
3. The application will automatically connect to the ROS Bridge endpoint (If local)
4. Use the **First-Person Camera controls** to navigate the 3D scene

## Project Structure

```
Assets/
├── CameraSubscriber.cs       # Camera stream subscription handler
├── FirstPersonCamera.cs       # Camera control system
├── PointCloud2Visualizer*    # Point cloud rendering
├── Resources/                # Asset resources
├── Scenes/                   # Unity scenes
├── Settings/                 # Configuration settings
├── Shaders/                  # Custom shader graphs
└── TextMesh Pro/             # UI text assets
```

## Configuration

Key settings can be modified in `Assets/PointCloud2VisualizerSettings.asset`:
- ROS Bridge endpoint (default: `localhost:10000`)
- Point cloud visualization parameters
- Camera sensitivity

## Troubleshooting

### Connection Issues
- Ensure ROS TCP Bridge is running on the compute machine
- Check firewall settings and port 10000 accessibility
- Verify network connectivity between machines

### No Point Cloud Appearing
- Verify sensor is publishing to the correct topics
- Check static transforms are published correctly


### No Movement
- Ensure `odom_to_tf.py` is running


## Building for Release

To build a standalone executable:

1. Go to **File → Build Settings** in Unity
2. Configure build platform (PC, WebGL, etc.)
3. Click **Build** and select output directory



